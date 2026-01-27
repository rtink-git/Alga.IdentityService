namespace Alga.IdentityService.Application.Ports.InAPI.Project;

public interface IH : I
{
    Task<Res?> HAsync(string session, CancellationToken ct = default);
}