using Me.Shishioko.Msdl.Data;
using Net.Myzuc.ShioLib;
using System.Collections.Concurrent;

namespace Me.Shishioko.Msdl.Test
{
    public sealed class World
    {
        public readonly string Name;
        public readonly Game Game;
        internal readonly Dimension Object; //TODO: class not struct
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
                    Chunks.TryAdd((x, z), new(x, z, this));
                }
            }
        }
        internal async Task SaveAsync(bool full)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "save", Name.Replace(':', '.'));
            Directory.CreateDirectory(path).Create();
            using MemoryStream dataOut = new();
            dataOut.WriteS32V(0);
            dataOut.WriteS32V(Object.Height);
            dataOut.WriteS32V(Object.Depth);
            dataOut.WriteS32V((int)Object.Effects);
            dataOut.WriteBool(Object.Natural);
            dataOut.WriteBool(Object.HasSkylight);
            dataOut.WriteF32(Object.AmbientLight);
            using FileStream fileOut = new(Path.Combine(path, "info.bin"), FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            await fileOut.WriteU8AAsync(dataOut.ToArray());
            if (full) await Task.WhenAll(Chunks.Values.Select(chunk => chunk.SaveAsync()));
        }
        internal static async Task<World?> LoadAsync(Game game, string name)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "save", name.Replace(':', '.'), "info.bin");
            if (!File.Exists(path)) return null;
            using FileStream dataIn = new(path, FileMode.Open, FileAccess.Read, FileShare.None);
            int version = dataIn.ReadS32V();
            World world = new(game, new(name, dataIn.ReadS32V(), dataIn.ReadS32V())
            {
                Effects = (Sky)dataIn.ReadS32V(),
                Natural = dataIn.ReadBool(),
                HasSkylight = dataIn.ReadBool(),
                AmbientLight = dataIn.ReadF32()
            });
            await Task.WhenAll(world.Chunks.Values.Select(chunk => chunk.LoadAsync()));
            return world;
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
