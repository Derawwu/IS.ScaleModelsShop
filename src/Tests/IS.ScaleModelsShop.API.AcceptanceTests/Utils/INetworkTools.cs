namespace IS.ScaleModelsShop.API.AcceptanceTests.Utils;

/// <summary>
/// Provides service for getting free TCP port for acceptance tests.
/// </summary>
public interface INetworkTools
{
    /// <summary>
    /// Get the free TCP port.
    /// </summary>
    /// <returns>The free TCP port.</returns>
    int GetFreeTcpPort();
}