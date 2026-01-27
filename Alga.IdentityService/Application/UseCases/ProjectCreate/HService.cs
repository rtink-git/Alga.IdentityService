namespace Alga.IdentityService.Application.UseCases.ProjectCreate;

public class HService : Ports.InAPI.ProjectCreate.IH
{
    readonly Alga.IdentityService.Client.I _identityServiceClient;
    readonly Core.Entities.Project.Publish.E _projectPublishE;
    readonly Core.Entities.Project.SecretKey.E _projectSecretKeyE;
    readonly Core.Entities.ProjectUserMap.E _projectUserMapE;

    public HService(
        Alga.IdentityService.Client.I identityServiceClient,
        Core.Entities.Project.Publish.E projectPublishE,
        Core.Entities.Project.SecretKey.E projectSecretKeyE,
        Core.Entities.ProjectUserMap.E projectUserMapE)
    {
        _identityServiceClient = identityServiceClient;
        _projectPublishE = projectPublishE;
        _projectSecretKeyE = projectSecretKeyE;
        _projectUserMapE = projectUserMapE;
    }

    public async Task<Ports.InAPI.ProjectCreate.Res?> HAsync(string session, CancellationToken ct = default)
    {
        if (_identityServiceClient.GetUserId(session) is not { } userIdE) return null;
        if (await _projectUserMapE.GetIdAsync(userIdE) is { } projectIdE) return new(projectIdE);
        if (await _projectSecretKeyE.AddAsync() is not { } projectSecretKey) return null;
        if (await _projectPublishE.AddAsync(projectSecretKey.Id) is not { } projectPublishE) return null;
        if (!await _projectUserMapE.AddAsync(projectSecretKey.Id, userIdE)) return null;

        return new(projectSecretKey.Id);
    }
}
