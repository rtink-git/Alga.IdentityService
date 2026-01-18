namespace Alga.IdentityService.Application.Handlers.Simple.ServiceSettings;

public class Req
{
    /// <summary>
    /// Base service domain url. Example: "https://example.com"
    /// </summary>
    public required string BaseUrl { get; init; }
    public required string NatsUrl { get; init; }
    public required string NatsUserName { get; init; }
    public required string NatsUserPassword { get; init; }
    public required string PostgresConnectionString { get; init; }
    public required string GoogleAuthenticationClientId { get; init; }
    public required string GoogleAuthenticationClientSecret { get; init; }
    public required string AlgaSessionsSecretKey { get; init; }
    public required byte AlgaSessionsSessionRefreshIntervalInMin { get; init; }

    // ----------

    public bool IsDebug { get; set; } = true;
}
