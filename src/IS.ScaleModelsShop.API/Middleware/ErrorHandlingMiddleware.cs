﻿using IS.ScaleModelsShop.API.Common;
using IS.ScaleModelsShop.API.Resources;
using IS.ScaleModelsShop.API.Responses;

namespace IS.ScaleModelsShop.API.Middleware;

/// <summary>
/// Represents a middleware to handle application exceptions.
/// </summary>
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly IResponseHandler responseHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorHandlingMiddleware" /> class.
    /// </summary>
    /// <param name="next"><see cref="RequestDelegate" />.</param>
    /// <param name="responseHandler">Instance of the <see cref="ResponseHandler" />.</param>
    public ErrorHandlingMiddleware(RequestDelegate next, IResponseHandler responseHandler)
    {
        this.next = next ?? throw new ArgumentNullException(nameof(next));
        this.responseHandler = responseHandler ?? throw new ArgumentNullException(nameof(responseHandler));
    }

    /// <summary>
    /// The method to be invoked on each request pipeline step.
    /// </summary>
    /// <param name="context"><see cref="HttpContext" />.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task Invoke(HttpContext context)
    {
        _ = context ?? throw new ArgumentNullException(nameof(context));

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        ExceptionResponse responseObject = ex switch
        {
            FileNotFoundException exception => new ExceptionResponse
            {
                Title = ErrorTitles.NotFoundError,
                Detail = exception.Message,
                Instance = context.Request.Path.Value,
                Status = StatusCodes.Status404NotFound
            },
            KeyNotFoundException exception => new ExceptionResponse
            {
                Title = ErrorTitles.NotFoundError,
                Detail = exception.Message,
                Instance = context.Request.Path.Value,
                Status = StatusCodes.Status404NotFound
            },
            ArgumentException exception => new ExceptionResponse
            {
                Title = ErrorTitles.ArgumentError,
                Detail = exception.Message,
                Instance = context.Request.Path.Value,
                Status = StatusCodes.Status400BadRequest
            },
            _ => new ExceptionResponse
            {
                Title = ErrorTitles.InternalServerError,
                Detail = ex.Message,
                Instance = context.Request.Path.Value,
                Status = StatusCodes.Status500InternalServerError
            }
        };

        await responseHandler.HandleResponseAsync(context, ex, responseObject);
    }
}