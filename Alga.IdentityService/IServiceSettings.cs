namespace Alga.IdentityService;

public interface IServiceSettings
{
    /// <summary>
    /// example: "nats://1.1.1.1:4222"
    /// </summary>
    string NatsUrl { get; }
    string NatsUserName { get; }
    string NatsUserPassword { get; }
    string PostgresConnectionString { get; }
    string GoogleAuthenticationClientId { get; }
    string GoogleAuthenticationClientSecret { get; }
}