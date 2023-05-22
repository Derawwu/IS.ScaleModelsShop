using IS.ScaleModelsShop.API.Common;
using IS.ScaleModelsShop.API.Providers;
using IS.ScaleModelsShop.API.Resources;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace IS.ScaleModelsShop.API.Responses
{
    public class ResponseHandler : IResponseHandler
    {
        private const string ContentType = "application/problem+json";

        private readonly IJsonResponseProvider<ExceptionResponse> jsonResponseProvider;


        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseHandler"/> class.
        /// </summary>
        /// <param name="jsonResponseProvider">Instance of the <see cref="IJsonResponseProvider{ExceptionResponse}"/>.</param>
        public ResponseHandler(
            IJsonResponseProvider<ExceptionResponse> jsonResponseProvider)
        {
            this.jsonResponseProvider = jsonResponseProvider ?? throw new ArgumentNullException(nameof(jsonResponseProvider));
        }

        /// <inheritdoc />
        public async Task HandleResponseAsync(HttpContext context, Exception ex, ExceptionResponse responseObject)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));
            _ = responseObject ?? throw new ArgumentNullException(nameof(responseObject));

            var errorType = GetErrorType(responseObject.Status);

            responseObject.Type = errorType;

            context.Response.StatusCode = responseObject.Status.HasValue ? (int)responseObject.Status : default;

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };

            await jsonResponseProvider.WriteAsJsonAsync(context.Response, responseObject, serializerOptions, ContentType);
        }

        private string GetErrorType(int? httpStatusCode)
        {
            return httpStatusCode switch
            {
                StatusCodes.Status400BadRequest => ErrorTypes.BadRequest,
                StatusCodes.Status404NotFound => ErrorTypes.NotFound,
                StatusCodes.Status422UnprocessableEntity => ErrorTypes.UnprocessableEntity,
                _ => ErrorTypes.InternalServerError,
            };
        }
    }
}
