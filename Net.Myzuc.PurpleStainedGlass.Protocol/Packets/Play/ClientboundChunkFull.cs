using Net.Myzuc.PurpleStainedGlass.Protocol.Const;
using Net.Myzuc.ShioLib;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Net;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Play
{
    public sealed class ClientboundChunkFull : Clientbound
    {
        public override Client.EnumState Mode => Client.EnumState.Play;
        private readonly int Sections;
        private readonly int BiomeCount;
        public ClientboundChunkFull(int x, int z, int[][] blocks, int[][] biomes, int biomeCount, SemiCompactArray?[] skyLight, SemiCompactArray?[] blockLight, SemiCompactArray motionBlocking)
        {
            BiomeCount = biomeCount;
            Sections = blocks.Length;
            if (biomes.Length != Sections) throw new ArgumentException();
            if (blockLight.Length != Sections + 2) throw new ArgumentException();
            if (skyLight.Length != Sections + 2) throw new ArgumentException();
            if (motionBlocking.Bits != (int)Math.Ceiling(Math.Log2(Sections << 4))) throw new ArgumentException();
            if (motionBlocking.Length != 256) throw new ArgumentException();
            using MemoryStream stream = new();
            stream.WriteS32V(PacketIds.OutgoingPlayChunkFull);
            stream.WriteS32(x);
            stream.WriteS32(z);
            stream.WriteU8(0x0A);
            stream.WriteU8(12);
            stream.WriteString("MOTION_BLOCKING", SizePrefix.S16); //TODO: proper heightmaps
            stream.WriteS32(motionBlocking.Data.Length);
            stream.WriteU64A(motionBlocking.Data);
            stream.WriteU8(0);
            using MemoryStream dataOut = new();
            for (int i = 0; i < Sections; i++)
            {
                int[] block = blocks[i];
                Contract.Assert(block.Length == 4096); //TODO: not these
                Dictionary<int, int> blockCounts = [];
                ushort count = 0;
                for (int e = 0; e < 4096; e++)
                {
                    int id = Math.Clamp(block[e], 0, 32768); //TODO: proper auto updating value
                    if (id != 0) count++; //TODO: support other air block types
                    blockCounts.TryAdd(id, 0);
                    blockCounts[id]++;
                }
                dataOut.WriteU16(count);
                byte blockBits = (byte)Math.Ceiling(Math.Log2(blockCounts.Count));
                byte blockBitsTotal = (byte)Math.Ceiling(Math.Log2(32768)); //TODO: proper auto updating value
                if (blockBits > 8)
                {
                    SemiCompactArray data = new(blockBitsTotal, 4096);
                    dataOut.WriteU8(data.Bits);
                    for (int e = 0; e < data.Length; e++) data[e] = Math.Clamp(block[e], 0, 32768); //TODO: proper auto updating value
                    dataOut.WriteU64A(data.Data, SizePrefix.S32V);
                }
                else
                if (blockBits > 0)
                {
                    byte usedBits = blockBits < 4 ? (byte)4 : blockBits;
                    dataOut.WriteU8(usedBits);
                    SemiCompactArray data = new(usedBits, 4096);
                    Dictionary<int, int> mappings = [];
                    dataOut.WriteS32V(blockCounts.Count);
                    foreach (int id in blockCounts.Keys)
                    {
                        mappings[id] = mappings.Count;
                        dataOut.WriteS32V(id);
                    } //TODO: below: fix evaluation time of clamp thing and in other occurences too
                    for (int e = 0; e < 4096; e++) data[e] = mappings[Math.Clamp(block[e], 0, 32768)]; //TODO: proper auto updating value
                    dataOut.WriteU64A(data.Data, SizePrefix.S32V);
                }
                else
                {
                    dataOut.WriteU8(0);
                    dataOut.WriteS32V(blockCounts.First().Key);
                    dataOut.WriteS32V(0);
                }
                int[] biome = biomes[i];
                Contract.Assert(biome.Length == 64);
                Dictionary<int, int> biomeCounts = [];
                for (int e = 0; e < 64; e++)
                {
                    int id = Math.Clamp(biome[e], 0, biomeCount);
                    biomeCounts.TryAdd(id, 0);
                    biomeCounts[id]++;
                }
                byte biomeBits = (byte)Math.Ceiling(Math.Log2(biomeCounts.Count));
                byte biomeBitsTotal = (byte)Math.Ceiling(Math.Log2(biomeCount));
                if (biomeBits >= 4)
                {
                    dataOut.WriteU8(biomeBitsTotal);
                    SemiCompactArray data = new(biomeBitsTotal, 64);
                    for (int e = 0; e < data.Length; e++) data[i] = Math.Clamp(biome[e], 0, biomeCount);
                    dataOut.WriteU64A(data.Data, SizePrefix.S32V);
                }
                else
                if (biomeBits > 0)
                {
                    dataOut.WriteU8(biomeBits);
                    SemiCompactArray data = new(biomeBits, 64);
                    Dictionary<int, int> mappings = [];
                    dataOut.WriteS32V(biomeCounts.Count);
                    foreach (int id in biomeCounts.Keys)
                    {
                        mappings[id] = mappings.Count;
                        dataOut.WriteS32V(id);
                    }
                    for (int e = 0; e < data.Length; e++) data[i] = mappings[Math.Clamp(biome[e], 0, biomeCount)];
                    dataOut.WriteU64A(data.Data, SizePrefix.S32V);
                }
                else
                {
                    dataOut.WriteU8(0);
                    dataOut.WriteS32V(biomeCounts.First().Key);
                    dataOut.WriteS32V(0);
                }
            }
            stream.WriteU8A(dataOut.ToArray(), SizePrefix.S32V);
            stream.WriteS32V(0); //TODO: block entities
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
            stream.WriteU64A(maskSkyLight.Data, SizePrefix.S32V);
            stream.WriteU64A(maskBlockLight.Data, SizePrefix.S32V);
            stream.WriteU64A(maskSkyLightEmpty.Data, SizePrefix.S32V);
            stream.WriteU64A(maskBlockLightEmpty.Data, SizePrefix.S32V);
            stream.WriteS32V(numSkyLight);
            for (int i = 0; i < skyLight.Length; i++)
            {
                if (maskSkyLight[i] == 0) continue;
                stream.WriteU8A(MemoryMarshal.AsBytes<ulong>(skyLight[i].Data).ToArray(), SizePrefix.S32V);
            }
            stream.WriteS32V(numBlockLight);
            for (int i = 0; i < blockLight.Length; i++)
            {
                if (maskBlockLight[i] == 0) continue;
                stream.WriteU8A(MemoryMarshal.AsBytes<ulong>(blockLight[i].Data).ToArray(), SizePrefix.S32V);
            }
            Buffer = stream.ToArray();
        }
        internal override void Transform(Client client)
        {
            if (client.Dimension is null) throw new ProtocolViolationException();
            if (client!.Dimension.Height >> 4 != Sections) throw new ProtocolViolationException();
            if (client.BiomeRegistry.Length != BiomeCount) throw new ProtocolViolationException();
        }
    }
}
