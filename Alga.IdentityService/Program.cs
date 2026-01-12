using NATS.Client.Core;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);


// Logging
// ----------------------------

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


// Service Settings
// ----------------------------

const string serviceName = "AlgaIdentityService";
const string serviceSettingsSectionName = $"{serviceName}Settings";

var algaIdentityServiceSettingsReq = builder.Configuration.GetSection(serviceSettingsSectionName).Get<Alga.IdentityService.Operations.ServiceSettings.Req>();
var algaIdentityServiceSettingsRes = Alga.IdentityService.Operations.ServiceSettings.Builder.Do(algaIdentityServiceSettingsReq);


// NATS
// ----------------------------

builder.Services.AddSingleton<INatsConnection>(sp =>
{
    var opts = NatsOpts.Default with
    {
        Url = algaIdentityServiceSettingsRes.NatsUrl,
        Name = serviceName,
        AuthOpts = new NatsAuthOpts
        {
            Username = algaIdentityServiceSettingsRes.NatsUserName,
            Password = algaIdentityServiceSettingsRes.NatsUserPassword
        }
    };

    return new NatsConnection(opts);
});


// Alga.sessions
// ----------------------------

builder.Services.AddSingleton<Alga.sessions.IProvider>(sp =>
{
    var opts = new Alga.sessions.Operations.LibSettings.Req() { SecretKey = algaIdentityServiceSettingsRes.AlgaSessionsSecretKey, SessionRefreshIntervalInMin = algaIdentityServiceSettingsRes.AlgaSessionsSessionRefreshIntervalInMin };
    return new Alga.sessions.Provider(opts);
});


// Authentication
// --------------------------
// Google: https://console.cloud.google.com/apis/credentials

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
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

// ----------------------------


var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Alga.IdentityService has started");

app.UseAuthentication();
app.UseAuthorization();

app.Run();