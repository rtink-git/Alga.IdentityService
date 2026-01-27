namespace Alga.IdentityService.Application.UseCases.ProjectSecretKeyNew;

public class HService : Ports.InAPI.ProjectSecretKeyNew.IH
{
    readonly Alga.IdentityService.Client.I _identityServiceClient;
    readonly Core.Entities.Project.SecretKey.E _projectSecretKeyE;
    readonly Core.Entities.ProjectUserMap.E _projectUserMapE;

    public HService(
        Alga.IdentityService.Client.I identityServiceClient,
        Core.Entities.Project.SecretKey.E projectSecretKeyE,
        Core.Entities.ProjectUserMap.E projectUserMapE)
    {
        _identityServiceClient = identityServiceClient;
        _projectSecretKeyE = projectSecretKeyE;
        _projectUserMapE = projectUserMapE;
    }

    public async Task<Ports.InAPI.ProjectSecretKeyNew.Res?> HAsync(string session, Guid projectId, CancellationToken ct = default)
    {
        if (_identityServiceClient.GetUserId(session) is not { } userIdE) return null;
        if (await _projectUserMapE.GetIdAsync(userIdE) is not { } projectIdE) return null;
        var newSK = _projectSecretKeyE.GetRandomString();
        if (!await _projectSecretKeyE.UpdateValueAsync(projectId, newSK)) return null;

        return new(newSK);
    }
}