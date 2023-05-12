using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace IS.ScaleModelsShop.API.Providers
{
    /// <inheritdoc cref="IJsonResponseProvider{T}"/>
    [ExcludeFromCodeCoverage]
    public class JsonResponseProvider<T> : IJsonResponseProvider<T>
    {
        /// <inheritdoc />
        public async Task WriteAsJsonAsync(
            HttpResponse response,
            T value,
            JsonSerializerOptions serializerOptions,
            string contentType)
        {
            await response.WriteAsJsonAsync(value, serializerOptions, contentType);
        }
    }
}