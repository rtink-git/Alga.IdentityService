namespace Alga.IdentityService.Operations.ServiceSettings;

public class Req
{
    public required string NatsUrl { get; init; }
    public required string NatsUserName { get; init; }
    public required string NatsUserPassword { get; init; }
    public required string PostgresConnectionString { get; init; }
    public required string GoogleAuthenticationClientId { get; init; }
    public required string GoogleAuthenticationClientSecret { get; init; }
    public required string AlgaSessionsSecretKey { get; init; }
    public required byte AlgaSessionsSessionRefreshIntervalInMin { get; init; }
}
