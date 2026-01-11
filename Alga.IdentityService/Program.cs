using NATS.Client.Core;

var builder = WebApplication.CreateBuilder(args);

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

// ----------------------------

var app = builder.Build();

app.Run();
