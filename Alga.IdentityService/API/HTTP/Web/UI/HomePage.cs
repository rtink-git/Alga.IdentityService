namespace Alga.IdentityService.API.HTTP.Web.UI;

public class HomePage : Infrastructure.HTTP.Endpoint.IDefinition
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
                    Robot = "index, follow",
                    UrlCanonical = "/",
                    Path = "/",
                    Lang = "en",
                    TypeOg = "website"
                };

                await Infrastructure.HTTP.PageWriter.WriteAsync("Home", context, www, seo, null);
            }
            catch (Exception ex) { await Infrastructure.HTTP.ErrorHandler.HandleExceptionAsync(context, logger, ex); }
        }); //.CacheOutput(Infrastructure.HTTP.Endpoint.OutputCachePolicies.HOutputCachePolicy);
}