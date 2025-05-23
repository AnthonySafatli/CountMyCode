using System.Net;
using System.Net.Sockets;

namespace CountMyCode.Utils;

public static class SocketUtils
{
    public static bool IsPortInUse(int port)
    {
        try
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();
            listener.Stop();
            return false; // Port is free
        }
        catch (SocketException)
        {
            return true; // Port is in use
        }
    }
}
