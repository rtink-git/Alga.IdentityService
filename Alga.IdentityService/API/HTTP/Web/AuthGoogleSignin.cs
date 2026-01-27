using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Alga.IdentityService.Infrastructure.HTTP.Endpoint;

namespace Alga.IdentityService.API.HTTP.Web;

class AuthGoogleSignin : IDefinition
{
    public async ValueTask MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"/google/signin", async (HttpContext context) =>
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = "/google/callback"
            };
            props.Items["project-id"] = context.Request.Query["project-id"].ToString();
            props.Items["base-url"] = context.Request.Query["base-url"].ToString();

            await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, props);
        })
        .WithRequestTimeout(RequestTimeoutPolicies.S5TimeoutPolicy)
        .WithSummary("Initiate Google OAuth2 authentication flow")
        .WithDescription("Redirects user to Google authentication page to start OAuth 2.0 process. After successful authentication, user is redirected to the specified callback URL");
        //.RequireCors(CorsPolicies.FohouseRuCorsPolicy) //.WithOpenApi(generatedOperation => BaseOpenApiOperation(generatedOperation, new List<OpenApiTag> { TagApiRtInk, TagRtInk }, "Initiates Google OAuth 2.0 sign-in and redirects the user to Google's login page.")); // cheched: 20250525
    }
}