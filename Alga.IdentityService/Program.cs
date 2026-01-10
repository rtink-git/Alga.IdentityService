var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Alga.IdentityService
// ----------------------------

const string serviceSettingsSectionName = "AlgaIdentityServiceSettings";

var algaIdentityServiceSettingsReq = builder.Configuration.GetSection(serviceSettingsSectionName).Get<Alga.IdentityService.Operations.ServiceSettings.Req>();
var algaIdentityServiceSettingsRes = Alga.IdentityService.Operations.ServiceSettings.Builder.Do(algaIdentityServiceSettingsReq);

app.Run();
