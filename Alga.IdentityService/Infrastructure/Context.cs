namespace Alga.IdentityService.Infrastructure;

internal static class Context
{
    internal static string PostgresConnectionString = string.Empty;
    public static void Initialize(Application.Handlers.Simple.ServiceSettings.Res res)
    {
        PostgresConnectionString = res.PostgresConnectionString;
    }
}
