using MediatR;

namespace IS.ScaleModelsShop.Application.Models.Queries;

/// <summary>
/// Base query.
/// </summary>
/// <typeparam name="TResult">The type of the result.</typeparam>
public abstract class QueryBase<TResult> : IRequest<TResult>
    where TResult : class
{
}