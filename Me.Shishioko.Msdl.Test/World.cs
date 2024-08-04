using Me.Shishioko.Msdl.Data;
using System.Collections.Concurrent;

namespace Me.Shishioko.Msdl.Test
{
    public sealed class World
    {
        public readonly string Name;
        public readonly Game Game;
        internal readonly Dimension Object; //TODO: readonly struct for biomes too
        internal readonly ConcurrentDictionary<Guid, Player> Players = [];
        internal readonly ConcurrentDictionary<(int x, int z), Chunk> Chunks = [];
        public World(Game game, Dimension dimension)
        {
            Game = game;
            Name = dimension.Name;
            Object = dimension;
            for (int x = -4; x < 4; x++)
            {
                for (int z = -4; z < 4; z++)
                {
                    Chunk chunk = new(x, z, this);
                    for (int y = Object.Depth; y < Object.Depth + Object.Height; y++)
                    {
                        int block = 0;
                        if (y < 64) block = 1;
                        else if (y == 64) block = 2;
                        else if (y <= 67) block = 95;
                        for (int xx = 0; xx < 16; xx++)
                        {
                            for (int zz = 0; zz < 16; zz++)
                            {
                                chunk[xx, y, zz] = block;
                            }
                        }
                    }
                    Chunks.TryAdd((chunk.X, chunk.Z), chunk);
                }
            }
        }
        public void SetBlock(int x, int y, int z, int id)
        {
            int xx = x >> 4;
            int zz = z >> 4;
            if (!Chunks.TryGetValue((xx, zz), out Chunk? chunk) || y < Object.Depth || y >= Object.Depth + Object.Height) //TODO: acknowledge
            {
                return;
            }
            chunk[x & 15, y, z & 15] = id;
            _ = Task.WhenAll(Players.Values.Select(async (currentPlayer) => currentPlayer.Connection.SendBlockSingleAsync(new(x, y, z), id)));
        }
    }
}
