using System.Text.RegularExpressions;

namespace IS.ScaleModelsShop.API.Helpers;

/// <summary>
///     Helper for building controllers names to match the rule book.
/// </summary>
public class ParameterTransformer : IOutboundParameterTransformer
{
    private const string Prefix = "api/";

    /// <summary>
    ///     Transforms controller name to match the rule book.
    /// </summary>
    /// <param name="value">Controller name.</param>
    /// <returns>Transformed controller name.</returns>
    public string? TransformOutbound(object? value)
    {
        return value == null ? null : $"{Prefix}{Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower()}";
    }
}