using System.Reflection;
using FluentValidation;
using IS.ScaleModelsShop.Application.Middleware;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace IS.ScaleModelsShop.Application.Extensions
{
    public static class ServiceCollectionDIExtension
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(typeof(ServiceCollectionDIExtension).Assembly);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ServiceCollectionDIExtension).Assembly));
            services.AddValidatorsFromAssembly(typeof(ServiceCollectionDIExtension).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>),
                typeof(ValidationForEmptyResponseBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}