// using System;

// namespace Alga.IdentityService.API.HTTP;

// class AuthTelegram : Alga.IdentityService.Infrastructure.HTTP.Endpoint.IDefinition
// {
//     static string sessionHStr = "AlgaSession";
//     public async ValueTask MapEndpoints(IEndpointRouteBuilder app)
//     {
//         app.MapGet($"/auth/telegram", async (HttpContext context) =>
//         {
//         }); //.RequireCors(CorsPolicies.FohouseRuCorsPolicy).WithSummary("User Session Refresh").WithDescription("Refreshes a user session using the current session token and updates its expiration.");
//     }

//     //app.MapPost($"{UR_Auth}/Telegram", async (HttpContext context, RtInkApi.DBProviders.rtinkapidbMSSQL.Client dbProvider, Alga.sessions.Provider sessionProvider) => { return await new RtInkApi.MapLogics.Auth(context).Telegram(dbProvider, sessionProvider); }).RequireCors(ApiAndRtInkCorsPolicy).WithRequestTimeout(Ms500TimeoutPolicy).WithOpenApi(generatedOperation => BaseOpenApiOperation(generatedOperation, new List<OpenApiTag> { TagRtInk }, "Handles Telegram authentication or account linking for a signed-in user." )); // cheched: 20250525
// }
