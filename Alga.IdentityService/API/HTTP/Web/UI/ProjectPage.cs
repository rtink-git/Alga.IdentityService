using Alga.IdentityService.Infrastructure.HTTP.Endpoint;

namespace Alga.IdentityService.API.HTTP.Web.UI;

public class ProjectPage : IDefinition
{
    public async ValueTask MapEndpoints(IEndpointRouteBuilder app) =>
        app.MapGet("/project", async (HttpContext context, Alga.wwwcore.Root www, ILogger<AccessPage> logger) =>
        {
            try
            {
                var seo = new Alga.wwwcore.SeoPageOptions
                {
                    Title = $"Provider for Your Project - {www.ClientSettings.Name}",
                    Description = $"Manage user authentication and sessions in your project",
                };

                await Infrastructure.HTTP.PageWriter.WriteAsync("Project", context, www, seo, null);
            }
            catch (Exception ex) { await ErrorHandler.HandleExceptionAsync(context, logger, ex); }

            return Results.Empty;
        })
        .WithRequestTimeout(RequestTimeoutPolicies.S1TimeoutPolicy); //.CacheOutput(Infrastructure.HTTP.Endpoint.OutputCachePolicies.HOutputCachePolicy);
}