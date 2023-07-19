using System.Diagnostics.CodeAnalysis;

namespace IS.ScaleModelsShop.API.Constants;

/// <summary>
/// OData constants.
/// </summary>
[ExcludeFromCodeCoverage]
public static class SwaggerODataConstants
{
    /// <summary>
    /// Filter parameter name.
    /// </summary>
    public const string Filter = "$filter";

    /// <summary>
    /// Skip parameter name.
    /// </summary>
    public const string Skip = "$skip";

    /// <summary>
    /// Top parameter name.
    /// </summary>
    public const string Top = "$top";

    /// <summary>
    /// OrderBy parameter name.
    /// </summary>
    public const string OrderBy = "$orderby";
}