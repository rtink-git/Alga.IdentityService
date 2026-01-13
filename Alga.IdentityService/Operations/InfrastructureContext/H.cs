namespace Alga.IdentityService.Operations.InfrastructureContext;

static class H
{
    public static Res Do(Operations.ServiceSettings.Req req) => new Res(req.PostgresConnectionString);
}
