using System.Diagnostics.CodeAnalysis;

namespace IS.ScaleModelsShop.API.Attributes;

/// <summary>
/// Workaround attribute, allowing to use OData parameters with Swagger.
/// </summary>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class SwaggerODataParameterSetupAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the name of the OData parameter.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the parameter is required.
    /// </summary>
    public bool IsRequired { get; set; } = false;

    /// <summary>
    /// Gets or sets the example value.
    /// </summary>
    public string Example { get; set; }
}