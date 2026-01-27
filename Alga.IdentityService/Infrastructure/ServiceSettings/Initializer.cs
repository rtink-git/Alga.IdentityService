namespace Alga.IdentityService.Infrastructure.ServiceSettings;

class Initializer : IServiceSettings
{
    /// <summary>
    /// Base service domain url. Example: "https://example.com"
    /// </summary>
    public Guid ProjectId { get; init; }
    public string ProjectSecretKey { get; init; }
    public string BaseUrl { get; set; }
    public string GoogleAuthenticationClientId { get; init; }
    public string GoogleAuthenticationClientSecret { get; init; }
    public string PostgresConnectionString { get; init; }


    // public string NatsUrl { get; init; }
    // public string NatsUserName { get; init; }
    // public string NatsUserPassword { get; init; }

    // ----------

    public bool IsDebug { get; init; }

    // ----------

    public Initializer(Req? req)
    {
        if (req is null) throw new InvalidOperationException("Request configuration is missing. Builder.Do cannot proced with a null request.");
        if (string.IsNullOrEmpty(req.ProjectSecretKey)) throw new InvalidOperationException($"{nameof(req.GoogleAuthenticationClientId)} is missing or empty. Request configuration is invalid.");
        if (string.IsNullOrEmpty(req.BaseUrl)) throw new InvalidOperationException($"{nameof(req.BaseUrl)} is missing or empty. Request configuration is invalid.");
        if (string.IsNullOrEmpty(req.GoogleAuthenticationClientId)) throw new InvalidOperationException($"{nameof(req.GoogleAuthenticationClientId)} is missing or empty. Request configuration is invalid.");
        if (string.IsNullOrEmpty(req.GoogleAuthenticationClientSecret)) throw new InvalidOperationException($"{nameof(req.GoogleAuthenticationClientSecret)} is missing or empty. Request configuration is invalid.");
        if (string.IsNullOrEmpty(req.NatsUrl)) throw new InvalidOperationException($"{nameof(req.NatsUrl)} is missing or empty. Request configuration is invalid.");
        if (string.IsNullOrEmpty(req.NatsUserName)) throw new InvalidOperationException($"{nameof(req.NatsUserName)} is missing or empty. Request configuration is invalid.");
        if (string.IsNullOrEmpty(req.NatsUserPassword)) throw new InvalidOperationException($"{nameof(req.NatsUserPassword)} is missing or empty. Request configuration is invalid.");
        if (string.IsNullOrEmpty(req.PostgresConnectionString)) throw new InvalidOperationException($"{nameof(req.PostgresConnectionString)} is missing or empty. Request configuration is invalid.");

        ProjectId = req.ProjectId;
        ProjectSecretKey = req.ProjectSecretKey;
        BaseUrl = req.BaseUrl;
        GoogleAuthenticationClientId = req.GoogleAuthenticationClientId;
        GoogleAuthenticationClientSecret = req.GoogleAuthenticationClientSecret;
        PostgresConnectionString = req.PostgresConnectionString;
        // NatsUrl = req.NatsUrl;
        // NatsUserName = req.NatsUserName;
        // NatsUserPassword = req.NatsUserPassword;
        IsDebug = req.IsDebug;
    }
}

// public required Guid ProjectId { get; init; }
// public required string ProjectSecretKey { get; init; }