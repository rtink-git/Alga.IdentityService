namespace Alga.IdentityService.API.HTTP;

public class HomePage : Alga.IdentityService.Infrastructure.Endpoint.IDefinition
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (HttpContext context) =>
        {
        });
    }
}