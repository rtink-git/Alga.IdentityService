using System.Buffers;

namespace Alga.IdentityService.Infrastructure.HTTP;

internal static class PageWriter
{
    const string TextHtmlUtf8 = "text/html; charset=utf-8";
    const string TextPlainUtf8 = "text/plain; charset=utf-8";
    static ReadOnlySpan<byte> ErrorBytes => "Internal Server Error"u8;

    public static async ValueTask WriteAsync(
        string template,
        HttpContext context,
        Alga.wwwcore.Root www,
        Alga.wwwcore.SeoPageOptions seoModel,
        string? pageModelAsJSON = null)
    {
        var response = context.Response;

        try
        {
            response.Headers.ContentType = TextHtmlUtf8;
            response.Headers.CacheControl = Context.CacheControlValue;
            response.StatusCode = StatusCodes.Status200OK;

            var writer = response.BodyWriter;

            var fullPath = string.Create(
                Context.UIPrefix.Length + template.Length,
                (Context.UIPrefix, template),
                static (span, state) =>
                {
                    state.UIPrefix.AsSpan().CopyTo(span);
                    state.template.AsSpan().CopyTo(span[state.UIPrefix.Length..]);
                });

            www.WriteHtml(writer, fullPath, seoModel, pageModelAsJSON);
            await writer.FlushAsync(context.RequestAborted);
        }
        catch (OperationCanceledException)
        {
            // клиент закрыл соединение
        }
        catch (Exception)
        {
            if (!response.HasStarted)
            {
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.ContentType = TextPlainUtf8;

                var writer = response.BodyWriter;
                writer.Write(ErrorBytes);
                await writer.FlushAsync(context.RequestAborted);
            }
        }
    }
}
