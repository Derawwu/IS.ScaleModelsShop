using FluentValidation;
using IS.ScaleModelsShop.Application.Middleware;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace IS.ScaleModelsShop.Application.Extensions;

/// <summary>
/// Extension methods for adding services to an <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionDIExtension
{
    /// <summary>
    /// Adds Application layer to application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ServiceCollectionDIExtension).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ServiceCollectionDIExtension).Assembly));
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionDIExtension).Assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>),
            typeof(ValidationForEmptyResponseBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}