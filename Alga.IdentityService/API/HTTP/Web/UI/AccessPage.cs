using Alga.IdentityService.Infrastructure.HTTP.Endpoint;

namespace Alga.IdentityService.API.HTTP.Web.UI;

public class AccessPage : IDefinition
{
    public async ValueTask MapEndpoints(IEndpointRouteBuilder app) =>
        app.MapGet("/access", async (string session, HttpContext context, Alga.wwwcore.Root www, Alga.IdentityService.Client.I identityServiceClient, ILogger<AccessPage> logger) =>
        {
            var access = identityServiceClient.GetUserId(session);

            if (access is null)
                return Results.Redirect(identityServiceClient.GetAccessErrorUrl()); // надо проверить как работает

            try
            {
                var seo = new Alga.wwwcore.SeoPageOptions
                {
                    Title = $"Welcome - {www.ClientSettings.Name}",
                    Description = $"Your session is being verified to ensure secure access to {www.ClientSettings.Name}",
                };

                await Infrastructure.HTTP.PageWriter.WriteAsync("Access", context, www, seo, null);
            }
            catch (Exception ex) { await ErrorHandler.HandleExceptionAsync(context, logger, ex); }

            return Results.Empty;
        })
        .WithRequestTimeout(RequestTimeoutPolicies.S1TimeoutPolicy); //.CacheOutput(Infrastructure.HTTP.Endpoint.OutputCachePolicies.HOutputCachePolicy);
}