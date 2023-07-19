using System.Text.Json;

namespace IS.ScaleModelsShop.API.Providers;

/// <summary>
/// Json response provider.
/// </summary>
/// <typeparam name="TValue">Type of the value to write as JSON.</typeparam>
public interface IJsonResponseProvider<in TValue>
{
    /// <summary>
    /// Writes the specified value as JSON to the response body.
    /// </summary>
    /// <param name="response">The response to write JSON to.</param>
    /// <param name="value">The value to write as JSON.</param>
    /// <param name="serializerOptions">The serializer options use when serializing the value.</param>
    /// <param name="contentType">The content-type to set on the response.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    Task WriteAsJsonAsync(
        HttpResponse response,
        TValue value,
        JsonSerializerOptions serializerOptions,
        string contentType);
}