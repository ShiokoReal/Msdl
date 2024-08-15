using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System;

namespace Net.Myzuc.PurpleStainedGlass.Extensions
{
    public sealed class ExtensionStandardListener : Extension
    {
        public ExtensionStandardListener()
        {
            _ = StartListeningAsync(new IPEndPoint(IPAddress.Any, 4647));
        }
        private async Task StartListeningAsync(EndPoint endpoint)
        {
            try
            {
                using Socket listener = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(endpoint);
                listener.Listen();
                Logs.Info($"Listening on {endpoint}.");
                while (true)
                {
                    Socket socket = await listener.AcceptAsync();
                    Server.RegisterConnection(new NetworkStream(socket, true));
                }
            }
            catch(Exception ex)
            {
                Logs.Warning(ex.ToString());
            }
        }
        ~ExtensionStandardListener()
        {

        }
    }
}
