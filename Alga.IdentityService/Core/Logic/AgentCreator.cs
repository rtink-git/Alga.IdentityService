namespace Alga.IdentityService.Core.Logic;

public class AgentCreator : I
{
    readonly Entities.Agent.IP.E _agentIPE;
    readonly Entities.Agent.Lang.E _agentLangE;
    readonly Entities.Agent.SecChUa.E _agentSecChUaE;
    readonly Entities.Agent.SecCHUAPlatform.E _agentSecCHUAPlatformE;
    readonly Entities.Agent.UserAgent.E _agentUserAgentE;
    readonly Entities.AgentEmailMap.E _agentEmailMap;
    readonly Entities.AgentProjectMap.E _agentProjectMapE;
    readonly Entities.AgentUserIndex.E _agentUserIndexE;

    public AgentCreator(
        Entities.Agent.IP.E agentIPE,
        Entities.Agent.Lang.E agentLangE,
        Entities.Agent.SecChUa.E agentSecChUaE,
        Entities.Agent.SecCHUAPlatform.E agentSecCHUAPlatformE,
        Entities.Agent.UserAgent.E agentUserAgentE,
        Entities.AgentEmailMap.E agentEmailMap,
        Entities.AgentProjectMap.E agentProjectMapE,
        Entities.AgentUserIndex.E agentUserIndexE)
    {
        _agentIPE = agentIPE;
        _agentLangE = agentLangE;
        _agentSecChUaE = agentSecChUaE;
        _agentSecCHUAPlatformE = agentSecCHUAPlatformE;
        _agentUserAgentE = agentUserAgentE;
        _agentEmailMap = agentEmailMap;
        _agentProjectMapE = agentProjectMapE;
        _agentUserIndexE = agentUserIndexE;
    }

    public async Task<Guid?> Create(
        Guid projectId,
        Guid userIdE,
        Guid emailIdE,
        string agentIPNormalized,
        string? agentLangNormalized,
        string? agentSecChUaNormalized,
        string? agentSecCHUAPlatformNormalazed,
        string? agentUserAgentNormalazed)
    {
        if (await _agentIPE.AddAsync(agentIPNormalized) is not { } agentId) return null;
        await _agentLangE.AddAsync(agentId, agentLangNormalized);
        await _agentSecChUaE.AddAsync(agentId, agentSecChUaNormalized);
        await _agentSecCHUAPlatformE.AddAsync(agentId, agentSecCHUAPlatformNormalazed);
        await _agentUserAgentE.AddAsync(agentId, agentUserAgentNormalazed);
        if (!await _agentEmailMap.AddAsync(agentId, emailIdE)) return null;
        if (!await _agentUserIndexE.AddAsync(agentId, userIdE)) return null;
        if (!await _agentProjectMapE.AddAsync(agentId, projectId)) return null;

        return agentId;
    }
}
