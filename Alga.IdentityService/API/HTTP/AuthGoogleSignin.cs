using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;

namespace Alga.IdentityService.API.HTTP;

public class AuthGoogleSignin : Alga.IdentityService.Infrastructure.HTTP.Endpoint.IDefinition
{
    public async ValueTask MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"/auth/google/signin", (HttpContext context) =>
        {
            var redirectUri = context.Request.Query["redirect-uri"].ToString();

            context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties() { RedirectUri = "/auth/google/callback", Items = { { "RedirectUrl", redirectUri } } });
        })
        .WithDescription("Redirects user to Google authentication page to start OAuth 2.0 process. After successful authentication, user is redirected to the specified callback URL"); //.RequireCors(CorsPolicies.FohouseRuCorsPolicy).WithSummary("Initiate Google OAuth2 authentication flow")//.WithOpenApi(generatedOperation => BaseOpenApiOperation(generatedOperation, new List<OpenApiTag> { TagApiRtInk, TagRtInk }, "Initiates Google OAuth 2.0 sign-in and redirects the user to Google's login page.")); // cheched: 20250525
    }
}
