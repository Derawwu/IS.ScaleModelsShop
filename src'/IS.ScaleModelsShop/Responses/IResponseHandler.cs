using IS.ScaleModelsShop.API.Common;

namespace IS.ScaleModelsShop.API.Responses
{
    /// <summary>
    /// Provides functionality to handle response.
    /// </summary>
    public interface IResponseHandler
    {
        /// <summary>
        /// Handles response.
        /// </summary>
        /// <param name="context">Http context.</param>
        /// <param name="ex">Exception.</param>
        /// <param name="responseObject">Instance of the <see cref="ExceptionResponse"/>.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task HandleResponseAsync(HttpContext context, Exception ex, ExceptionResponse responseObject);
    }
}
