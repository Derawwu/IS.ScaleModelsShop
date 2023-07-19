namespace IS.ScaleModelsShop.API.AcceptanceTests.Communication;

/// <summary>
/// Allows communicating with backend services using HTTP.
/// </summary>
public interface IHttpClientService : IDisposable
{
    /// <summary>
    /// Sends the request by type.
    /// </summary>
    /// <param name="requestType">Request type.</param>
    /// <param name="url">Endpoint Url.</param>
    /// <param name="content">Content string.</param>
    /// <param name="headers">A list of headers as key value data.</param>
    /// <returns>Async HttpResponseMessage from the request.</returns>
    Task<HttpResponseMessage> SendRequestAsync(HttpMethod requestType, string url, StringContent? content = null, Dictionary<string, string>? headers = null);
}