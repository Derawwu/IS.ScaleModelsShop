using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace IS.ScaleModelsShop.API.Providers;

/// <inheritdoc cref="IJsonResponseProvider{TValue}" />
[ExcludeFromCodeCoverage]
public class JsonResponseProvider<TValue> : IJsonResponseProvider<TValue>
{
    /// <inheritdoc />
    public async Task WriteAsJsonAsync(
        HttpResponse response,
        TValue value,
        JsonSerializerOptions serializerOptions,
        string contentType)
    {
        await response.WriteAsJsonAsync(value, serializerOptions, contentType);
    }
}