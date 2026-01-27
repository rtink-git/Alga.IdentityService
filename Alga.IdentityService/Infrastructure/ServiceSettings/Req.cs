namespace Alga.IdentityService.Infrastructure.ServiceSettings;

public class Req
{
    public required bool IsDebug { get; set; } = true;
    public required string BaseUrl { get; set; }
    /// <summary>
    /// Base service domain url. Example: "https://example.com"
    /// </summary>
    public required Guid ProjectId { get; init; }
    public required string ProjectSecretKey { get; init; }
    public required string NatsUrl { get; init; }
    public required string NatsUserName { get; init; }
    public required string NatsUserPassword { get; init; }
    public required string PostgresConnectionString { get; init; }
    public required string GoogleAuthenticationClientId { get; init; }
    public required string GoogleAuthenticationClientSecret { get; init; }
}
