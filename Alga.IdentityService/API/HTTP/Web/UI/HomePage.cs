using System;

namespace Alga.IdentityService.API.HTTP.Web.UI;

public class HomePage : Infrastructure.HTTP.Endpoint.IDefinition
{
    public async ValueTask MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (HttpContext context, Alga.wwwcore.Root www, ILogger<Program> logger) =>
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(404.ToString());
            // try
            // {
            // var request = new Application.UseCases.GetCategoryPage.Req(0, null);

            // var page = await Application.UseCases.GetCategoryPage.H.Do(request, bus);

            // var seo = new Alga.wwwcore.SeoPageOptions
            // {
            //     Title = page?.Seo.TitleHeadHtml ?? string.Empty,
            //     Description = page?.Seo.DescripionHeadHtml,
            //     Robot = "index, follow",
            //     UrlCanonical = "/",
            //     ImageUrl = page?.Seo.ImageUrl,
            //     ImageWidth = page?.Seo.ImageWidth,
            //     ImageHeight = page?.Seo.ImageHeight,
            //     ImageEncodingFormat = page?.Seo.ImageEncodingFormat,
            //     Path = "/",
            //     Lang = page?.Seo.Lang,
            //     TypeOg = "website",
            //     SchemaOrgsJson = page?.Seo.LDJson
            // };

            // Infrastructure.HTTP.PageWriter.Write("i", context, www, seo, page?.PageModelJson);
            // }
            // catch (HttpRequestException httpEx) { await Infrastructure.HTTP.ErrorResult.HandleNotFoundAsync(context, logger, 503, httpEx.Message); }
            // catch (Exception ex) { await Infrastructure.HTTP.ErrorResult.HandleNotFoundAsync(context, logger, 500, ex.Message); }
        }); //.CacheOutput(Infrastructure.HTTP.Endpoint.OutputCachePolicies.HOutputCachePolicy);
    }
}