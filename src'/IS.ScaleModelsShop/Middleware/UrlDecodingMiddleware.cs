using System.Net;

namespace IS.ScaleModelsShop.API.Middleware
{
    public class UrlDecodingMiddleware
    {
        private readonly RequestDelegate _next;

        public UrlDecodingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.HasValue)
            {
                var path = WebUtility.UrlDecode(context.Request.Path.Value);
                context.Request.Path = new PathString(path);
            }

            await _next(context);
        }
    }
}