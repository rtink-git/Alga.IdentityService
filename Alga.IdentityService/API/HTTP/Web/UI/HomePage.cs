using Alga.IdentityService.Infrastructure.HTTP.Endpoint;

namespace Alga.IdentityService.API.HTTP.Web.UI;

class HomePage : IDefinition
{
    public async ValueTask MapEndpoints(IEndpointRouteBuilder app) =>
        app.MapGet("/", async (HttpContext context, Alga.wwwcore.Root www, ILogger<HomePage> logger) =>
        {
            try
            {
                var seo = new Alga.wwwcore.SeoPageOptions
                {
                    Title = $"{www.ClientSettings.Name} - Central Hub for Your Projects",
                    Description = www.ClientSettings.Description,
                    Robot = "index, follow"
                };

                await Infrastructure.HTTP.PageWriter.WriteAsync("Home", context, www, seo, null);
            }
            catch (Exception ex) { await ErrorHandler.HandleExceptionAsync(context, logger, ex); }
        })
        .WithRequestTimeout(RequestTimeoutPolicies.S1TimeoutPolicy); //.CacheOutput(Infrastructure.HTTP.Endpoint.OutputCachePolicies.HOutputCachePolicy);
}