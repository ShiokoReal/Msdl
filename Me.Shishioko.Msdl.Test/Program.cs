using Me.Shishioko.Msdl.Data;
using Me.Shishioko.Msdl.Data.Chat;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Me.Shishioko.Msdl.Test
{
    internal static class Program
    {
        private static void Main()
        {
            using Socket listener = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, 4647));
            listener.Listen();
            while (true)
            {
                Socket client = listener.Accept();
                ThreadPool.QueueUserWorkItem(Serve, client, false);
            }
        }
        private static void Serve(Socket socket)
        {
            try
            {

                Connection connection = new(new NetworkStream(socket)); //TODO: make process sync without events?
                Console.WriteLine("connected.");
                string cachedAddress = string.Empty;
                ushort cachedPort = 0;
                connection.OnAddress += (string address, ushort port) =>
                {
                    cachedAddress = address;
                    cachedPort = port;
                    Console.WriteLine(address);
                    Console.WriteLine(port);
                };
                connection.OnStatus += () =>
                {
                    return new ServerStatus()
                    {
                        Version = new("version", 765),
                        Description = new ChatText($"balls\n{cachedAddress}:{cachedPort}")
                    };
                };
                connection.OnLoginBegin += (string eventName, Guid eventGuid) =>
                {
                    string name = eventName;
                    Guid guid = eventGuid;
                    IEnumerable<Property> properties = [];
                    connection.OnEncryption += async () =>
                    {
                        return true;
                    };
                    connection.OnEncryptionFailure += (JsonElement error) => 
                    {
                        Console.WriteLine(error.ToString());
                    };
                    connection.OnEncryptionSuccess += (string eventName, Guid eventGuid, IEnumerable<Property> eventProperties) =>
                    {
                        name = eventName;
                        guid = eventGuid;
                        properties = eventProperties;
                    };
                    connection.OnLoginCompression += () =>
                    {
                        return 256;
                    };
                    connection.OnLoginEnd += () =>
                    {
                        Console.WriteLine($"{name} {guid}");
                        return (name, guid, properties);
                    };
                    connection.OnPlay += (Client client) =>
                    {
                        string[] dimensionNames = ["namespace:dimensionname"];
                        Dimension[] dimensionTypes = [new("namespace:dimensiontype")];
                        Biome[] biomes = [new("namespace:biome")];
                        client.SendConfigurationRegistries(dimensionTypes, biomes);
                        //registry
                        //end
                        //login
                        client.OnPluginMessage += (string channel, byte[] data) =>
                        {
                            switch (channel)
                            {
                                case "minecraft:brand":
                                    {
                                        Console.WriteLine(Encoding.UTF8.GetString(data));
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine(channel);
                                        break;
                                    }
                            }
                        };
                        Console.WriteLine("blls.");
                    };
                };
                connection.OnDisconnect += (Exception? exception) =>
                {
                    if (exception is not null) Console.WriteLine(exception);
                    socket.Dispose();
                    Console.WriteLine("disconnected.");
                };
                Console.WriteLine("done.");
            }
            finally
            {
                socket.Dispose();
            }
        }
    }
}
