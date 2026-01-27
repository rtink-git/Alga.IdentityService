using System.Net;

namespace Alga.IdentityService.Application.UseCases.SessionRefresh;

public class HService : Ports.InAPI.SessionRefresh.IH
{
    readonly Core.Entities.AgentProjectMap.E _agentProjectMapE;
    readonly Core.Entities.AgentUserIndex.E _agentUserIndexE;
    readonly Core.Entities.Project.SecretKey.E _projectSecretKeyE;
    readonly Core.Entities.Session.PreviousSessionId.E _sessionPreviousSessionIdE;
    readonly Core.Entities.Session.Publish.E _sessionPublishE;
    readonly Core.Entities.SessionAgentIndex.E _sessionAgentIndexE;
    readonly Core.Logic.SessionAgentChecker _sessionAgentCheckerLogic;
    readonly Core.Logic.SessionCreator _sessionCreatorLogic;

    public HService(
        Core.Entities.AgentProjectMap.E agentProjectMapE,
        Core.Entities.AgentUserIndex.E agentUserIndexE,
        Core.Entities.Project.SecretKey.E projectSecretKeyE,
        Core.Entities.Session.PreviousSessionId.E sessionPreviousSessionIdE,
        Core.Entities.Session.Publish.E sessionPublishE,
        Core.Entities.SessionAgentIndex.E sessionAgentIndexE,
        Core.Logic.SessionAgentChecker sessionAgentCheckerLogic,
        Core.Logic.SessionCreator sessionCreatorLogic)
    {
        _agentProjectMapE = agentProjectMapE;
        _agentUserIndexE = agentUserIndexE;
        _projectSecretKeyE = projectSecretKeyE;
        _sessionPreviousSessionIdE = sessionPreviousSessionIdE;
        _sessionPublishE = sessionPublishE;
        _sessionAgentIndexE = sessionAgentIndexE;
        _sessionAgentCheckerLogic = sessionAgentCheckerLogic;
        _sessionCreatorLogic = sessionCreatorLogic;
    }

    public async Task<string?> HAsync(
        string session,
        string baseUrl,
        System.Net.IPAddress ip,
        string? lang,
        string? secChUa,
        string? secChUaPlatform,
        string? userAgent,
        string? refererUrl,
        CancellationToken ct = default)
    {
        var sessionSplit = session.Split(':');

        if (string.IsNullOrEmpty(session)) return null;
        if (string.IsNullOrEmpty(baseUrl)) return null;

        if (sessionSplit.Length != 2) return null;

        var sId = new Guid(sessionSplit[0]);

        if (ip == null) return null;
        if (Handlers.Simple.AgentNormalizer.Do(ip, lang, secChUa, secChUaPlatform, userAgent) is not { } agentNormalized) return null;
        if (await _sessionAgentIndexE.GetValueAsync(sId) is not { } agentId) return null;
        if (!await _sessionAgentCheckerLogic.Do(agentId, agentNormalized.IpNormalized, agentNormalized.LangNormalized, agentNormalized.SecChUaNormalized, agentNormalized.SecCHUAPlatformNormalized, agentNormalized.UserAgentNormalized)) return null;
        if (await _agentProjectMapE.GetValueAsync(agentId) is not { } projectIdE) return null;
        if (await _projectSecretKeyE.GetValueAsync(projectIdE) is not { } projectSecretKey) return null;
        var encriptedTokenX = WebUtility.UrlDecode(sessionSplit[1]);
        encriptedTokenX = encriptedTokenX.Replace(" ", "+");
        string? tokenDecrypted = null; try { tokenDecrypted = Client.Helpers.AesGcmCrypto.Decrypt(encriptedTokenX, projectSecretKey); } catch { tokenDecrypted = null; }
        if (tokenDecrypted == null) return null;
        if (await _agentUserIndexE.GetValueAsync(agentId) is not { } userId) return null;
        var tokenDecryptedSplit = tokenDecrypted.Split(':');
        var tokenDt = tokenDecrypted.Replace($"{tokenDecryptedSplit[0]}:", null).ToString();
        if (await _sessionPublishE.GetValueAsync(sId) is not { } sPublish) return null;
        if (sPublish.ToString() != tokenDt) return null;
        var dto = DateTimeOffset.Parse(tokenDt, null, System.Globalization.DateTimeStyles.AssumeUniversal).UtcDateTime;
        dto = await _sessionPreviousSessionIdE.GetValueAsync(sId) is null ? dto.AddMinutes(1) : dto.AddDays(7);
        if ((dto - DateTime.UtcNow).TotalSeconds < 0) return null;
        if (await _sessionCreatorLogic.Create(agentId, sId, refererUrl) is not { } newSession) return null;
        var encryptKey = Client.Helpers.AesGcmCrypto.Encrypt($"{userId}:{newSession.sessionPublish}", projectSecretKey);

        return $"{newSession.sessionId}:{encryptKey}";
    }
}