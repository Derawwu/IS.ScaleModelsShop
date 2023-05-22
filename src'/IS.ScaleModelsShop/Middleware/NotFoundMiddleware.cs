using IS.ScaleModelsShop.API.Common;
using IS.ScaleModelsShop.API.Resources;
using IS.ScaleModelsShop.API.Responses;
using IS.ScaleModelsShop.Application.Exceptions;

namespace IS.ScaleModelsShop.API.Middleware
{
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IResponseHandler responseHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next"><see cref="RequestDelegate"/>.</param>
        /// <param name="responseHandler">Instance of the <see cref="ResponseHandler"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when one of the constructor parameters is omitted.</exception>
        public NotFoundMiddleware(RequestDelegate next, IResponseHandler responseHandler)
        {
            this.responseHandler = responseHandler ?? throw new ArgumentNullException(nameof(responseHandler));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <summary>
        /// The method to be invoked on each request pipeline step.
        /// </summary>
        /// <param name="context"><see cref="HttpContext" />.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Invoke(HttpContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));

            try
            {
                await next(context);
            }
            catch (NotFoundException ex)
            {
                var responseObject = new ExceptionResponse
                {
                    Title = ErrorTitles.NotFoundError,
                    Detail = ex.Message,
                    Instance = context.Request.Path.Value,
                    Status = StatusCodes.Status404NotFound
                };

                await responseHandler.HandleResponseAsync(context, ex, responseObject);
            }
        }
    }
}