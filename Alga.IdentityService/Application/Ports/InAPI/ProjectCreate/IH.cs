namespace Alga.IdentityService.Application.Ports.InAPI.ProjectCreate;

public interface IH : I
{
    Task<Res?> HAsync(string session, CancellationToken ct = default);
}
