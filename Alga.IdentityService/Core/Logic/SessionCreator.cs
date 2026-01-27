namespace Alga.IdentityService.Core.Logic;

public class SessionCreator : I
{
    readonly Entities.Session.PreviousSessionId.E _sessionPreviousSessionIdE;
    readonly Entities.Session.Publish.E _sessionPublishE;
    readonly Entities.Session.RefererUrl.E _sessionRefererUrlE;
    readonly Entities.SessionAgentIndex.E _sessionAgentIndexE;
    public SessionCreator(
        Entities.Session.PreviousSessionId.E sessionPreviousSessionIdE,
        Entities.Session.Publish.E sessionPublishE,
        Entities.Session.RefererUrl.E sessionRefererUrlE,
        Entities.SessionAgentIndex.E sessionAgentIndexE)
    {
        _sessionPreviousSessionIdE = sessionPreviousSessionIdE;
        _sessionPublishE = sessionPublishE;
        _sessionRefererUrlE = sessionRefererUrlE;
        _sessionAgentIndexE = sessionAgentIndexE;
    }

    public async Task<(Guid sessionId, DateTime sessionPublish)?> Create(
        Guid AgentIdE,
        Guid? PreviosSessionId,
        string? RefererUrl)
    {
        if (await _sessionPublishE.AddAsync() is not { } s) return null;
        if (PreviosSessionId != null && !await _sessionPreviousSessionIdE.AddAsync(s.Id, PreviosSessionId)) return null;
        if (!await _sessionAgentIndexE.AddAsync(s.Id, AgentIdE)) return null;

        await _sessionRefererUrlE.AddAsync(s.Id, RefererUrl);

        return (s.Id, s.Value);
    }
}
