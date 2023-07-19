namespace IS.ScaleModelsShop.API.AcceptanceTests.Communication
{
    /// <inheritdoc cref="IHttpClientService" />
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _client;

        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientService"/> class.
        /// </summary>
        /// <param name="client">Http client.</param>
        public HttpClientService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<HttpResponseMessage> SendRequestAsync(HttpMethod requestType, string url, StringContent? content = null, Dictionary<string, string>? headers = null)
        {
            using var request = new HttpRequestMessage(requestType, url);
            request.Content = content;

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            return await _client.SendAsync(request);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc cref="IDisposable" />
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                _client.Dispose();
            }

            this.disposed = true;
        }

    }
}