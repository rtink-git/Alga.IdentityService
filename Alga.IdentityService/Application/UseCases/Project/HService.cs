
namespace Alga.IdentityService.Application.UseCases.Project;

public class HService : Ports.InAPI.Project.IH
{
    readonly Alga.IdentityService.Client.I _identityServiceClient;
    readonly Core.Entities.AgentUserIndex.E _agentUserIndexE;
    readonly Core.Entities.Project.SecretKey.E _projectSecretKeyE;
    readonly Core.Entities.ProjectUserMap.E _projectUserMapE;
    readonly Core.Entities.SessionAgentIndex.E _sessionAgentIndex;

    public HService(
        Alga.IdentityService.Client.I identityServiceClient,
        Core.Entities.AgentUserIndex.E agentUserIndexE,
        Core.Entities.Project.SecretKey.E projectSecretKeyE,
        Core.Entities.ProjectUserMap.E projectUserMapE,
        Core.Entities.SessionAgentIndex.E sessionAgentIndex)
    {
        _identityServiceClient = identityServiceClient;
        _agentUserIndexE = agentUserIndexE;
        _projectSecretKeyE = projectSecretKeyE;
        _projectUserMapE = projectUserMapE;
        _sessionAgentIndex = sessionAgentIndex;
    }
    public async Task<Ports.InAPI.Project.Res?> HAsync(string session, CancellationToken ct = default)
    {
        if (_identityServiceClient.GetUserId(session) is not { } userIdE) return null;
        var sessionSplit = session.Split(':');
        var sessionIdE = new Guid(sessionSplit[0]);
        if (await _sessionAgentIndex.GetValueAsync(sessionIdE) is not { } agentId) return null;
        if (await _agentUserIndexE.GetValueAsync(agentId) is not { } agentUserId) return null;
        if (userIdE != agentUserId) return null;

        // получаем проекты пользователя

        var projectIds = await _projectUserMapE.GetIdsAsync(userIdE);

        return new(userIdE, projectIds);
    }
}
