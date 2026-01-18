namespace Alga.IdentityService.API.HTTP;

public class CheckSession : Alga.IdentityService.Infrastructure.HTTP.Endpoint.IDefinition
{
    static string sessionHStr = "AlgaSession";
    public async ValueTask MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"/session/check", async (HttpContext context) =>
        {
        }); //.RequireCors(CorsPolicies.FohouseRuCorsPolicy).WithSummary("User Session Refresh").WithDescription("Refreshes a user session using the current session token and updates its expiration.");
    }
}