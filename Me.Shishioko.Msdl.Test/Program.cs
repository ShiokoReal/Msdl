using Me.Shishioko.Msdl.Data;
using Me.Shishioko.Msdl.Data.Chat;
using Net.Myzuc.ShioLib;
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
                Connection connection = new(new NetworkStream(socket));
                (ProtocolState state, string address, ushort port) = await connection.ReceiveHandshakeAsync();
                if (state == ProtocolState.Status)
                {
                    await connection.ExchangeStatusStatusAsync(new ServerStatus()
                    {
                        Version = new("version", 765),
                        Description = new ChatText($"balls\n{address}:{port}")
                    });
                    await connection.ExchangeStatusPingAsync();
                }
                else if (state == ProtocolState.Login)
                {
                    (string name, Guid guid) = await connection.ReceiveLoginStartAsync();
                    if (Game.Players.Count > 0) guid = Guid.NewGuid();
                    Property[]? properties = Game.Players.Count <= 0 ? await connection.ExchangeLoginEncryptionAsync(guid, name, "test", true) : [];
                    if (properties == null) return;
                    await connection.SendLoginCompressionAsync(256, CompressionLevel.Optimal);
                    await connection.ExchangeLoginEndAsync(guid, name, properties);
                    Player player = new(guid, connection, name, properties, address, port);
                    await Task.WhenAny([
                        player.PulseAsync(),
                        Game.ServeAsync(player)
                    ]);
                }
                else throw new NotSupportedException();
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
    }
}
