namespace Alga.IdentityService.Operations.ServiceSettings;

public class Req : Alga.IdentityService.IServiceSettings
{
    public required string NatsUrl { get; init; }
    public required string NatsUserName { get; init; }
    public required string NatsUserPassword { get; init; }
    public required string PostgresConnectionString { get; init; }
    public required string GoogleAuthenticationClientId { get; init; }
    public required string GoogleAuthenticationClientSecret { get; init; }
}
