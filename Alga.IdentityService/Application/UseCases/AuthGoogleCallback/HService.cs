namespace Alga.IdentityService.Application.UseCases.AuthGoogleCallback;

public class HService : Ports.InAPI.AuthGoogleCallback.IH
{
    readonly Core.Entities.ProjectUserMap.E _projectUserMap;
    readonly Core.Entities.AgentSessionFirstMap.E _agentSessionFirstMapE;
    readonly Core.Entities.Email.Value.E _emailValueE;
    readonly Core.Entities.EmailGoogleMap.E _emailGoogleMapE;
    readonly Core.Entities.Google.GivenName.E _googleGivenNameE;
    readonly Core.Entities.Google.Identifier.E _googleIdentifierE;
    readonly Core.Entities.Google.Name.E _googleNameE;
    readonly Core.Entities.Google.Surname.E _googleSurnameE;
    readonly Core.Entities.Project.SecretKey.E _projectSecretKeyE;
    readonly Core.Entities.User.Publish.E _userPublishE;
    readonly Core.Entities.UserEmailUMap.E _userEmailUMapE;
    readonly Core.Logic.AgentCreator _agentCreatorLogic;
    readonly Core.Logic.SessionCreator _sessionCreatorLogic;


    public HService(
        Core.Entities.ProjectUserMap.E projectUserMap,
        Core.Entities.AgentSessionFirstMap.E agentSessionFirstMapE,
        Core.Entities.Email.Value.E emailValueE,
        Core.Entities.EmailGoogleMap.E emailGoogleMapE,
        Core.Entities.Google.GivenName.E googleGivenNameE,
        Core.Entities.Google.Identifier.E googleIdentifierE,
        Core.Entities.Google.Name.E googleNameE,
        Core.Entities.Google.Surname.E googleSurnameE,
        Core.Entities.Project.SecretKey.E projectSecretKeyE,
        Core.Entities.User.Publish.E userPublishE,
        Core.Entities.UserEmailUMap.E userEmailUMapE,
        Core.Logic.AgentCreator agentCreatorLogic,
        Core.Logic.SessionCreator sessionCreatorLogic)
    {
        _projectUserMap = projectUserMap;
        _agentSessionFirstMapE = agentSessionFirstMapE;
        _emailValueE = emailValueE;
        _emailGoogleMapE = emailGoogleMapE;
        _googleGivenNameE = googleGivenNameE;
        _googleIdentifierE = googleIdentifierE;
        _googleNameE = googleNameE;
        _googleSurnameE = googleSurnameE;
        _projectSecretKeyE = projectSecretKeyE;
        _userPublishE = userPublishE;
        _userEmailUMapE = userEmailUMapE;
        _agentCreatorLogic = agentCreatorLogic;
        _sessionCreatorLogic = sessionCreatorLogic;
    }

    public async Task<string> HAsync(
        Ports.InAPI.AuthGoogleCallback.Req req,
        CancellationToken ct = default)
    {
        var redirectUrl = "https://auth.rt.ink";

        if (await _emailValueE.GetOrAddIdAsync(req.email) is not { } emaiId) return redirectUrl;
        var userId = await _userEmailUMapE.GetIdAsync(emaiId);

        if (userId is null)
        {
            userId = await _userPublishE.AddAsync();
            if (!await _userEmailUMapE.AddAsync(userId, emaiId)) userId = null;
        }

        if (userId is null) return redirectUrl;
        if (await _projectSecretKeyE.GetValueAsync(req.projectId) is not { } projectSecretKey) return redirectUrl;

        if (req.ip == null) return redirectUrl;
        if (Handlers.Simple.AgentNormalizer.Do(req.ip, req.agentLang, req.agentSecChUa, req.agentSecCHUAPlatform, req.agentUserAgent) is not { } agentNormalized) return redirectUrl;
        if (await _agentCreatorLogic.Create(req.projectId, userId.Value, emaiId, agentNormalized.IpNormalized, agentNormalized.LangNormalized, agentNormalized.SecChUaNormalized, agentNormalized.SecCHUAPlatformNormalized, agentNormalized.UserAgentNormalized) is not { } agentIdE) return redirectUrl;

        if (!string.IsNullOrEmpty(req.googleUserId))
        {
            var googleIdE = await _emailGoogleMapE.GetValueAsync(emaiId);
            if (googleIdE == null)
            {
                var userIdE = await _googleIdentifierE.GetOrAddIdAsync(req.googleUserId);

                if (userIdE != null)
                {
                    var emailGoogleMapEF = await _emailGoogleMapE.AddAsync(emaiId, userIdE);
                }
            }
            else
            {
                await _googleNameE.AddAsync(googleIdE, req.googleName);
                await _googleGivenNameE.AddAsync(googleIdE, req.googleGivenName);
                await _googleSurnameE.AddAsync(googleIdE, req.googleSurname);
            }
        }

        if (await _sessionCreatorLogic.Create(agentIdE, null, null) is not { } sessionNew) return redirectUrl;
        if (!await _agentSessionFirstMapE.AddAsync(agentIdE, sessionNew.sessionId)) return redirectUrl;

        // -----

        var userIdWithSessionDate = $"{userId}:{sessionNew.sessionPublish}";

        var encryptKey = Client.Helpers.AesGcmCrypto.Encrypt(userIdWithSessionDate, projectSecretKey);

        // -----

        var sten = Uri.EscapeDataString($"{sessionNew.sessionId}:{encryptKey}");

        redirectUrl = $"{req.BaseUrl}/access?session={sten}";

        return redirectUrl;
    }
}