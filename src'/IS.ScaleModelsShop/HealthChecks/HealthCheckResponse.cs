namespace IS.ScaleModelsShop.API.HealthChecks;

/// <summary>
///     Health Check response.
/// </summary>
public class HealthCheckResponse
{
    /// <summary>
    ///     Gets the status.
    /// </summary>
    /// <value>
    ///     The status.
    /// </value>
    public string Status { get; init; }

    /// <summary>
    ///     Gets the health checks.
    /// </summary>
    /// <value>
    ///     The health checks.
    /// </value>
    public IEnumerable<IndividualHealthCheckResponse> HealthChecks { get; init; }

    /// <summary>
    ///     Gets the duration of the health check.
    /// </summary>
    /// <value>
    ///     The duration of the health check.
    /// </value>
    public TimeSpan HealthCheckDuration { get; init; }
}