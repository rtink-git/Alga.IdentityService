using Alga.IdentityService.Infrastructure.HTTP.Endpoint;

namespace Alga.IdentityService.API.HTTP.Web.UI;

public class AuthorizePage : IDefinition
{
    public async ValueTask MapEndpoints(IEndpointRouteBuilder app) =>
        app.MapGet("/authorize", async (HttpContext context, Alga.wwwcore.Root www, ILogger<HomePage> logger) =>
        {
            try
            {
                var seo = new Alga.wwwcore.SeoPageOptions
                {
                    Title = $"{www.ClientSettings.Name} - Hub"
                };

                await Infrastructure.HTTP.PageWriter.WriteAsync("Home", context, www, seo, null);
            }
            catch (Exception ex) { await ErrorHandler.HandleExceptionAsync(context, logger, ex); }
        })
        .WithRequestTimeout(RequestTimeoutPolicies.S5TimeoutPolicy); //.CacheOutput(Infrastructure.HTTP.Endpoint.OutputCachePolicies.HOutputCachePolicy);
}