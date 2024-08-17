using Net.Myzuc.PurpleStainedGlass.Protocol.Packets;
using Handshake = Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Handshake;
using Status = Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Status;
using Login = Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Login;
using Configuration = Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Configuration;
using Microsoft.VisualStudio.Threading;
using Net.Myzuc.ShioLib;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Security.Cryptography;
using Net.Myzuc.PurpleStainedGlass.Protocol.Objects;
using Net.Myzuc.PurpleStainedGlass.Protocol.Const;
using Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Play;

namespace Net.Myzuc.PurpleStainedGlass.Protocol
{
    public sealed class Client : IDisposable
    {
        public enum EnumState
        {
            Handshake = 0,
            Status = 1,
            Login = 2,
            Configuration = 3,
            Play = 4,
        }
        public EnumState State { get; internal set; }
        internal Stream Stream;
        private readonly AsyncQueue<Serverbound> QueueIn = new();
        private readonly AsyncQueue<Clientbound> QueueOut = new();
        internal int CompressionThreshold = -1;
        internal CompressionLevel CompressionLevel = CompressionLevel.Optimal;
        private Exception? Error = null;
        internal RSA? KeyExchange = null;
        internal RegistryDimension[] DimensionRegistry = [];
        internal RegistryBiome[] BiomeRegistry = [];
        internal RegistryChat[] ChatRegistry = [];
        internal RegistryDamage[] DamageRegistry = [];
        internal RegistryDimension? Dimension = null;
        public Client(Stream stream)
        {
            Stream = stream;
            _ = StartReceivingAsync();
            _ = StartSendingAsync();
        }
        public void Dispose()
        {
            Stream.Dispose();
            QueueIn.Complete();
            QueueOut.Complete();
        }
        public Task<Serverbound> ReceiveAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return QueueIn.DequeueAsync(cancellationToken);
            }
            catch (Exception)
            {
                if (cancellationToken.IsCancellationRequested || Error is null) throw;
                throw Error;
            }
        }
        public void Send(Clientbound packet)
        {
            if (Error is not null) throw Error;
            QueueOut.Enqueue(packet);
        }
        private async Task StartReceivingAsync()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = await DirectReceiveAsync();
                    using MemoryStream stream = new(buffer);
                    int id = stream.ReadS32V();
                    Serverbound? packet = DecodePacket(id, stream);
                    if (packet is null) continue; //TODO: remove
                    if (stream.Position != stream.Length) throw new ProtocolViolationException();
                    QueueIn.Enqueue(packet);
                }
            }
            catch (Exception ex)
            {
                Error ??= ex;
            }
            finally
            {
                Dispose();
            }
        }
        private async Task StartSendingAsync()
        {
            try
            {
                while (true)
                {
                    Clientbound packet = await QueueOut.DequeueAsync();
                    byte[] buffer = packet.Buffer;
                    if (packet.Mode != State) throw new ProtocolViolationException();
                    await DirectSendAsync(buffer);
                    packet.Transform(this);
                }
            }
            catch (Exception ex)
            {
                Error ??= ex;
            }
            finally
            {
                Dispose();
            }
        }
        internal async Task<byte[]> DirectReceiveAsync()
        {
            byte[] data = await Stream.ReadU8AAsync(SizePrefix.S32V, 2097152);
            if (CompressionThreshold < 0) return data;
            using MemoryStream packetIn = new(data);
            int size = packetIn.ReadS32V();
            if (size <= 0) return packetIn.ReadU8A(data.Length - (int)packetIn.Position);
            using ZLibStream zlib = new(packetIn, CompressionMode.Decompress, false);
            return zlib.ReadU8A(size);
        }
        internal async Task DirectSendAsync(byte[] data)
        {
            if (data.Length > 2097152) throw new ProtocolViolationException();
            if (CompressionThreshold >= 0)
            {
                if (data.Length < CompressionThreshold)
                {
                    await Stream.WriteS32VAsync(data.Length + 1);
                    await Stream.WriteU8Async(0);
                    await Stream.WriteU8AAsync(data);
                }
                else
                {
                    using MemoryStream packetStream = new();
                    using (ZLibStream zlib = new(packetStream, CompressionLevel, true)) zlib.Write(data);
                    byte[] compressed = packetStream.ToArray();
                    int extra = 0;
                    for (int value = data.Length; value != 0; value >>= 7) extra++;
                    await Stream.WriteS32VAsync(compressed.Length + extra);
                    await Stream.WriteS32VAsync(data.Length);
                    await Stream.WriteU8AAsync(compressed);
                }
                return;
            }
            await Stream.WriteU8AAsync(data, SizePrefix.S32V);
        }
        private Serverbound? DecodePacket(int id, MemoryStream stream) => State switch
        {
            EnumState.Handshake => id switch
            {
                0x00 => new Handshake.ServerboundHandshake(stream, this),
                _ => throw new ProtocolViolationException()
            },
            EnumState.Status => id switch
            {
                0x00 => new Status.ServerboundStatusRequest(),
                0x01 => new Status.ServerboundPingRequest(stream),
                _ => throw new ProtocolViolationException()
            },
            EnumState.Login => id switch
            {
                0x00 => new Login.ServerboundStart(stream),
                0x01 => new Login.ServerboundEncryptionResponse(stream, this),
                0x02 => new Login.ServerboundCustomResponse(stream),
                0x03 => new Login.ServerboundEnd(this),
                0x04 => new Login.ServerboundCookieResponse(stream),
                _ => throw new ProtocolViolationException()
            },
            EnumState.Configuration => id switch
            {
                0x00 => new Configuration.ServerboundPreferences(stream),
                0x01 => new Configuration.ServerboundCookieResponse(stream),
                0x02 => new Configuration.ServerboundCustom(stream),
                0x03 => new Configuration.ServerboundEnd(this),
                0x04 => new Configuration.ServerboundHeartbeat(stream),
                0x05 => new Configuration.ServerboundPingResponse(stream),
                0x06 => new Configuration.ServerboundResourcepackFeedback(stream),
                0x07 => new Configuration.ServerboundDatapacks(stream),
                _ => throw new ProtocolViolationException()
            },
            EnumState.Play => id switch
            {
                PacketIds.IncomingPlayCommand => new ServerboundChatCommand(stream),
                _ => null//_ => throw new ProtocolViolationException()
            },
            _ => throw new ProtocolViolationException()
        };
    }
}
