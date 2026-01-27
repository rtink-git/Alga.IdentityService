using System;

namespace Alga.IdentityService.Application.Ports.InAPI.ProjectSecretKeyNew;

public interface IH : I
{
    Task<Res?> HAsync(string session, Guid projectId, CancellationToken ct = default);
}