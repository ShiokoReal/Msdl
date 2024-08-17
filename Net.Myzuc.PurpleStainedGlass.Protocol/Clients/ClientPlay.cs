/*using Net.Myzuc.PurpleStainedGlass.Protocol.Connections;
using Net.Myzuc.PurpleStainedGlass.Protocol;
using Net.Myzuc.PurpleStainedGlass.Protocol.Chat;
using Net.Myzuc.PurpleStainedGlass.Protocol.Entities;
using Net.Myzuc.PurpleStainedGlass.Protocol.Items;
using Net.Myzuc.PurpleStainedGlass.Protocol.Protocol;
using Net.Myzuc.ShioLib;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Clients
{
    public sealed class ClientPlay
    {
        private readonly ClientOld Client;
        public Func<string, Task> ReceiveCommandAsync = (string text) => Task.CompletedTask;
        public Func<string, Task> ReceiveChatAsync = (string text) => Task.CompletedTask;
        public Func<long, Task> ReceiveHeartbeatAsync = (long id) => Task.CompletedTask;
        public Func<double, double, double, Task> ReceiveLocationAsync = (double x, double y, double z) => Task.CompletedTask;
        public Func<float, float, Task> ReceiveRotationAsync = (float yaw, float pitch) => Task.CompletedTask;
        public Func<Position, float, BlockFace, Task> ReceiveBreakAsync = (Position location, float progress, BlockFace face) => Task.CompletedTask;
        public Func<PlayerAction, Task> ReceiveActionAsync = (PlayerAction action) => Task.CompletedTask;
        public Func<int, Task> ReceiveHotbarAsync = (int slot) => Task.CompletedTask;
        public Func<bool, Task> ReceiveSwingAsync = (bool offhanded) => Task.CompletedTask;
        public Func<bool, Position, BlockFace, float, float, float, bool, Task> ReceiveInteractionBlockAsync = (bool offhanded, Position position, BlockFace face, float cursorX, float cursorY, float cursorZ, bool inside) => Task.CompletedTask;
        public Func<ClientConfiguration, Task> SwitchConfiguration = (ClientConfiguration client) => Task.CompletedTask;
        private bool Complete = false;
        private readonly Dimension[] DimensionTypes;
        private readonly Biome[] Biomes;
        private Dimension? DimensionType = null;
        private string[]? DimensionNames = null;
        private string? DimensionName = null;
        private Gamemode Gamemode = Gamemode.Survival;
        private int EID = 0;
        private bool Hardcore = false;
        private bool RespawnScreen = false;


        public Func<Preferences, Task> ReceivePreferencesAsync = (Preferences preferences) => Task.CompletedTask; //which id


        internal ClientPlay(ClientOld client, Dimension[] dimensionTypes, Biome[] biomes)
        {
            Client = client;
            DimensionTypes = dimensionTypes;
            Biomes = biomes;
        }
        public async Task StartReceivingAsync()
        {
            while (true)
            {
                using MemoryStream packetIn = new(await Client.ReceiveAsync());
                switch (packetIn.ReadS32V())
                {
                    case PacketIds.IncomingPlayCommand:
                        {
                            await ReceiveCommandAsync(packetIn.ReadString(SizePrefix.S32V, 256));
                            break;
                        }
                    case PacketIds.IncomingPlayChat:
                        {
                            await ReceiveChatAsync(packetIn.ReadString(SizePrefix.S32V, 256));
                            break;
                        }
                    case PacketIds.IncomingPlayConfigure:
                        {
                            if (!Complete) throw new ProtocolViolationException();
                            await SwitchConfiguration(new ClientConfiguration(Client));
                            return;
                        }
                    case PacketIds.IncomingPlayHeartbeat:
                        {
                            await ReceiveHeartbeatAsync(packetIn.ReadS64());
                            break;
                        }
                    case PacketIds.IncomingPlayLocation:
                        {
                            await ReceiveLocationAsync(packetIn.ReadF64(), packetIn.ReadF64(), packetIn.ReadF64());
                            break;
                        }
                    case PacketIds.IncomingPlayPosition:
                        {
                            await ReceiveLocationAsync(packetIn.ReadF64(), packetIn.ReadF64(), packetIn.ReadF64());
                            await ReceiveRotationAsync(packetIn.ReadF32(), packetIn.ReadF32());
                            break;
                        }
                    case PacketIds.IncomingPlayRotation:
                        {
                            await ReceiveRotationAsync(packetIn.ReadF32(), packetIn.ReadF32());
                            break;
                        }
                    case PacketIds.IncomingPlayActionGeneric:
                        {
                            int type = packetIn.ReadS32V();
                            Position position = new(packetIn.ReadU64());
                            BlockFace face = (BlockFace)packetIn.ReadS8();
                            using MemoryStream packetOut = new();
                            packetOut.WriteS32V(PacketIds.OutgoingPlayBlockFeedback);
                            packetOut.WriteS32V(packetIn.ReadS32V());
                            await Client.SendAsync(packetOut.ToArray());
                            switch (type)
                            {
                                case 0:
                                    {
                                        await ReceiveBreakAsync(position, Gamemode == Gamemode.Creative ? 1.0f : 0.0f, face);
                                        break;
                                    }
                                case 1:
                                    {
                                        await ReceiveBreakAsync(position, -1.0f, face);
                                        break;
                                    }
                                case 2:
                                    {
                                        await ReceiveBreakAsync(position, Gamemode == Gamemode.Spectator ? 0.0f : 1.0f, face);
                                        break;
                                    }
                                case 3:
                                    {
                                        await ReceiveActionAsync(PlayerAction.DropStack);
                                        break;
                                    }
                                case 4:
                                    {
                                        await ReceiveActionAsync(PlayerAction.DropSingle);
                                        break;
                                    }
                                case 5:
                                    {
                                        //TODO:
                                        break;
                                    }
                                case 6:
                                    {
                                        await ReceiveActionAsync(PlayerAction.Swap);
                                        break;
                                    }
                            }
                            break;
                        }
                    case PacketIds.IncomingPlayActionMovement:
                        {
                            if (packetIn.ReadS32V() != EID) throw new ProtocolViolationException();
                            int type = packetIn.ReadS32V();
                            int strength = packetIn.ReadS32V();
                            switch (type)
                            {
                                case 0:
                                    {
                                        await ReceiveActionAsync(PlayerAction.SneakStart);
                                        break;
                                    }
                                case 1:
                                    {
                                        await ReceiveActionAsync(PlayerAction.SneakStop);
                                        break;
                                    }
                                case 2:
                                    {
                                        //TODO:
                                        break;
                                    }
                                case 3:
                                    {
                                        await ReceiveActionAsync(PlayerAction.SprintStart);
                                        break;
                                    }
                                case 4:
                                    {
                                        await ReceiveActionAsync(PlayerAction.SprintStop);
                                        break;
                                    }
                                case 5:
                                    {
                                        //TODO:
                                        break;
                                    }
                                case 6:
                                    {
                                        //TODO:
                                        break;
                                    }
                                case 7:
                                    {
                                        //TODO:
                                        break;
                                    }
                                case 8:
                                    {
                                        //TODO:
                                        break;
                                    }
                            }
                            break;
                        }
                    case PacketIds.IncomingPlayHotbar:
                        {
                            int slot = packetIn.ReadS16();
                            if (slot < 0 || slot >= 9) throw new ProtocolViolationException();
                            await ReceiveHotbarAsync(slot);
                            break;
                        }
                    case PacketIds.IncomingPlaySwing:
                        {
                            await ReceiveSwingAsync(packetIn.ReadS32V() == 0 ? false : true);
                            break;
                        }
                    case PacketIds.IncomingPlayInteractionBlock:
                        {
                            bool offhanded = packetIn.ReadS32V() == 0 ? false : true;
                            Position position = new(packetIn.ReadU64());
                            BlockFace face = (BlockFace)packetIn.ReadS32V();
                            float cursorX = packetIn.ReadF32();
                            float cursorY = packetIn.ReadF32();
                            float cursorZ = packetIn.ReadF32();
                            bool inside = packetIn.ReadBool();
                            using MemoryStream packetOut = new();
                            packetOut.WriteS32V(PacketIds.OutgoingPlayBlockFeedback);
                            packetOut.WriteS32V(packetIn.ReadS32V());
                            await Client.SendAsync(packetOut.ToArray());
                            await ReceiveInteractionBlockAsync(offhanded, position, face, cursorX, cursorY, cursorZ, inside);
                            break;
                        }
                    default:
                        {
                            //throw new ProtocolViolationException(); //TODO: remove comment
                            break;
                        }
                }
            }
        }
        public Task SendEntityAddAsync(int eid, Guid id, Entity entity, double x, double y, double z, float pitch, float yaw, float headYaw)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayEntityAdd);
            packetOut.WriteS32V(eid);
            packetOut.WriteGuid(id);
            packetOut.WriteS32V(entity.Id);
            packetOut.WriteF64(x);
            packetOut.WriteF64(y);
            packetOut.WriteF64(z);
            packetOut.WriteU8((byte)(pitch / 360.0f * 256));
            packetOut.WriteU8((byte)(yaw / 360.0f * 256));
            packetOut.WriteU8((byte)(headYaw / 360.0f * 256));
            packetOut.WriteS32V(0);
            packetOut.WriteS16(0);
            packetOut.WriteS16(0);
            packetOut.WriteS16(0);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendEntityAnimationAsync(int eid, EntityAnimation animation)
        {
            if (Complete) throw new InvalidOperationException();
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayEntityAnimation);
            packetOut.WriteS32V(eid);
            packetOut.WriteS32V((int)animation);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendBlockSingleAsync(Position position, int id)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayBlockSingle);
            packetOut.WriteU64(position.Data);
            packetOut.WriteS32V(id);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendChunkBiomesAsync(int x, int z, int[][] biomes)
        {
            if (Complete) throw new InvalidOperationException();
            if (biomes.Length * 16 != DimensionType.Value.Height) throw new ArgumentException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayChunkBiomes);
            packetOut.WriteS32V(1);
            packetOut.WriteS32(x);
            packetOut.WriteS32(z);
            using MemoryStream dataOut = new();
            for (int i = 0; i < biomes.Length; i++)
            {
                int[] biome = biomes[i];
                Contract.Assert(biome.Length == 64);
                Dictionary<int, int> counts = [];
                for (int e = 0; e < 64; e++)
                {
                    int id = Math.Clamp(biome[e], 0, Biomes.Length);
                    counts.TryAdd(id, 0);
                    counts[id]++;
                }
                byte bits = (byte)Math.Ceiling(Math.Log2(counts.Count));
                byte totalbits = (byte)Math.Ceiling(Math.Log2(Biomes.Length));
                if (bits >= 4)
                {
                    dataOut.WriteU8(totalbits);
                    SemiCompactArray data = new(totalbits, 64);
                    for (int e = 0; e < data.Length; e++) data[i] = Math.Clamp(biome[e], 0, Biomes.Length);
                    dataOut.WriteU64A(data.Data, SizePrefix.S32V);
                }
                else
                if (bits > 0)
                {
                    dataOut.WriteU8(bits);
                    SemiCompactArray data = new(bits, 64);
                    Dictionary<int, int> mappings = [];
                    dataOut.WriteS32V(counts.Count);
                    foreach (int id in counts.Keys)
                    {
                        mappings[id] = mappings.Count;
                        dataOut.WriteS32V(id);
                    }
                    for (int e = 0; e < data.Length; e++) data[i] = mappings[Math.Clamp(biome[e], 0, Biomes.Length)];
                    dataOut.WriteU64A(data.Data, SizePrefix.S32V);
                }
                else
                {
                    dataOut.WriteU8(0);
                    dataOut.WriteS32V(counts.First().Key);
                    dataOut.WriteS32V(0);
                }
            }
            packetOut.WriteU8A(dataOut.ToArray(), SizePrefix.S32V);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendContainerFullAsync(sbyte id, Item?[] content, Item? carry, int sequence)
        {
            if (Complete) throw new InvalidOperationException();
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayContainerFull);
            packetOut.WriteS8(id);
            packetOut.WriteS32V(sequence);
            packetOut.WriteS32V(content.Length);
            for (int i = 0; i < content.Length; i++)
            {
                Item? item = content[i];
                item?.Serialize(packetOut);
                if (item is null) packetOut.WriteS32V(0);
            }
            carry?.Serialize(packetOut);
            if (carry is null) packetOut.WriteS32V(0);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendContainerSingleAsync(sbyte id, short slot, Item? item, int sequence)
        {
            if (Complete) throw new InvalidOperationException();
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayContainerSingle);
            packetOut.WriteS8(id);
            packetOut.WriteS32V(sequence);
            packetOut.WriteS16(slot);
            item?.Serialize(packetOut);
            if (item is null) packetOut.WriteS32V(0);
            return Client.SendAsync(packetOut.ToArray());
        }
        public async Task SendDisconnectAsync(ChatComponent message)
        {
            if (Complete) throw new InvalidOperationException();
            Complete = true;
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayDisconnect);
            packetOut.WriteU8(0x0A);
            message.Serialize(packetOut);
            await Client.SendAsync(packetOut.ToArray());
            Client.Dispose();
        }
        public Task SendChunkUnloadAsync(int x, int z)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayChunkUnload);
            packetOut.WriteS32(x);
            packetOut.WriteS32(z);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendHeartbeatAsync(long sequence)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayHeartbeat);
            packetOut.WriteS64(sequence);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendChunkLightAsync(int x, int z, SemiCompactArray?[] skyLight, SemiCompactArray?[] blockLight)
        {
            if (Complete) throw new InvalidOperationException();
            if (blockLight.Length * 16 != DimensionType.Value.Height + 2) throw new ArgumentException();
            if (skyLight.Length * 16 != DimensionType.Value.Height + 2) throw new ArgumentException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayChunkLight);
            packetOut.WriteS32(x);
            packetOut.WriteS32(z);
            SemiCompactArray maskSkyLight = new(1, skyLight.Length);
            SemiCompactArray maskSkyLightEmpty = new(1, skyLight.Length);
            SemiCompactArray maskBlockLight = new(1, blockLight.Length);
            SemiCompactArray maskBlockLightEmpty = new(1, blockLight.Length);
            int numSkyLight = 0;
            for (int i = 0; i < skyLight.Length; i++)
            {
                SemiCompactArray? light = skyLight[i];
                if (light is not null)
                {
                    Contract.Assert(light.Bits == 4);
                    Contract.Assert(light.Length == 4096);
                    numSkyLight++;
                    maskSkyLight[i] = light.Data is not null ? 1 : 0;
                    maskSkyLightEmpty[i] = light.Data is null ? 1 : 0;
                }
            }
            int numBlockLight = 0;
            for (int i = 0; i < blockLight.Length; i++)
            {
                SemiCompactArray? light = blockLight[i];
                if (light is not null)
                {
                    Contract.Assert(light.Bits == 4);
                    Contract.Assert(light.Length == 4096);
                    numBlockLight++;
                    maskBlockLight[i] = light.Data is not null ? 1 : 0;
                    maskBlockLightEmpty[i] = light.Data is null ? 1 : 0;
                }
            }
            packetOut.WriteU64A(maskSkyLight.Data, SizePrefix.S32V);
            packetOut.WriteU64A(maskBlockLight.Data, SizePrefix.S32V);
            packetOut.WriteU64A(maskSkyLightEmpty.Data, SizePrefix.S32V);
            packetOut.WriteU64A(maskBlockLightEmpty.Data, SizePrefix.S32V);
            packetOut.WriteS32V(numSkyLight);
            for (int i = 0; i < skyLight.Length; i++)
            {
                if (maskSkyLight[i] == 0) continue;
                packetOut.WriteU8A(MemoryMarshal.AsBytes<ulong>(skyLight[i].Data).ToArray(), SizePrefix.S32V);
            }
            packetOut.WriteS32V(numBlockLight);
            for (int i = 0; i < blockLight.Length; i++)
            {
                if (maskBlockLight[i] == 0) continue;
                packetOut.WriteU8A(MemoryMarshal.AsBytes<ulong>(blockLight[i].Data).ToArray(), SizePrefix.S32V);
            }
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendTablistRemoveAsync(Guid[] id)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayTablistRemove);
            packetOut.WriteS32V(id.Length);
            for (int i = 0; i < id.Length; i++)
            {
                packetOut.WriteGuid(id[i]);
            }
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendTablistUpdateAsync(Guid[] id, (string name, Property[] properties)[]? additions, Gamemode[]? gamemode = null, bool[]? visibility = null, int[]? latency = null, ChatComponent?[]? display = null)
        {
            if (Complete) throw new InvalidOperationException();
            if (additions is not null && additions.Length != id.Length) throw new ArgumentException();
            if (gamemode is not null && gamemode.Length != id.Length) throw new ArgumentException();
            if (visibility is not null && visibility.Length != id.Length) throw new ArgumentException();
            if (latency is not null && latency.Length != id.Length) throw new ArgumentException();
            if (display is not null && display.Length != id.Length) throw new ArgumentException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayTablistAction);
            packetOut.WriteU8((byte)((additions is not null ? 0x01 : 0x00) | (gamemode is not null ? 0x04 : 0x00) | (visibility is not null ? 0x08 : 0x00) | (latency is not null ? 0x10 : 0x00) | (display is not null ? 0x20 : 0x00)));
            packetOut.WriteS32V(id.Length);
            for (int i = 0; i < id.Length; i++)
            {
                packetOut.WriteGuid(id[i]);
                if (additions is not null)
                {
                    (string name, Property[] properties) = additions[i];
                    packetOut.WriteString(name, SizePrefix.S32V, 16);
                    packetOut.WriteS32V(properties.Length);
                    for (int e = 0; e < properties.Length; e++)
                    {
                        Property property = properties[e];
                        packetOut.WriteString(property.Name, SizePrefix.S32V, 32767);
                        packetOut.WriteString(property.Value, SizePrefix.S32V, 32767);
                        packetOut.WriteBool(property.Signature is not null);
                        if (property.Signature is not null) packetOut.WriteString(property.Signature, SizePrefix.S32V, 32767);
                    }
                }
                if (gamemode is not null) packetOut.WriteS32V((int)gamemode[i]);
                if (visibility is not null) packetOut.WriteBool(visibility[i]);
                if (latency is not null) packetOut.WriteS32V(latency[i]);
                if (display is not null)
                {
                    ChatComponent? displayName = display[i];
                    packetOut.WriteBool(displayName is not null);
                    if (displayName is not null)
                    {
                        packetOut.WriteU8(0x0A);
                        displayName.Serialize(packetOut);
                    }
                }
            }
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendPlayerPointBlockAsync(double x, double y, double z, bool eyes, (int eid, bool eyes)? entity)
        {
            if (Complete) throw new InvalidOperationException();
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayPlayerPoint);
            packetOut.WriteS32V(eyes ? 1 : 0);
            packetOut.WriteF64(x);
            packetOut.WriteF64(y);
            packetOut.WriteF64(z);
            packetOut.WriteBool(entity.HasValue);
            if (entity.HasValue)
            {
                packetOut.WriteS32V(entity.Value.eid);
                packetOut.WriteS32V(entity.Value.eyes ? 1 : 0);
            }
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendEntityRemoveAsync(int[] eid)
        {
            if (Complete) throw new InvalidOperationException();
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayEntityRemove);
            packetOut.WriteS32V(eid.Length);
            for (int i = 0; i < eid.Length; i++) packetOut.WriteS32V(eid[i]);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendEffectRemoveAsync(int eid, Effect effect)
        {
            if (Complete) throw new InvalidOperationException();
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayEffectRemove);
            packetOut.WriteS32V(eid);
            packetOut.WriteS32V((int)effect);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendRespawnAsync(Dimension dimensionType, string dimensionName, ulong seedHash, Gamemode currentGamemode, Gamemode? previousGamemode, bool debug, bool flatWorld, DeathLocation? deathLocation, bool reset)
        {
            if (Complete) throw new InvalidOperationException();
            if (!DimensionNames.Contains(dimensionName)) throw new ArgumentException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayRespawn);
            packetOut.WriteS32V(Array.IndexOf(DimensionTypes!, DimensionType = dimensionType));
            packetOut.WriteString(DimensionName = dimensionName, SizePrefix.S32V);
            packetOut.WriteU64(seedHash);
            packetOut.WriteU8((byte)(Gamemode = currentGamemode));
            packetOut.WriteS8(previousGamemode.HasValue ? (sbyte)previousGamemode.Value : (sbyte)-1);
            packetOut.WriteBool(false);
            packetOut.WriteBool(flatWorld);
            packetOut.WriteBool(deathLocation.HasValue);
            if (deathLocation.HasValue)
            {
                Contract.Assert(DimensionNames.Contains(deathLocation.Value.Dimension));
                packetOut.WriteString(deathLocation.Value.Dimension, SizePrefix.S32V);
                packetOut.WriteU64(deathLocation.Value.Location.Data);
            }
            packetOut.WriteS32V(0);
            packetOut.WriteU8((byte)(reset ? 0x00 : 0x03));
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendHotbarAsync(int slot)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayHotbar);
            packetOut.WriteS8((sbyte)(((slot % 9) + 9) % 9));
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendEntityDataAsync(int eid, Entity entity, Entity? previous = null)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayEntityData);
            packetOut.WriteS32V(eid);
            entity.Serialize(packetOut, previous);
            packetOut.WriteU8(0xFF);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendTimeAsync(long time)
        {
            if (Complete) throw new InvalidOperationException();
            Contract.Assert(DimensionName is not null);
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayTime);
            packetOut.WriteS64(time);
            packetOut.WriteS64(time);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendConfigurationAsync()
        {
            if (Complete) throw new InvalidOperationException();
            Complete = true;
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayConfigure);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendChatSystemAsync(ChatComponent message)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayChatSystem);
            packetOut.WriteU8(0x0A);
            message.Serialize(packetOut);
            packetOut.WriteBool(false);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendTablistTextAsync(ChatComponent header, ChatComponent footer)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayTablistText);
            packetOut.WriteU8(0x0A);
            header.Serialize(packetOut);
            packetOut.WriteU8(0x0A);
            footer.Serialize(packetOut);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendEffectAddAsync(int eid, Effect effect, int level, int duration, bool beacon, bool particles, bool icon, bool blend)
        {
            if (Complete) throw new InvalidOperationException();
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(PacketIds.OutgoingPlayEffectAdd);
            packetOut.WriteS32V(eid);
            packetOut.WriteS32V((int)effect);
            packetOut.WriteS32V(level);
            packetOut.WriteS32V(duration);
            byte flags = 0x00;
            if (beacon) flags |= 0x01;
            if (particles) flags |= 0x02;
            if (icon) flags |= 0x04;
            if (blend) flags |= 0x08;
            packetOut.WriteU8(flags);
            return Client.SendAsync(packetOut.ToArray());
        }
        public async Task SendEntityPositionAsync(int eid, (double x, double y, double z, float yaw, float pitch, float headYaw) current, (double x, double y, double z, float yaw, float pitch, float headYaw)? previous = null)
        {
            if (Complete) throw new InvalidOperationException();
            double dx = previous.HasValue ? current.x - previous.Value.x : double.PositiveInfinity;
            double dy = previous.HasValue ? current.y - previous.Value.y : double.PositiveInfinity;
            double dz = previous.HasValue ? current.z - previous.Value.z : double.PositiveInfinity;
            bool dr = previous.HasValue ? (current.yaw != previous.Value.yaw || current.pitch != previous.Value.pitch) : true;
            if (Math.Abs(dx) >= 8 || Math.Abs(dy) >= 8 || Math.Abs(dz) >= 8)
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(PacketIds.OutgoingPlayEntityPositionFar);
                packetOut.WriteS32V(eid);
                packetOut.WriteF64(current.x);
                packetOut.WriteF64(current.y);
                packetOut.WriteF64(current.z);
                packetOut.WriteU8((byte)(current.yaw / 360.0f * 256));
                packetOut.WriteU8((byte)(current.pitch / 360.0f * 256));
                packetOut.WriteBool(false);
                await Client.SendAsync(packetOut.ToArray());
            }
            else if (dr && (dx != 0 || dy != 0 || dz != 0))
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(PacketIds.OutgoingPlayEntityPositionShort);
                packetOut.WriteS32V(eid);
                packetOut.WriteS16((short)(dx * 4096));
                packetOut.WriteS16((short)(dy * 4096));
                packetOut.WriteS16((short)(dz * 4096));
                packetOut.WriteU8((byte)(current.yaw / 360.0f * 256));
                packetOut.WriteU8((byte)(current.pitch / 360.0f * 256));
                packetOut.WriteBool(false);
                await Client.SendAsync(packetOut.ToArray());
            }
            else if (dx != 0 || dy != 0 || dz != 0)
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(PacketIds.OutgoingPlayEntityLocationShort);
                packetOut.WriteS32V(eid);
                packetOut.WriteS16((short)(dx * 4096));
                packetOut.WriteS16((short)(dy * 4096));
                packetOut.WriteS16((short)(dz * 4096));
                packetOut.WriteBool(false);
                await Client.SendAsync(packetOut.ToArray());
            }
            else if (dr)
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(PacketIds.OutgoingPlayEntityRotation);
                packetOut.WriteS32V(eid);
                packetOut.WriteU8((byte)(current.yaw / 360.0f * 256));
                packetOut.WriteU8((byte)(current.pitch / 360.0f * 256));
                packetOut.WriteBool(false);
                await Client.SendAsync(packetOut.ToArray());
            }
            if (previous.HasValue ? current.headYaw != previous.Value.headYaw : true)
            {
                using MemoryStream packetOut = new();
                packetOut.WriteS32V(PacketIds.OutgoingPlayEntityHead);
                packetOut.WriteS32V(eid);
                packetOut.WriteU8((byte)(current.headYaw / 360.0f * 256));
                await Client.SendAsync(packetOut.ToArray());
            }
        }
    }
}
*/