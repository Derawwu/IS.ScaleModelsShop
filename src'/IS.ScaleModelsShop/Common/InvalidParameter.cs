using System.Diagnostics.CodeAnalysis;

namespace IS.ScaleModelsShop.API.Common;

/// <summary>
///     Invalid parameter model.
/// </summary>
[ExcludeFromCodeCoverage]
public class InvalidParameter
{
    /// <summary>
    ///     Gets or sets invalid parameter name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets invalid parameter reason.
    /// </summary>
    public string Reason { get; set; }
}