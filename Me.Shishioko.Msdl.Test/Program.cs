using Me.Shishioko.Msdl.Clients;
using Me.Shishioko.Msdl.Data;
using Me.Shishioko.Msdl.Data.Chat;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;

namespace Me.Shishioko.Msdl.Test
{
    internal static class Program
    {
        private static readonly Game Game = new();
        private static async Task Main()
        {
            await Game.LoadAsync();
            using Socket listener = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, 4647));
            listener.Listen();
            while (true)
            {
                Socket client = await listener.AcceptAsync();
                _ = ServeAsync(client);
            }
        }
        private static async Task ServeAsync(Socket socket)
        {
            try
            {
                int version = 0;
                string host = string.Empty;
                ushort port = 0;
                ClientHandshake handshake = new(new NetworkStream(socket))
                {
                    ReceiveHandshake = async (int pkVersion, string pkHost, ushort pkPort) =>
                    {
                        version = pkVersion;
                        host = pkHost;
                        port = pkPort;
                    },
                    SwitchStatus = async (ClientStatus status) =>
                    {
                        status.ReceiveStatusRequest = () => status.SendStatusResponseAsync(new ServerStatus()
                        {
                            Version = new("version", 765),
                            Description = new ChatText($"balls\n{host}:{port}")
                        });
                        status.ReceivePingRequest = (long sequence) => status.SendPingResponseAsync(sequence);
                        await status.StartReceivingAsync();
                    },
                    SwitchLogin = async (ClientLogin login) =>
                    {
                        string name = string.Empty;
                        Guid guid = Guid.Empty;
                        Property[] properties = [];
                        login.ReceiveStart = async (string pkName, Guid pkGuid) =>
                        {
                            name = pkName;
                            guid = pkGuid;
                            await login.SendEncryptionRequestAsync("test", true);
                        };
                        login.ReceiveAuthentication += async (string pkName, Guid pkGuid, Property[] pkProperties) =>
                        {
                            name = pkName;
                            guid = pkGuid;
                            properties = pkProperties;
                            await login.SendCompressionAsync(256, CompressionLevel.Optimal);
                            await login.SendEndAsync(guid, name, properties);
                        };
                        login.SwitchConfiguration += async (ClientConfiguration client) =>
                        {
                            await ServeConfigurationAsync(guid, name, properties, client);
                        };
                        await login.StartReceivingAsync();
                    }
                };
                await handshake.StartReceivingAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                socket.Dispose();
            }
        }
        private static async Task ServeConfigurationAsync(Guid guid, string name, Property[] properties, ClientConfiguration client, World world)
        {
            client.
            //add to and set up world
            //wait 


            /*Player player = new(guid, connection, name, properties, address, port);
            await Task.WhenAny([
                player.PulseAsync(),
                                Game.ServeAsync(player)
            ]);*/
        }
    }
}
