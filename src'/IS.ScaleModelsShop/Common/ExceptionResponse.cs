using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace IS.ScaleModelsShop.API.Common;

/// <summary>
///     Exception response model.
/// </summary>
[ExcludeFromCodeCoverage]
public class ExceptionResponse
{
    /// <summary>
    ///     Gets or sets a URI identifier that categorizes the error.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    ///     Gets or sets a brief, human-readable message about the error.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    ///     Gets or sets a human-readable explanation of the error.
    /// </summary>
    public string Detail { get; set; }

    /// <summary>
    ///     Gets or sets a status code.
    /// </summary>
    public int? Status { get; set; }

    /// <summary>
    ///     Gets or sets a URI that identifies the specific occurrence of the error.
    /// </summary>
    public string Instance { get; set; }

    /// <summary>
    ///     Gets or sets collection of <see cref="InvalidParameter" />.
    /// </summary>
    [JsonPropertyName("invalid-params")]
    [JsonProperty("invalid-params")]
    public IEnumerable<InvalidParameter> InvalidParameters { get; set; }

    /// <summary>
    ///     Gets or sets the correlation ID is meant to be used to help an Admin trace what was happening at
    ///     the time of an error.
    /// </summary>
    [JsonPropertyName("trace-id")]
    [JsonProperty("trace-id")]
    public string TraceId { get; set; }
}