using IS.ScaleModelsShop.API.Filters;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IS.ScaleModelsShop.API.SwaggerConfiguration;

/// <summary>
/// Swagger extension methods for <see cref="IServiceCollection" />.
/// </summary>
public static class SwaggerServiceCollectionExtension
{
    /// <summary>
    /// Adds Swagger configuration.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection" />.</param>
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "1.0",
                Title = "IS.ScaleModelsShop.API"
            });
            c.DescribeAllParametersInCamelCase();
            c.EnableAnnotations();
            c.OperationFilter<SwaggerODataParametersFilter>();

            AddSwaggerXmlComments(c);
        });


        return services;
    }

    private static void AddSwaggerXmlComments(SwaggerGenOptions options)
    {
        foreach (var xmlDocFile in Directory.EnumerateFiles(AppContext.BaseDirectory, "*.XML"))
            options.IncludeXmlComments(xmlDocFile);
    }
}