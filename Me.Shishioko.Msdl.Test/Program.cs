using Me.Shishioko.Msdl.Data;
using Me.Shishioko.Msdl.Data.Chat;
using Net.Myzuc.ShioLib;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Me.Shishioko.Msdl.Test
{
    internal static class Program
    {
        private static async Task Main()
        {
            using Socket listener = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, 4647));
            listener.Listen();
            while (true)
            {
                Socket client = await listener.AcceptAsync();
                _ = Serve(client);
            }
        }
        private static async Task Serve(Socket socket)
        {
            try
            {
                Connection connection = new(new NetworkStream(socket)); //TODO: make process sync without events?
                Console.WriteLine("connected.");
                string cachedAddress = string.Empty;
                ushort cachedPort = 0;
                (ProtocolState state, string address, ushort port) = await connection.ReceiveHandshakeAsync();
                Console.WriteLine(address);
                Console.WriteLine(port);
                if (state == ProtocolState.Status)
                {
                    await connection.ExchangeStatusStatusAsync(new ServerStatus()
                    {
                        Version = new("version", 765),
                        Description = new ChatText($"balls\n{cachedAddress}:{cachedPort}")
                    });
                    await connection.ExchangeStatusPingAsync();
                }
                else if (state == ProtocolState.Login)
                {
                    (string name, Guid guid) = await connection.ReceiveLoginStartAsync();
                    Property[]? properties = await connection.ExchangeLoginEncryptionAsync(guid, name, "test");
                    if (properties == null)
                    {
                        Console.WriteLine("auth failed");
                        properties = [];
                    }
                    await connection.SendLoginCompressionAsync(256, CompressionLevel.Optimal);
                    await connection.ExchangeLoginEndAsync(guid, name, properties);
                    Console.WriteLine($"{name} {guid}");

                    string[] dimensionNames = ["namespace:dimensionname"];
                    Dimension[] dimensionTypes = [new("namespace:dimensiontype")];
                    Biome[] biomes = [new("namespace:biome"), new("minecraft:plains")];
                    string dimensionName = dimensionNames.First();
                    Dimension dimensionType = dimensionTypes.First();
                    await connection.SendConfigurationRegistriesAsync(dimensionTypes, biomes);
                    connection.ReceiveConfigurationMessageAsync += async (string channel, byte[] data) =>
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
                    await connection.ProcessAsync();
                    await connection.SendConfigurationEndAsync();
                    await connection.SendPlayLoginAsync(-1, false, 7, 7, false, true, dimensionNames, dimensionType, dimensionName, 0, Gamemode.Creative, null, false, null);
                    List<(int x, int z, ChunkSectionBlocks[] blocks, int[][] biomes, ChunkSectionLight?[] skyLight, ChunkSectionLight?[] blockLight, CompactArray motionBlocking)> chunks = [];
                    for (int x = -3; x <= 3; x++)
                    {
                        for (int z = -3; z <= 3; z++)
                        {
                            ChunkSectionBlocks[] blocks = new ChunkSectionBlocks[16];
                            Array.Fill(blocks, new(new int[4096], 0));
                            int[][] chunkbiomes = new int[16][];
                            Array.Fill(chunkbiomes, new int[64]);
                            ChunkSectionLight?[] skyLight = new ChunkSectionLight?[18];
                            Array.Fill(skyLight, new(new(4, 4096)));
                            ChunkSectionLight?[] blockLight = new ChunkSectionLight?[18];
                            Array.Fill(blockLight, new(new(4, 4096)));
                            CompactArray motionBlocking = new(8, 256);
                            chunks.Add((x, z, blocks, chunkbiomes, skyLight, blockLight, motionBlocking));
                        }
                    }
                    await connection.SendPlayChunkCenter(0, 0);
                    await connection.SendPlaySpawnpoint(new(0, 0, 0), 0.0f);
                    await connection.SendPlayChunkWait();
                    foreach ((int x, int z, ChunkSectionBlocks[] blocks, int[][] biomes, ChunkSectionLight?[] skyLight, ChunkSectionLight?[] blockLight, CompactArray motionBlocking) chunk in chunks)
                    {
                        await connection.SendPlayChunkFullAsync(chunk.x, chunk.z, chunk.blocks, chunk.biomes, chunk.skyLight, chunk.blockLight, chunk.motionBlocking);
                    }
                    await connection.SendPlayPlayerPosition(0, 0, 0, 0, 0, 0);
                    while (true)
                    {
                        await connection.ProcessAsync();
                        Thread.Sleep(50);
                    }
                }
                else throw new NotSupportedException();
                Console.WriteLine("disconnected.");
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
