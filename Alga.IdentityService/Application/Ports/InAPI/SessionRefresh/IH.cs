namespace Alga.IdentityService.Application.Ports.InAPI.SessionRefresh;

public interface IH : I
{
    Task<string?> HAsync(
        string session,
        string baseUrl,
        System.Net.IPAddress ip,
        string? lang,
        string? secChUa,
        string? secChUaPlatform,
        string? userAgent,
        string? refererUrl,
        CancellationToken ct = default);
}
