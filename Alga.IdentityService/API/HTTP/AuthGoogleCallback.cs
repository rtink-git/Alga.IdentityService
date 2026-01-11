using System;

namespace Alga.IdentityService.API.HTTP;

public class AuthGoogleCallback : Alga.IdentityService.Infrastructure.Endpoint.IDefinition
{
    static string sessionHStr = "AlgaSession";
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"/auth/google/callback", async (HttpContext context) =>
        {
            // try
            // {
            //     if (context == null) return Results.BadRequest("HttpContext is null.");

            //     var result = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //     if (result.Principal == null) return Results.Unauthorized();

            //     ClaimsIdentity? identity = result.Principal.Identities.FirstOrDefault();

            //     if (identity == null) return Results.BadRequest("Authentication failed, identity is null.");

            //     string? email = null;
            //     foreach (var i in identity.Claims)
            //         if (i.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress") { email = i.Value; break; }

            //     if (string.IsNullOrEmpty(email)) return Results.BadRequest("Email is missing from authentication data.");

            //     if (result.Properties == null || result.Properties.Items == null) return Results.BadRequest("Properties = null");

            //     var rsub = result.Properties.Items["RedirectUrl"];

            //     if (rsub == null) return Results.BadRequest("RedirectUrl = null");

            //     string baseUrlAuto = new Uri(rsub).GetLeftPart(UriPartial.Authority);

            //     var RedirectUrl = baseUrlAuto ?? "/";

            //     NatsMsg<string>? session = null;
            //     try { session = await bus.RequestAsync<string, string>(subject: $"{Constants.EnvironmentName}.auth_db_service.session.create.request", data: JsonSerializer.Serialize(new AuthRequest() { Identifier = email, RoleId = 1 }), replyOpts: new NatsSubOpts { Timeout = TimeSpan.FromSeconds(3) }); }
            //     catch (Exception ex) { return Results.BadRequest($"NATS connection error. Error message:"); }

            //     if (session == null || string.IsNullOrEmpty(session.Value.Data)) return Results.BadRequest("NATS = null");

            //     string sessionEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(session.Value.Data)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
            //     string rsubEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(rsub)).TrimEnd('=').Replace('+', '-').Replace('/', '_');

            //     RedirectUrl = $"{baseUrlAuto}/access?tkn={sessionEncoded}&redirect_url={rsubEncoded}";

            //     return Results.Redirect(RedirectUrl);
            // }
            // catch
            // {
            //     return Results.StatusCode(StatusCodes.Status500InternalServerError);
            // }
        }).WithSummary("Google OAuth authentication callback").WithDescription("Processes Google authentication response, creates user session and redirects"); // cheched: 20250525
    }

    public class AuthRequest
    {
        public required string Identifier { get; set; }
        public required long RoleId { get; set; }
    }
}
