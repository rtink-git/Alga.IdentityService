using Alga.IdentityService.Infrastructure.HTTP.Endpoint;

namespace Alga.IdentityService.API.HTTP.Web;

public class SessionRefresh : IDefinition
{
    public async ValueTask MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"/refresh", async (string session, HttpContext context, Application.Ports.InAPI.SessionRefresh.IH handler) =>
        {
            var baseUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}";
            var ip = context.Connection.RemoteIpAddress; if (ip == null) return null;
            var userAgent = context.Request.Headers.UserAgent.ToString();
            var lang = context.Request.Headers.AcceptLanguage.ToString();
            var secChUa = context.Request.Headers["Sec-CH-UA"].ToString();
            var secChUaPlatform = context.Request.Headers["Sec-CH-UA-Platform"].ToString();

            var refererUrl = context.Request.Headers["Referer"].ToString();

            var refreshedSession = await handler.HAsync(session, baseUrl, ip, lang, secChUa, secChUaPlatform, userAgent, refererUrl, context.RequestAborted);

            return Results.Json(refreshedSession);
        })
        .RequireCors(CorsPolicies.OpenForAllCorsPolicy)
        .WithRequestTimeout(RequestTimeoutPolicies.S1TimeoutPolicy)
        .WithSummary("Initiate Google OAuth2 authentication flow")
        .WithDescription("Redirects user to Google authentication page to start OAuth 2.0 process. After successful authentication, user is redirected to the specified callback URL");
        //.RequireCors(CorsPolicies.FohouseRuCorsPolicy) //.WithOpenApi(generatedOperation => BaseOpenApiOperation(generatedOperation, new List<OpenApiTag> { TagApiRtInk, TagRtInk }, "Initiates Google OAuth 2.0 sign-in and redirects the user to Google's login page.")); // cheched: 20250525
    }
}
