namespace Alga.IdentityService.Infrastructure.ServiceSettings;

public interface IServiceSettings
{
    bool IsDebug { get; init; }
    string BaseUrl { get; set; }
    Guid ProjectId { get; init; }
    string ProjectSecretKey { get; init; }
    string PostgresConnectionString { get; init; }
    string GoogleAuthenticationClientId { get; init; }
    string GoogleAuthenticationClientSecret { get; init; }
}
