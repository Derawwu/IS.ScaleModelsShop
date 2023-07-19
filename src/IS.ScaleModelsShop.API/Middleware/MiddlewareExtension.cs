namespace IS.ScaleModelsShop.API.Middleware;

public static class MiddlewareExtension
{
    /// <summary>
    /// Adds the error handling middleware to the HTTP request pipeline.
    /// </summary>
    /// <param name="applicationBuilder"><see cref="IApplicationBuilder" />.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IApplicationBuilder UseErrorsHandlingMiddleware(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder
            .UseMiddleware<ErrorHandlingMiddleware>()
            .UseMiddleware<ValidationExceptionMiddleware>()
            .UseMiddleware<NotFoundMiddleware>();
    }
}