namespace Alga.IdentityService.Application.Ports.InAPI.AuthGoogleCallback;

public record class Req(
    string email,
    string BaseUrl,
    Guid projectId,
    System.Net.IPAddress ip,
    string? agentUserAgent,
    string? agentLang,
    string? agentSecChUa,
    string? agentSecCHUAPlatform,
    string? googleUserId,
    string? googleName,
    string? googleGivenName,
    string? googleSurname
);