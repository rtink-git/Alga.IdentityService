using System;

namespace Alga.IdentityService.Core.Logic;

public class SessionAgentChecker : I
{
    readonly Entities.Agent.IP.E _agentIPE;
    readonly Entities.Agent.Lang.E _agentLangE;
    readonly Entities.Agent.SecChUa.E _agentSecChUaE;
    readonly Entities.Agent.SecCHUAPlatform.E _agentSecCHUAPlatformE;
    readonly Entities.Agent.UserAgent.E _agentUserAgentE;

    public SessionAgentChecker(
        Entities.Agent.IP.E agentIPE,
        Entities.Agent.Lang.E agentLangE,
        Entities.Agent.SecChUa.E agentSecChUaE,
        Entities.Agent.SecCHUAPlatform.E agentSecCHUAPlatformE,
        Entities.Agent.UserAgent.E agentUserAgentE)
    {
        _agentIPE = agentIPE;
        _agentLangE = agentLangE;
        _agentSecChUaE = agentSecChUaE;
        _agentSecCHUAPlatformE = agentSecCHUAPlatformE;
        _agentUserAgentE = agentUserAgentE;
    }

    public async Task<bool> Do(
        Guid agentIdE,
        string agentIPNormalized,
        string? agentLangNormalized,
        string? agentSecChUaNormalized,
        string? agentSecCHUAPlatformNormalazed,
        string? agentUserAgentNormalazed)
    {
        if (await _agentIPE.GetValueAsync(agentIdE) is not { } ipVE) return false;
        if (!ipVE.Equals(agentIPNormalized)) return false;
        var langVE = await _agentLangE.GetValueAsync(agentIdE);
        if (langVE != null && !langVE.Equals(agentLangNormalized)) return false;
        var secChUaVE = await _agentSecChUaE.GetValueAsync(agentIdE);
        if (secChUaVE != null && !secChUaVE.Equals(agentSecChUaNormalized)) return false;
        var secCHUAPlatformVE = await _agentSecCHUAPlatformE.GetValueAsync(agentIdE);
        if (secCHUAPlatformVE != null && !secCHUAPlatformVE.Equals(agentSecCHUAPlatformNormalazed)) return false;
        var userAgentVE = await _agentUserAgentE.GetValueAsync(agentIdE);
        if (userAgentVE != null && !userAgentVE.Equals(agentUserAgentNormalazed)) return false;

        return true;
    }
}
