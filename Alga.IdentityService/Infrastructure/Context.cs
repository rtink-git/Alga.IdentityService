namespace Alga.IdentityService.Infrastructure;

internal static class Context
{
    internal const string UIPrefix = "/UISs/";
    internal static bool IsDebug = true;
    internal static string PostgresConnectionString = string.Empty;
    internal static string CacheControlValue = "";

    public static void Initialize(ServiceSettings.IServiceSettings res)
    {
        IsDebug = res.IsDebug;
        PostgresConnectionString = res.PostgresConnectionString;
        CacheControlValue = $"public, max-age={0}, stale-while-revalidate={0 * 24}, stale-if-error={0 * 24}";
    }
}
