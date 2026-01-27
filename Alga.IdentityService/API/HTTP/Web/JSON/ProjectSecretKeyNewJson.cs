using Alga.IdentityService.Infrastructure.HTTP.Endpoint;

namespace Alga.IdentityService.API.HTTP.Web.JSON;

public class ProjectSecretKeyNewJson : IDefinition
{
    public async ValueTask MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"/api/project/secret-key/new", async (Guid projectId, HttpContext context, Application.Ports.InAPI.ProjectSecretKeyNew.IH handler) =>
        {
            var ses = GetHeader("AlgaIdentityServiceSession", context);

            if (string.IsNullOrEmpty(ses)) return null;

            if (await handler.HAsync(ses, projectId) is { } r) return Results.Json(r);

            return Results.NotFound();
        })
        .RequireCors(CorsPolicies.OnlyCurrentCorsPolicy)
        .WithRequestTimeout(RequestTimeoutPolicies.S5TimeoutPolicy)
        .WithSummary("")
        .WithDescription("");
        //.RequireCors(CorsPolicies.FohouseRuCorsPolicy) //.WithOpenApi(generatedOperation => BaseOpenApiOperation(generatedOperation, new List<OpenApiTag> { TagApiRtInk, TagRtInk }, "Initiates Google OAuth 2.0 sign-in and redirects the user to Google's login page.")); // cheched: 20250525
    }

    string? GetHeader(string name, HttpContext context) { Microsoft.Extensions.Primitives.StringValues value = ""; context.Request.Headers.TryGetValue(name, out value); return value; }
}