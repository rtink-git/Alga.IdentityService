namespace Alga.IdentityService.API.HTTP;

public class RefreshSession : Alga.IdentityService.Infrastructure.HTTP.Endpoint.IDefinition
{
    static string sessionHStr = "AlgaSession";
    public async ValueTask MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"/session/refresh", async (HttpContext context) =>
        {
            // try
            // {
            //     if (!context.Request.Headers.TryGetValue(sessionHStr, out var sessionHeader)) return Results.BadRequest();

            //     NatsMsg<string>? session = null;
            //     try { session = await bus.RequestAsync<string, string>(subject: $"{Constants.EnvironmentName}.auth_db_service.session.refresh.request", data: sessionHeader[0], replyOpts: new NatsSubOpts { Timeout = TimeSpan.FromSeconds(3) }); }
            //     catch (Exception ex) { return Results.BadRequest($"NATS connection error. Error message: {ex}"); }

            //     if (session == null || string.IsNullOrEmpty(session.Value.Data)) return Results.BadRequest("NATS = null");

            //     return session != null ? TypedResults.Text(session.Value.Data) : Results.Unauthorized();
            // }
            // catch
            // {
            //     return Results.StatusCode(StatusCodes.Status500InternalServerError);
            // }
        }); //.RequireCors(CorsPolicies.FohouseRuCorsPolicy).WithSummary("User Session Refresh").WithDescription("Refreshes a user session using the current session token and updates its expiration.");
    }
}