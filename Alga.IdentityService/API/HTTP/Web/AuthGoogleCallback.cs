using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Alga.IdentityService.Infrastructure.HTTP.Endpoint;

namespace Alga.IdentityService.API.HTTP.Web;

class AuthGoogleCallback : IDefinition
{
    public async ValueTask MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"/google/callback", async (
            HttpContext context,
            Application.Ports.InAPI.AuthGoogleCallback.IH handler) =>
        {
            try
            {
                if (context == null) return Results.BadRequest("HttpContext is null.");

                var result = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                if (result.Principal == null) return Results.Unauthorized();

                ClaimsIdentity? identity = result.Principal.Identities.FirstOrDefault();

                if (identity == null) return Results.BadRequest("Authentication failed, identity is null.");

                string? googleUserId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string? country = identity.FindFirst(ClaimTypes.Country)?.Value;
                string? googleName = identity.FindFirst(ClaimTypes.Name)?.Value;
                string? googleGivenName = identity.FindFirst(ClaimTypes.GivenName)?.Value;
                string? googleSurname = identity.FindFirst(ClaimTypes.Surname)?.Value;
                string? email = identity.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email)) return Results.BadRequest("Email is missing from authentication data.");

                var baseUrl = result?.Properties?.Items["base-url"];

                if (string.IsNullOrEmpty(baseUrl)) return Results.BadRequest("Base url should not be null or empty");

                var projectId = result?.Properties?.Items["project-id"];

                if (string.IsNullOrEmpty(projectId)) return Results.BadRequest("Projecct id should not be null or empty");

                // -----

                var ip = context.Connection.RemoteIpAddress;
                var userAgent = context.Request.Headers.UserAgent.ToString();
                var lang = context.Request.Headers.AcceptLanguage.ToString();
                var secChUa = context.Request.Headers["Sec-CH-UA"].ToString();
                var chPlatform = context.Request.Headers["Sec-CH-UA-Platform"].ToString();

                // -----

                var req = new Application.Ports.InAPI.AuthGoogleCallback.Req(
                    email: email,
                    BaseUrl: baseUrl,
                    projectId: new Guid(projectId),
                    ip: ip,
                    agentUserAgent: userAgent,
                    agentLang: lang,
                    agentSecChUa: secChUa,
                    agentSecCHUAPlatform: chPlatform,
                    googleUserId: googleUserId,
                    googleName: googleName,
                    googleGivenName: googleGivenName,
                    googleSurname: googleSurname
                );

                var finalRedirectUrl = await handler.HAsync(req, context.RequestAborted);

                return Results.Redirect(finalRedirectUrl);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        })
        .WithRequestTimeout(RequestTimeoutPolicies.S5TimeoutPolicy)
        .WithSummary("Google OAuth authentication callback")
        .WithDescription("Processes Google authentication response, creates user session and redirects");
    }
}

// cheched: 20260126 / 20250525