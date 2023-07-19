using IS.ScaleModelsShop.API.Attributes;
using IS.ScaleModelsShop.API.Constants;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IS.ScaleModelsShop.API.Filters;

/// <summary>
/// This filter replaces ODataQueryOptions parameters with corresponding OData operator parameters for Swagger.
/// It is a temporary workaround solution.
/// </summary>
public class SwaggerODataParametersFilter : IOperationFilter
{
    private readonly List<string> _defaultOdataParameters = new() { SwaggerODataConstants.Filter, SwaggerODataConstants.Skip, SwaggerODataConstants.Top, SwaggerODataConstants.OrderBy};

    /// <inheritdoc/>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        ArgumentNullException.ThrowIfNull(operation, nameof(operation));
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        operation.Parameters ??= new List<OpenApiParameter>();

        var odataQueryOptionsParameter = context.ApiDescription.ParameterDescriptions
            .FirstOrDefault(parameter => parameter.Type.IsGenericType
                            && parameter.Type.GetGenericTypeDefinition() == typeof(ODataQueryOptions<>));

        if (odataQueryOptionsParameter != null)
        {
            var operationOdataParameter = operation.Parameters.FirstOrDefault(parameter => parameter.Name == odataQueryOptionsParameter.Name);

            if (operationOdataParameter != null)
            {
                operation.Parameters.Remove(operationOdataParameter);
            }

            AddDefaultODataParameters(operation, context);
        }
    }

    private void AddDefaultODataParameters(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiParameters = context.ApiDescription.CustomAttributes()
            .Where(x => x.GetType() == typeof(SwaggerODataParameterSetupAttribute))
            .Select(x => (SwaggerODataParameterSetupAttribute)x).ToList();

        foreach (var parameter in _defaultOdataParameters)
        {
            var apiParameter = apiParameters.FirstOrDefault(apiParameter => apiParameter.Name == parameter);

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = parameter,
                Required = apiParameter?.IsRequired ?? false,
                In = ParameterLocation.Query,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString(apiParameter?.Example ?? string.Empty)
                }
            });
        }
    }
}