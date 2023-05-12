using FluentValidation;
using IS.ScaleModelsShop.API.Common;
using IS.ScaleModelsShop.API.Resources;
using IS.ScaleModelsShop.API.Responses;

namespace IS.ScaleModelsShop.API.Middleware
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IResponseHandler responseHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next"><see cref="RequestDelegate"/>.</param>
        /// <param name="responseHandler">Instance of the <see cref="ResponseHandler"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when one of the constructor parameters is omitted.</exception>
        public ValidationExceptionMiddleware(RequestDelegate next, IResponseHandler responseHandler)
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
                await this.next(context);
            }
            catch (ValidationException ex)
            {
                var responseObject = this.GetValidationResponse(ex);

                await this.responseHandler.HandleResponseAsync(context, ex, responseObject);
            }
        }

        private ExceptionResponse GetValidationResponse(ValidationException validationException)
        {
            var invalidParameters = validationException.Errors
                .Select(e =>
                    new InvalidParameter
                    {
                        Name = e.PropertyName,
                        Reason = e.ErrorMessage
                    });

            var responseObj = new ExceptionResponse
            {
                Title = ErrorTitles.ValidationError,
                InvalidParameters = invalidParameters,
                Status = this.GetStatusCodeFromValidationException(validationException)
            };

            return responseObj;
        }

        private int GetStatusCodeFromValidationException(ValidationException validationException)
        {
            return int.TryParse(validationException.Errors.FirstOrDefault()?.ErrorCode, out var statusCode)
                ? statusCode
                : StatusCodes.Status422UnprocessableEntity;
        }
    }
}
