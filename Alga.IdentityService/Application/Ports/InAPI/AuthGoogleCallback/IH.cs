namespace Alga.IdentityService.Application.Ports.InAPI.AuthGoogleCallback;

public interface IH : I
{
    Task<string> HAsync(Req req, CancellationToken ct = default);
}

