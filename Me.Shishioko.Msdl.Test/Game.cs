using Me.Shishioko.Msdl.Data;
using Me.Shishioko.Msdl.Data.Chat;
using Me.Shishioko.Msdl.Data.Entities;
using Me.Shishioko.SJNetChat;
using Me.Shishioko.SJNetChat.Extensions;
using Net.Myzuc.ShioLib;
using System.Collections.Concurrent;
using System.Drawing;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Me.Shishioko.Msdl.Test
{
    public sealed class Game
    {
        internal int LastEID = -1;
        private readonly Dictionary<string, World> Worlds = [];
        internal readonly List<Biome> Biomes = [];
        internal readonly ConcurrentDictionary<Guid, Player> Players = [];
        private int Running = 0;
        private Func<Task>? Reloader = null;
        public Game()
        {

        }
        internal async Task LoadAsync()
        {
            string worldname = "world:default";
            Worlds.Add(worldname, await World.LoadAsync(this, worldname) ?? new(this, new(worldname, 256, 0)
            {
                Effects = Data.Sky.Overworld,
                HasSkylight = false,
            }));
            Biomes.Add(new("minecraft:plains")
            {
                SkyColor = Color.Black,
                FogColor = Color.Crimson,
                WaterColor = Color.Purple,
                WaterFogColor = Color.Black,
                GrassColor = Color.Red,
            });
            Biomes.Add(new("biome:default"));
        }
        internal async Task ServeAsync(Player player)
        {
            while(player.Connection.ProtocolState != ProtocolState.Disconnected)
            {
                if (Reloader is not null) await Reloader();
                Running++;
                SJNC? sjnc = null;
                try
                {
                    World world = Worlds.Values.First();
                    World? nextWorld = null;
                    await player.Connection.SendConfigurationRegistryAsync(Worlds.Select(world => world.Value.Object).ToArray());
                    await player.Connection.SendConfigurationRegistryAsync(Biomes.ToArray());
                    await player.Connection.SendConfigurationRegistryAsync_Temp_A();
                    await player.Connection.SendConfigurationRegistryAsync_Temp_B();
                    await player.Connection.SendFluidsAsync([1], []);
                    player.Connection.ReceivePreferencesAsync = async (Preferences preferences) =>
                    {
                        player.Entity.Righthanded = preferences.RightHanded;
                    };
                    player.Connection.ReceiveConfigurationMessageAsync = async (string channel, byte[] data) =>
                    {
                        MemoryStream packetIn = new(data);
                        switch (channel)
                        {
                            case "minecraft:brand":
                                {
                                    Console.WriteLine(packetIn.ReadString(SizePrefix.S32V, 256));
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine(channel);
                                    break;
                                }
                        }
                    };
                    await player.Connection.SendConfigurationEndAsync();
                    while (player.Connection.ProtocolState == ProtocolState.Configuration) await player.Connection.ProcessConfigurationAsync();
                    if (player.Connection.ProtocolState == ProtocolState.Disconnected) return;
                    if (!Players.TryAdd(player.ID, player)) return;
                    player.EID = ++LastEID;
                    player.Connection.ReceivePreferencesAsync += async (Preferences preferences) =>
                    {
                        await Task.WhenAll(world.Players.Values.Select(async (currentPlayer) =>
                        {
                            if (currentPlayer == player) return;
                            try
                            {
                                await currentPlayer.Connection.SendEntityDataAsync(player.EID, player.Entity, null);
                            }
                            catch (Exception)
                            {

                            }
                        }));
                    };
                    player.Connection.ReceiveCommandAsync = async (string message) =>
                    {
                        string[] split = message.Split(' ', 2);
                        string command = split[0];
                        string input = split.Length > 1 ? split[1] : string.Empty;
                        _ = BroadcastMessageAsync(new ChatText(string.Empty)
                        {
                            Italic = true,
                            Extra = [
                                new ChatText($"{player.Name} "){
                                    Color = player.DarkColor,
                                },
                                new ChatText($"executed "){
                                    Color = player.MediumColor,
                                },
                                new ChatText($"/{message}"){
                                    Color = player.LightColor,
                                }
                                ]
                        });
                        switch (command)
                        {
                            case "save":
                                {
                                    await world.SaveAsync(true);
                                    break;
                                }
                            case "load":
                                {
                                    SemaphoreSlim sync = new(0, 1);
                                    SemaphoreSlim countsync = new(1, 1);
                                    bool running = false;
                                    Reloader = async () =>
                                    {
                                        await countsync.WaitAsync();
                                        if (Running > 0 || running)
                                        {
                                            countsync.Release();
                                            await sync.WaitAsync();
                                            sync.Release();
                                            return;
                                        }
                                        running = true;
                                        countsync.Release();
                                        string worldname = Worlds.Keys.First();
                                        World world = await World.LoadAsync(this, worldname) ?? Worlds.Values.First();
                                        Worlds.Remove(worldname, out _);
                                        Worlds.Add(worldname, world);
                                        Reloader = null;
                                        sync.Release();
                                    };
                                    break;
                                }
                            case "random":
                                {
                                    SemaphoreSlim sync = new(0, 1);
                                    SemaphoreSlim countsync = new(1, 1);
                                    bool running = false;
                                    Reloader = async () =>
                                    {
                                        await countsync.WaitAsync();
                                        if (Running > 0 || running)
                                        {
                                            countsync.Release();
                                            await sync.WaitAsync();
                                            sync.Release();
                                            return;
                                        }
                                        running = true;
                                        countsync.Release();
                                        Biome baseBiome = Biomes.First();
                                        Biomes.Remove(baseBiome);
                                        Biome biome = new(baseBiome.Name)
                                        {
                                            AdditionsSound = baseBiome.AdditionsSound,
                                            AmbientSound = baseBiome.AmbientSound,
                                            FogColor = baseBiome.FogColor,
                                            MoodSound = baseBiome.MoodSound,
                                            Music = baseBiome.Music,
                                            Precipitation = baseBiome.Precipitation,
                                            FoliageColor = Color.FromArgb(Random.Shared.Next(0, 256), Random.Shared.Next(0, 256), Random.Shared.Next(0, 256)),
                                            GrassColor = Color.FromArgb(Random.Shared.Next(0, 256), Random.Shared.Next(0, 256), Random.Shared.Next(0, 256)),
                                            SkyColor = Color.FromArgb(Random.Shared.Next(0, 256), Random.Shared.Next(0, 256), Random.Shared.Next(0, 256)),
                                            WaterColor = Color.FromArgb(Random.Shared.Next(0, 256), Random.Shared.Next(0, 256), Random.Shared.Next(0, 256)),
                                            WaterFogColor = Color.FromArgb(Random.Shared.Next(0, 256), Random.Shared.Next(0, 256), Random.Shared.Next(0, 256)),
                                        };
                                        Biomes.Add(biome);
                                        Reloader = null;
                                        sync.Release();
                                    };
                                    break;
                                }
                            case "whereis00tf":
                                {
                                    await player.Connection.SendPlayerPointBlockAsync(0.0, 0.0, 0.0, true);
                                    break;
                                }
                            case "dark":
                                {
                                    await player.Connection.SendEffectAddAsync(player.EID, Effect.Darkness, int.MaxValue, 400, true, true, true, true);
                                    //await player.Connection.SendEffectAddAsync(player.EID, Effect.Levitation, int.MaxValue, 100, true, true, true, true);
                                    break;
                                }
                            case "illusion":
                                {
                                    EntityLightning entity = new()
                                    {
                                        Name = new ChatText(player.Name)
                                        {
                                            Color = Color.Crimson,
                                        },
                                        //Pose = EntityBase.EntityPose.Sneaking,
                                        Gravitationless = true,
                                        //Silent = true,
                                    };
                                    Property[] properties = [new("textures", "ewogICJ0aW1lc3RhbXAiIDogMTY1MzIwMTM1OTk5MCwKICAicHJvZmlsZUlkIiA6ICJiMjdjMjlkZWZiNWU0OTEyYjFlYmQ5NDVkMmI2NzE0YSIsCiAgInByb2ZpbGVOYW1lIiA6ICJIRUtUMCIsCiAgInNpZ25hdHVyZVJlcXVpcmVkIiA6IHRydWUsCiAgInRleHR1cmVzIiA6IHsKICAgICJTS0lOIiA6IHsKICAgICAgInVybCIgOiAiaHR0cDovL3RleHR1cmVzLm1pbmVjcmFmdC5uZXQvdGV4dHVyZS82ZDE4NGJkYTRkZDllYWEwOWZmMDMxYzU5ZTkzZTIzN2ZhY2E0MjQzODUxMDYwMTliYjNhMzMxOGZmNTk4ODlmIiwKICAgICAgIm1ldGFkYXRhIiA6IHsKICAgICAgICAibW9kZWwiIDogInNsaW0iCiAgICAgIH0KICAgIH0KICB9Cn0=", "s1r8R8zvhqcgQ+iWl83oSn3ewxlPYIL8z09Z9oqhFVSNeMyq0GZc9NuHWtgrvjRPnxMUkEe4H5yyXACNg+L9S9lyPFcOh8Zl9E8mjD2NscXgTFj/mbO1N+gtgS/b+sLrVebPih72x/rnjoVqOLdJNbAWxQLZH5slo1vbiU9Njx3BZSJBQhKvOoBFfvzg+FXjEfTNiJkWU7yAeecPJN5mj4gsVYCyDGK5IWN81apeGTNfAJheEWFonuvmOnivbVqCQex1CREWIrAFwN+xSgM7Pu0r8DecdGtHihftOz3A/7bFfnoNIGvVuV14U70Hfw8x2UlAOxOlVK2pX6HpxL4b4cq7BZ6ja16pJtwOplfFunQAEGAA11idITtdsN+Q1y2EDKTGtF1n33TacXeJSqGoUDV8MYblDg53HfdvFbI02rnIZpy6A7Wmn9ithUO4D8Bu9EHOs54ei9mANxkfjU0RJ12f/aEhzz+kRCxU6qLBTL7LFaauJbkoAvReCK+F0xZh6TTo39EZfwScWlhzutV3pBvEYXKinJ3t8r9eLbmY7lW169ppT9t9y2IjFlVMrtrVEztXq9NW9DozkHKOxn4rNVmUrPLBH1m0BWo6xheiR+lKIqQSBX7rmDNQeLn8kvMfODWJFhEMksICPU7I7u3wWirxJHVu50oW6v440tfYYEM=")];
                                    Guid id = Guid.NewGuid();
                                    int eid = ++LastEID;
                                    await Task.WhenAll(Players.Values.Select(async currentPlayer =>
                                    {
                                        await currentPlayer.Connection.SendTablistUpdate([id], [(eid.ToString(), properties)]);
                                        await currentPlayer.Connection.SendEntityAddAsync(eid, id, entity, player.X, player.Y, player.Z, player.Pitch, player.Yaw, player.HeadYaw);
                                        await currentPlayer.Connection.SendEntityDataAsync(eid, entity);
                                    }));
                                    break;
                                }
                            case "uwu":
                                {
                                    ChatComponent messageObj;
                                    messageObj = new ChatText("Sex ")
                                    {
                                        Color = Color.Red,
                                        Extra = [new ChatText("with children")]
                                    };
                                    BroadcastMessageAsync(messageObj);
                                    SemaphoreSlim sync = new(0, 1);
                                    SemaphoreSlim countsync = new(1, 1);
                                    bool running = false;
                                    Reloader = async () =>
                                    {
                                        await countsync.WaitAsync();
                                        if (Running > 0 || running)
                                        {
                                            countsync.Release();
                                            await sync.WaitAsync();
                                            sync.Release();
                                            return;
                                        }
                                        running = true;
                                        countsync.Release();
                                        //STUFF THAT NEEDS TO BE DONE BETWEEN RELOADS HERE
                                        Reloader = null;
                                        sync.Release();
                                    };
                                    break;
                                }
                        }
                    };
                    player.Connection.ReceiveBreakAsync = async (Position location, float progress, BlockFace face) =>
                    {
                        world.SetBlock(location.X, location.Y, location.Z, 0);
                    };
                    player.Connection.ReceiveInteractionBlockAsync = async (bool offhanded, Position position, BlockFace face, float cursorX, float cursorY, float cursorZ, bool inside) =>
                    {
                        int x = position.X;
                        int y = position.Y;
                        int z = position.Z;
                        switch (face)
                        {
                            case BlockFace.Top:
                                {
                                    y++;
                                    break;
                                }
                            case BlockFace.Bottom:
                                {
                                    y--;
                                    break;
                                }
                            case BlockFace.North:
                                {
                                    z--;
                                    break;
                                }
                            case BlockFace.South:
                                {
                                    z++;
                                    break;
                                }
                            case BlockFace.West:
                                {
                                    x--;
                                    break;
                                }
                            case BlockFace.East:
                                {
                                    x++;
                                    break;
                                }
                        }
                        world.SetBlock(x, y, z, player.Hotbar[4]?.BlockID ?? 0);
                    };
                    player.Connection.ReceiveLocationAsync = async (double x, double y, double z) =>
                    {
                        await Task.WhenAll(world.Players.Values.Select(async (currentPlayer) =>
                        {
                            if (currentPlayer == player) return;
                            try
                            {
                                await currentPlayer.Connection.SendEntityPositionAsync(player.EID, (x, y, z, player.Yaw, player.Pitch, player.HeadYaw), (player.X, player.Y, player.Z, player.Yaw, player.Pitch, player.HeadYaw));
                            }
                            catch (Exception)
                            {

                            }
                        }));
                        player.X = x;
                        player.Y = y;
                        player.Z = z;
                    };
                    player.Connection.ReceiveRotationAsync = async (float yaw, float pitch) =>
                    {
                        await Task.WhenAll(world.Players.Values.Select(async (currentPlayer) =>
                        {
                            if (currentPlayer == player) return;
                            try
                            {
                                await currentPlayer.Connection.SendEntityPositionAsync(player.EID, (player.X, player.Y, player.Z, yaw, pitch, yaw), (player.X, player.Y, player.Z, player.Yaw, player.Pitch, player.HeadYaw));
                            }
                            catch (Exception)
                            {

                            }
                        }));
                        player.Yaw = yaw;
                        player.HeadYaw = yaw;
                        player.Pitch = pitch;
                    };
                    player.Connection.ReceiveSwingAsync = async (bool offhanded) =>
                    {
                        await Task.WhenAll(world.Players.Values.Select(async (currentPlayer) =>
                        {
                            if (currentPlayer == player) return;
                            try
                            {
                                await currentPlayer.Connection.SendEntityAnimationAsync(player.EID, offhanded ? EntityAnimation.SwingOffhand : EntityAnimation.SwingMain);
                            }
                            catch (Exception)
                            {

                            }
                        }));
                    };
                    player.Connection.ReceiveActionAsync = async (PlayerAction action) =>
                    {
                        //_ = BroadcastMessageAsync(new ChatText($"{player.Name} did {Enum.GetName(action)!}"));
                    };
                    player.Connection.ReceiveHotbarAsync = async (int slot) =>
                    {
                        Item?[] hotbar = [.. player.Hotbar];
                        if (slot < 4)
                        {
                            Array.Copy(hotbar, 0, player.Hotbar, 1, 8);
                            player.Hotbar[0] = hotbar[8];
                        }
                        if (slot > 4)
                        {
                            Array.Copy(hotbar, 1, player.Hotbar, 0, 8);
                            player.Hotbar[8] = hotbar[0];
                        }
                        await player.Connection.SendHotbarAsync(4);
                        /*await player.Connection.SendContainerSingleAsync(-2, 0, new()
                        {
                            ID = player.Hotbar[slot]?.ItemID ?? 0,
                            Count = 1,
                        });*/
                        await player.Connection.SendContainerFullAsync(0, [..Enumerable.Repeat(new Data.Item(), 36), ..player.Hotbar.Select(item => new Data.Item()
                            {
                                ID = item?.ItemID ?? 0,
                                Count = item is not null ? 1 : 0,
                            })], new());
                        //player.Hotbar[0] = player.Hotbar[slot];
                    };
                    player.Connection.ReceiveChatAsync = async (string message) =>
                    {
                        try
                        {
                            if (sjnc is not null) await sjnc.SendAsync(message);
                        }
                        catch (Exception)
                        {

                        }
                    };
                    Func<Task> sjncListener = async () =>
                    {
                        Running++;
                        try
                        {
                            while (player.Connection.ProtocolState == ProtocolState.Play)
                            {
                                try
                                {
                                    SJNC sjncUninitialized = await SJNC.ConnectAsync(new IPEndPoint(IPAddress.Loopback, 1338));
                                    SJNCNameExtension nameExtension = new(player.Name);
                                    SJNCOnlineNameListExtension nameListExtension = new(null);
                                    SJNCAesExtension aes = new();
                                    nameListExtension.OnAddAsync += async (name) =>
                                    {
                                        byte[] colorBytes = MD5.HashData(Encoding.UTF8.GetBytes(name));
                                        Color color = Color.FromArgb(colorBytes[0], colorBytes[1], colorBytes[2]);
                                        Color darkColor = Color.FromArgb(color.R / 2, color.G / 2, color.B / 2);
                                        Color mediumColor = Color.FromArgb(darkColor.R + 64, darkColor.G + 64, darkColor.B + 64);
                                        Color lightColor = Color.FromArgb(darkColor.R + 128, darkColor.G + 128, darkColor.B + 128);
                                        await player.Connection.SendChatSystemAsync(new ChatText($"{name} connected.")
                                        {
                                            Color = darkColor,
                                            Italic = true,
                                        });
                                    };
                                    nameListExtension.OnRemoveAsync += async (name) =>
                                    {
                                        byte[] colorBytes = MD5.HashData(Encoding.UTF8.GetBytes(name));
                                        Color color = Color.FromArgb(colorBytes[0], colorBytes[1], colorBytes[2]);
                                        Color darkColor = Color.FromArgb(color.R / 2, color.G / 2, color.B / 2);
                                        Color mediumColor = Color.FromArgb(darkColor.R + 64, darkColor.G + 64, darkColor.B + 64);
                                        Color lightColor = Color.FromArgb(darkColor.R + 128, darkColor.G + 128, darkColor.B + 128);
                                        await player.Connection.SendChatSystemAsync(new ChatText($"{name} disconnected.")
                                        {
                                            Color = darkColor,
                                            Italic = true,
                                        });
                                    };
                                    await sjncUninitialized.InitializeAsync([nameExtension, nameListExtension, aes]);
                                    sjnc = sjncUninitialized;
                                    while (true)
                                    {
                                        string message = await sjnc.ReceiveAsync();
                                        string name = nameExtension.AttachedName ?? "null";
                                        byte[] colorBytes = MD5.HashData(Encoding.UTF8.GetBytes(name));
                                        Color color = Color.FromArgb(colorBytes[0], colorBytes[1], colorBytes[2]);
                                        Color darkColor = Color.FromArgb(color.R / 2, color.G / 2, color.B / 2);
                                        Color mediumColor = Color.FromArgb(darkColor.R + 64, darkColor.G + 64, darkColor.B + 64);
                                        Color lightColor = Color.FromArgb(darkColor.R + 128, darkColor.G + 128, darkColor.B + 128);
                                        await player.Connection.SendChatSystemAsync(new ChatText("")
                                        {
                                            Extra = [
                                                new ChatText($"{name} ")
                                                {
                                                    Color = mediumColor,
                                                },
                                                new ChatText(message)
                                                {
                                                    Color = lightColor,
                                                }
                                            ]
                                        });
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                                sjnc = null;
                            }
                        }
                        catch (Exception)
                        {
                            try
                            {
                                await player.Connection.SendChatSystemAsync(new ChatText("SJNC crashed"));
                            }
                            catch (Exception)
                            {

                            }
                        }
                        finally
                        {
                            Running--;
                        }
                    };
                    _ = sjncListener();
                    Player[] currentPlayers = [.. Players.Values];
                    await player.Connection.SendTablistText(new ChatText("header")
                    {
                        Color = Color.Yellow
                    }, new ChatText("footer")
                    {
                        Color = Color.AliceBlue
                    });
                    await player.Connection.SendTablistUpdate(currentPlayers.Select(currentPlayer => currentPlayer.ID).ToArray(), currentPlayers.Select(currentPlayer => (currentPlayer.Name, currentPlayer.Properties)).ToArray(), null, currentPlayers.Select(currentPlayer => true).ToArray(), null, null);
                    await Task.WhenAll(Players.Values.Select(async (currentPlayer) =>
                    {
                        if (currentPlayer == player) return;
                        try
                        {
                            await currentPlayer.Connection.SendTablistUpdate([player.ID], [(player.Name, player.Properties)], null, [true], null, null);
                        }
                        catch (Exception)
                        {

                        }
                    }));
                    while (player.Connection.ProtocolState == ProtocolState.Play)
                    {
                        if (nextWorld is not null) world = nextWorld;
                        nextWorld = null;
                        if (!world.Players.TryAdd(player.ID, player)) return;
                        try
                        {
                            await player.Connection.SendInitializeAsync(player.EID, false, 8, 8, false, true, Worlds.Keys.ToArray(), world.Object, world.Name, 0, Gamemode.Creative, null, false, null); ;
                            //await player.Connection.SendTime(13670); 
                            await player.Connection.SendTime(0);
                            await player.Connection.SendHotbarAsync(4);
                            await Task.WhenAll(world.Players.Values.Select(async (currentPlayer) =>
                            {
                                if (currentPlayer == player) return;
                                await player.Connection.SendEntityAddAsync(currentPlayer.EID, currentPlayer.ID, currentPlayer.Entity, currentPlayer.X, currentPlayer.Y, currentPlayer.Z, currentPlayer.Pitch, currentPlayer.Yaw, currentPlayer.HeadYaw);
                                await player.Connection.SendEntityDataAsync(currentPlayer.EID, currentPlayer.Entity, null);
                                try
                                {
                                    await currentPlayer.Connection.SendEntityAddAsync(player.EID, player.ID, player.Entity, player.X, player.Y, player.Z, player.Pitch, player.Yaw, player.HeadYaw);
                                    await currentPlayer.Connection.SendEntityDataAsync(player.EID, player.Entity, null);
                                }
                                catch (Exception)
                                {

                                }
                            }));
                            await player.Connection.SendContainerFullAsync(0, [..Enumerable.Repeat(new Data.Item(), 36), ..player.Hotbar.Select(item => new Data.Item()
                            {
                                ID = item?.ItemID ?? 0,
                                Count = item is not null ? 1 : 0,
                            })], new());
                            await player.Connection.SendChunkCenterAsync(0, 0);
                            await player.Connection.SendSpawnpointAsync(new(0, 128, 0), 0.0f);
                            await player.Connection.SendChunkWaitAsync();
                            foreach (Chunk chunk in world.Chunks.Values)
                            {
                                await player.Connection.SendChunkFullAsync(chunk.X, chunk.Z, chunk.Blocks, chunk.Biomes, chunk.Skylight, chunk.Blocklight, chunk.MotionBlocking);
                            }
                            await player.Connection.SendPlayerPositionAsync(0, 128, 0, 0, 0, 0);
                            /*await player.Connection.SendContainerSingleAsync(-2, 0, new()
                            {
                                ID = 35,
                                Count = int.MaxValue,
                            });
                            {
                                EntitySkeleton test = new()
                                {
                                    Pose = EntityPose.Sitting,
                                    SleepingLocation = new(0, 68, 0),
                                    DisplayName = new ChatText("testtt")
                                    {
                                        Color = Color.Crimson,
                                    }
                                };
                                await player.Connection.SendEntitySpawnAsync(69420, Guid.NewGuid(), test, 0, 66, 0, 0, 0, 0);
                                await player.Connection.SendEntityDataAsync(69420, test, null);

                                Property[] properties = [new("textures", "ewogICJ0aW1lc3RhbXAiIDogMTY1MzIwMTM1OTk5MCwKICAicHJvZmlsZUlkIiA6ICJiMjdjMjlkZWZiNWU0OTEyYjFlYmQ5NDVkMmI2NzE0YSIsCiAgInByb2ZpbGVOYW1lIiA6ICJIRUtUMCIsCiAgInNpZ25hdHVyZVJlcXVpcmVkIiA6IHRydWUsCiAgInRleHR1cmVzIiA6IHsKICAgICJTS0lOIiA6IHsKICAgICAgInVybCIgOiAiaHR0cDovL3RleHR1cmVzLm1pbmVjcmFmdC5uZXQvdGV4dHVyZS82ZDE4NGJkYTRkZDllYWEwOWZmMDMxYzU5ZTkzZTIzN2ZhY2E0MjQzODUxMDYwMTliYjNhMzMxOGZmNTk4ODlmIiwKICAgICAgIm1ldGFkYXRhIiA6IHsKICAgICAgICAibW9kZWwiIDogInNsaW0iCiAgICAgIH0KICAgIH0KICB9Cn0=", "s1r8R8zvhqcgQ+iWl83oSn3ewxlPYIL8z09Z9oqhFVSNeMyq0GZc9NuHWtgrvjRPnxMUkEe4H5yyXACNg+L9S9lyPFcOh8Zl9E8mjD2NscXgTFj/mbO1N+gtgS/b+sLrVebPih72x/rnjoVqOLdJNbAWxQLZH5slo1vbiU9Njx3BZSJBQhKvOoBFfvzg+FXjEfTNiJkWU7yAeecPJN5mj4gsVYCyDGK5IWN81apeGTNfAJheEWFonuvmOnivbVqCQex1CREWIrAFwN+xSgM7Pu0r8DecdGtHihftOz3A/7bFfnoNIGvVuV14U70Hfw8x2UlAOxOlVK2pX6HpxL4b4cq7BZ6ja16pJtwOplfFunQAEGAA11idITtdsN+Q1y2EDKTGtF1n33TacXeJSqGoUDV8MYblDg53HfdvFbI02rnIZpy6A7Wmn9ithUO4D8Bu9EHOs54ei9mANxkfjU0RJ12f/aEhzz+kRCxU6qLBTL7LFaauJbkoAvReCK+F0xZh6TTo39EZfwScWlhzutV3pBvEYXKinJ3t8r9eLbmY7lW169ppT9t9y2IjFlVMrtrVEztXq9NW9DozkHKOxn4rNVmUrPLBH1m0BWo6xheiR+lKIqQSBX7rmDNQeLn8kvMfODWJFhEMksICPU7I7u3wWirxJHVu50oW6v440tfYYEM=")];
                                for (int i = 0; i < 512; i++)
                                {
                                    await player.Connection.SendEntitySpawnAsync(69421 + i, id, test2, Random.Shared.Next(-64, 64), Random.Shared.Next(64, 96), Random.Shared.Next(-64, 64), (byte)Random.Shared.Next(0, 256), (byte)Random.Shared.Next(0, 256), (byte)Random.Shared.Next(0, 256));
                                    await player.Connection.SendEntityDataAsync(69421 + i, test2, null);
                                }
                            }*/
                            while (nextWorld is null && player.Connection.ProtocolState == ProtocolState.Play)
                            {
                                if (Reloader is not null)
                                {
                                    await player.Connection.SendReconfigureAsync();
                                    while (player.Connection.ProtocolState == ProtocolState.PlayToConfiguration)
                                    {
                                        await player.Connection.ProcessPlayAsync();
                                        await Task.Delay(50);
                                    }
                                    break;
                                }
                                await player.Connection.ProcessPlayAsync();
                                await Task.Delay(50);
                            }
                            if (Reloader is not null) break;
                            await Task.WhenAll(world.Players.Values.Select(async (currentPlayer) =>
                            {
                                if (currentPlayer == player) return;
                                if (player.Connection.ProtocolState == ProtocolState.Play)
                                {
                                    await player.Connection.SendEntityRemoveAsync([currentPlayer.EID]);
                                }
                                try
                                {
                                    await currentPlayer.Connection.SendEntityRemoveAsync([player.EID]);
                                }
                                catch (Exception)
                                {

                                }
                            }));
                        }
                        finally
                        {
                            world.Players.TryRemove(player.ID, out _);
                        }
                    }
                    if (Reloader is not null) continue;
                    await Task.WhenAll(Players.Values.Select(async (currentPlayer) =>
                    {
                        if (currentPlayer == player) return;
                        try
                        {
                            await currentPlayer.Connection.SendTablistRemove([player.ID]);
                        }
                        catch (Exception)
                        {

                        }
                    }));
                    if (player.Connection.ProtocolState == ProtocolState.Play)
                    {
                        await player.Connection.SendTablistRemove(Players.Values.Select(currentPlayer => currentPlayer.ID).ToArray());
                    }
                }
                finally
                {
                    Players.Remove(player.ID, out _);
                    sjnc?.Dispose();
                    Running--;
                }
            }
        }
        public Task BroadcastMessageAsync(ChatComponent message)
        {
            Console.WriteLine(message.ToANSI());
            return Task.WhenAll(Players.Values.Select(async player =>
            {
                try
                {
                    if (player.Connection.ProtocolState == ProtocolState.Play)
                    {
                        await player.Connection.SendChatSystemAsync(message);
                    }
                }
                catch (Exception)
                {

                }
            }));
        }
    }
}
