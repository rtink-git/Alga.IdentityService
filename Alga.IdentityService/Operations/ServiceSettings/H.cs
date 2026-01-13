namespace Alga.IdentityService.Operations.ServiceSettings;

static class H
{
    public static Res Do(Req? req)
    {
        if (req is null) throw new InvalidOperationException("Request configuration is missing. Builder.Do cannot proceed with a null request.");
        if (string.IsNullOrEmpty(req.GoogleAuthenticationClientId)) throw new InvalidOperationException($"{nameof(req.GoogleAuthenticationClientId)} is missing or empty. Request configuration is invalid.");
        if (string.IsNullOrEmpty(req.GoogleAuthenticationClientSecret)) throw new InvalidOperationException($"{nameof(req.GoogleAuthenticationClientSecret)} is missing or empty. Request configuration is invalid.");
        if (string.IsNullOrEmpty(req.NatsUrl)) throw new InvalidOperationException($"{nameof(req.NatsUrl)} is missing or empty. Request configuration is invalid.");
        if (string.IsNullOrEmpty(req.NatsUserName)) throw new InvalidOperationException($"{nameof(req.NatsUserName)} is missing or empty. Request configuration is invalid.");
        if (string.IsNullOrEmpty(req.NatsUserPassword)) throw new InvalidOperationException($"{nameof(req.NatsUserPassword)} is missing or empty. Request configuration is invalid.");
        if (string.IsNullOrEmpty(req.PostgresConnectionString)) throw new InvalidOperationException($"{nameof(req.PostgresConnectionString)} is missing or empty. Request configuration is invalid.");
        if (string.IsNullOrEmpty(req.AlgaSessionsSecretKey)) throw new InvalidOperationException($"{nameof(req.AlgaSessionsSecretKey)} is missing or empty. Request configuration is invalid.");

        if (string.IsNullOrEmpty(req.BaseUrl)) throw new InvalidOperationException($"{nameof(req.BaseUrl)} is missing or empty. Request configuration is invalid.");

        return new Res
        {
            AlgaSessionsSecretKey = req.AlgaSessionsSecretKey,
            AlgaSessionsSessionRefreshIntervalInMin = req.AlgaSessionsSessionRefreshIntervalInMin,
            BaseUrl = req.BaseUrl,
            GoogleAuthenticationClientId = req.GoogleAuthenticationClientId,
            GoogleAuthenticationClientSecret = req.GoogleAuthenticationClientSecret,
            NatsUrl = req.NatsUrl,
            NatsUserName = req.NatsUserName,
            NatsUserPassword = req.NatsUserPassword,
            PostgresConnectionString = req.PostgresConnectionString
        };
    }
}
