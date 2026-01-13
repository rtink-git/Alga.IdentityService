namespace Alga.IdentityService.Infrastructure;

internal static class Context
{
    public static Operations.InfrastructureContext.Res Res = default!;
    public static void Initialize(Operations.ServiceSettings.Res res) => Res = Operations.InfrastructureContext.H.Do(res);
}
