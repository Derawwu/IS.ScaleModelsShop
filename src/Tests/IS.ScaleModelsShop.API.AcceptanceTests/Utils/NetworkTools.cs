using System.Net;
using System.Net.Sockets;

namespace IS.ScaleModelsShop.API.AcceptanceTests.Utils;

/// <summary>
/// Implement of the <see cref="INetworkTools" /> functionality.
/// </summary>
public class NetworkTools : INetworkTools
{
    /// <inheritdoc />
    public int GetFreeTcpPort()
    {
        TcpListener l = new TcpListener(IPAddress.Loopback, 0);
        l.Start();
        var port = ((IPEndPoint)l.LocalEndpoint).Port;
        l.Stop();
        return port;
    }
}