using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace IS.ScaleModelsShop.API.UnitTests.TestEntities
{
    public static class RequestFactory
    {
        public static HttpRequest Create(string method, string uri)
        {
            HttpContext context = new DefaultHttpContext();
            var request = context.Request;

            IServiceCollection services = new ServiceCollection();
            context.RequestServices = services.BuildServiceProvider();

            request.Method = method;
            var requestUri = new Uri(uri);
            request.Scheme = requestUri.Scheme;
            request.Host = requestUri.IsDefaultPort ? new HostString(requestUri.Host) : new HostString(requestUri.Host, requestUri.Port);
            request.QueryString = new QueryString(requestUri.Query);
            request.Path = new PathString(requestUri.AbsolutePath);

            return request;
        }
    }
}