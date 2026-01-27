using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Alga.IdentityService.Infrastructure.HTTP.Endpoint;

var builder = WebApplication.CreateBuilder(args);


// Logging
// ----------------------------

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var isDebug = builder.Environment.IsDevelopment();

// -------------------------------

const int maxConnections = 500;
int maxRpsPerIp = (int)(maxConnections * 0.8);





// Kestrel Server Configuration
// -------------------------------
// Production-optimized web server setup with security hardening, protocol optimization and connection management for high-load scenarios

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 10 * 1024;
    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(30);
    serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(10);
    serverOptions.Limits.MinRequestBodyDataRate = new MinDataRate(100, TimeSpan.FromSeconds(10));
    serverOptions.Limits.MaxConcurrentConnections = maxConnections;
    serverOptions.AddServerHeader = false;
    serverOptions.ConfigureEndpointDefaults(lo => lo.Protocols = HttpProtocols.Http1AndHttp2);
});





// Global Request Throttling
// -------------------------
// Per-client (IP) token bucket limiter to enforce request rate caps, prevent DoS-like behavior, and maintain service responsiveness under load.

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.CreateChained(
        PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        {
            // Пропускаем health-чекеры
            if (httpContext.Request.Path.StartsWithSegments("/health"))
                return RateLimitPartition.GetNoLimiter("healthchecks");

            // Reject requests with missing IP address
            var ip = httpContext.Connection.RemoteIpAddress?.ToString();
            if (ip is null)
            {
                httpContext.Response.StatusCode = 400;
                return RateLimitPartition.GetNoLimiter("invalid-ip");
            }

            return RateLimitPartition.GetTokenBucketLimiter(partitionKey: ip, factory: _ => new TokenBucketRateLimiterOptions
            {
                // Maximum number of tokens available at once (burst capacity).
                // Allows short spikes of traffic.
                TokenLimit = maxRpsPerIp,

                // Tokens added per replenishment cycle (sustained rate).
                TokensPerPeriod = maxRpsPerIp, //Math.Min(maxRpsPerIp, 100),

                // Interval at which tokens are refilled.
                ReplenishmentPeriod = TimeSpan.FromSeconds(1),

                // How many requests can be queued when tokens run out.
                QueueLimit = 20, // Allows some short waiting

                // Requests are processed in FIFO order from the queue.
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,

                // Tokens are refilled automatically by a background timer.
                AutoReplenishment = true,
            });
        }),

        // Global sliding window limiter (to protect backend globally)
        PartitionedRateLimiter.Create<HttpContext, string>(_ =>
            RateLimitPartition.GetSlidingWindowLimiter(
                "GlobalLimit",
                _ => new SlidingWindowRateLimiterOptions
                {
                    // Total allowed requests per window
                    PermitLimit = maxConnections * 3,

                    // Time window duration
                    Window = TimeSpan.FromSeconds(1),

                    // Subdivision of window (e.g. 2 × 500ms)
                    SegmentsPerWindow = 2,

                    QueueLimit = (int)(maxConnections * 0.02) // Без очереди на глобальном уровне
                }))
    );

    // Respond with HTTP 429 "Too Many Requests" when rate limit is exceeded.
    options.RejectionStatusCode = 429;

    // Optional: Set a Retry-After header to suggest when the client can retry.
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.Headers["Retry-After"] = "10";
        await ValueTask.CompletedTask;
    };
});





// CORS Configuration
// ------------------
// Cross-Origin Resource Sharing (CORS) policies for different environments and projects

builder.Services.AddCors(options =>
{
    options.AddPolicy(Alga.IdentityService.Infrastructure.HTTP.Endpoint.CorsPolicies.OpenForAllCorsPolicy, policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
    options.AddPolicy(Alga.IdentityService.Infrastructure.HTTP.Endpoint.CorsPolicies.OnlyCurrentCorsPolicy, policy => { policy.WithOrigins(isDebug ? "https://localhost:7051" : "https://auth.rt.ink").AllowAnyHeader().AllowAnyMethod(); });
});





// Global Request Timeout Policy
// -----------------------------
// Global request timeout setup to limit request execution time and improve service reliability.
// Defines a default timeout and a named short-timeout policy for specific endpoints.

builder.Services.AddRequestTimeouts(options =>
{
    options.DefaultPolicy = new RequestTimeoutPolicy { Timeout = TimeSpan.FromSeconds(10), TimeoutStatusCode = 503 };
    options.AddPolicy(RequestTimeoutPolicies.S1TimeoutPolicy, TimeSpan.FromSeconds(1));
    options.AddPolicy(RequestTimeoutPolicies.S5TimeoutPolicy, TimeSpan.FromSeconds(5));
});





// Service Settings
// ----------------------------

const string serviceName = "AlgaIdentityService";
const string serviceSettingsSectionName = $"{serviceName}Settings";

var algaIdentityServiceSettingsReq = builder.Configuration.GetSection(serviceSettingsSectionName).Get<Alga.IdentityService.Infrastructure.ServiceSettings.Req>();
algaIdentityServiceSettingsReq?.IsDebug = builder.Environment.IsDevelopment();

if (algaIdentityServiceSettingsReq != null)
    if (algaIdentityServiceSettingsReq.IsDebug)
        algaIdentityServiceSettingsReq.BaseUrl = "https://localhost:7051";

// поменять baseUrl для тестирования

var algaIdentityServiceSettingsRes = new Alga.IdentityService.Infrastructure.ServiceSettings.Initializer(algaIdentityServiceSettingsReq);

builder.Services.AddSingleton<Alga.IdentityService.Infrastructure.ServiceSettings.IServiceSettings>(sp => new Alga.IdentityService.Infrastructure.ServiceSettings.Initializer(algaIdentityServiceSettingsReq));


// NATS
// ----------------------------

// builder.Services.AddSingleton<INatsConnection>(sp =>
// {
//     var opts = NatsOpts.Default with
//     {
//         Url = algaIdentityServiceSettingsRes.NatsUrl,
//         Name = serviceName,
//         AuthOpts = new NatsAuthOpts
//         {
//             Username = algaIdentityServiceSettingsRes.NatsUserName,
//             Password = algaIdentityServiceSettingsRes.NatsUserPassword
//         }
//     };

//     return new NatsConnection(opts);
// });


// Alga.sessions
// ----------------------------

// builder.Services.AddSingleton<Alga.sessions.IProvider>(sp =>
// {
//     var opts = new Alga.sessions.Operations.LibSettings.Req() { SecretKey = algaIdentityServiceSettingsRes.AlgaSessionsSecretKey, SessionRefreshIntervalInMin = algaIdentityServiceSettingsRes.AlgaSessionsSessionRefreshIntervalInMin };
//     return new Alga.sessions.Provider(opts);
// });


// Authentication
// --------------------------
// Google: https://console.cloud.google.com/apis/credentials

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.None;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    })
    .AddGoogle(options =>
    {
        options.ClientId = algaIdentityServiceSettingsRes.GoogleAuthenticationClientId;
        options.ClientSecret = algaIdentityServiceSettingsRes.GoogleAuthenticationClientSecret;
        options.SaveTokens = true;
    });

builder.Services.AddAuthorization();


// Hosted Services Registration
// ----------------------------
// Registers background hosted services that perform various runtime tasks,
// including caching, file handling, sitemap generation, and database synchronization.
// Some services are only registered in production mode (when !isDebug).

builder.Services.Scan(scan => scan
    .FromAssemblyOf<Alga.IdentityService.Infrastructure.Background.IBackgroundProcess>()
    .AddClasses(classes => classes.AssignableTo<Alga.IdentityService.Infrastructure.Background.IBackgroundProcess>())
    .As<Alga.IdentityService.Infrastructure.Background.IBackgroundProcess>()
    .WithSingletonLifetime()
);

builder.Services.AddHostedService<Alga.IdentityService.Infrastructure.Background.BackgroundHost>();


// Core Entities
// --------------------------------

builder.Services.Scan(scan => scan
    .FromAssemblyOf<Alga.IdentityService.Core.Entities.IE>()
    .AddClasses(classes => classes.AssignableTo<Alga.IdentityService.Core.Entities.IE>())
    .AsSelf()                  // 👈 теперь можно резолвить E напрямую
    .As<Alga.IdentityService.Core.Entities.IE>() // оставляем интерфейс на всякий случай
    .WithSingletonLifetime()
);

// Core Logic
// --------------------------------

builder.Services.Scan(scan => scan
    .FromAssemblyOf<Alga.IdentityService.Core.Logic.I>()
    .AddClasses(classes => classes.AssignableTo<Alga.IdentityService.Core.Logic.I>())
    .AsSelf()
    .As<Alga.IdentityService.Core.Logic.I>()
    .WithSingletonLifetime()
);


// Alga.wwwcore Root Initialization
// --------------------------------
// Registers the Alga.wwwcore.Root as a singleton service.
// Initializes it with configuration from "AlgaWwwcoreConfig", logger instance, and environment mode.
// Enables structured frontend generation and development-time support based on project config.

builder.Services.AddSingleton<Alga.wwwcore.Root>(sp =>
{
    var clientSettings = sp.GetRequiredService<IConfiguration>().GetSection("AlgaWwwcoreConfig").Get<Alga.wwwcore.ClientSettings>();

    if (clientSettings == null) throw new ArgumentException(nameof(clientSettings));

    return new(clientSettings, sp.GetRequiredService<IHostEnvironment>().IsDevelopment());
});

// Use-case service
// ----------------------------

builder.Services.Scan(scan => scan
    .FromAssemblyOf<Alga.IdentityService.Application.Ports.InAPI.I>() // или общий маркерный интерфейс I
    .AddClasses(classes => classes.InNamespaces("Alga.IdentityService.Application.UseCases")) // все UseCases
    .AsImplementedInterfaces() // IH → HService
    .WithScopedLifetime()
);

// ----------------------------

builder.Services.AddSingleton<Alga.IdentityService.Client.I>(sp => new Alga.IdentityService.Client.Builder(algaIdentityServiceSettingsRes.ProjectId, algaIdentityServiceSettingsRes.ProjectSecretKey, algaIdentityServiceSettingsRes.BaseUrl, algaIdentityServiceSettingsReq.IsDebug));

// ----------------------------

builder.Services.AddOutputCache();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedProto |
        ForwardedHeaders.XForwardedHost;

    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddEndpointsApiExplorer();

// ----------------------------

var app = builder.Build();

app.UseForwardedHeaders();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Alga.IdentityService has started");

Alga.IdentityService.Infrastructure.Context.Initialize(app.Services.GetRequiredService<Alga.IdentityService.Infrastructure.ServiceSettings.IServiceSettings>());



// Global Security Headers
// ------------------------
// Sets core HTTP security headers to improve privacy, prevent clickjacking, avoid MIME sniffing,
// and restrict access to sensitive browser features like camera, microphone, and opener context.

app.Use(async (context, next) =>
{
    var headers = context.Response.Headers;

    // Denies page embedding via <iframe>, <frame>, or <object> (clickjacking protection)
    headers["X-Frame-Options"] = "DENY";

    // Ensures no referrer information is sent with requests (maximizes privacy)
    headers["Referrer-Policy"] = "no-referrer";

    // Restricts use of sensitive browser APIs from this origin (hardens access to device features)
    headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";

    // Isolates the browsing context but still allows popups to retain opener (useful for SPA+MPA apps)
    headers["Cross-Origin-Opener-Policy"] = "same-origin-allow-popups";

    await next();
});

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseRequestTimeouts();
app.UseOutputCache();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapEndpoints();

app.Run();