using Microsoft.VisualStudio.Threading;
using Net.Myzuc.ShioLib;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace Me.Shishioko.Msdl.Connections
{
    public sealed class Client : IDisposable
    {
        internal Stream Stream;
        internal readonly SemaphoreSlim SyncWrite = new(1, 1);
        internal readonly SemaphoreSlim SyncRead = new(1, 1);
        internal int CompressionThreshold = -1;
        internal CompressionLevel CompressionLevel = CompressionLevel.Optimal;
        internal Client(Stream stream)
        {
            Stream = stream;
        }
        public void Dispose()
        {
            Stream.Dispose();
            SyncWrite.Dispose();
            SyncRead.Dispose();
        }
        internal async Task<byte[]> ReceiveAsync()
        {
            await SyncRead.WaitAsync();
            try
            {
                byte[] data = await Stream.ReadU8AAsync(SizePrefix.S32V);
                if (CompressionThreshold < 0) return data;
                using MemoryStream packetIn = new(data);
                int size = packetIn.ReadS32V();
                if (size <= 0) return packetIn.ReadU8A(data.Length - (int)packetIn.Position);
                using ZLibStream zlib = new(packetIn, CompressionMode.Decompress, false);
                return zlib.ReadU8A(size);
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
            finally
            {
                SyncRead.Release();
            }
        }
        internal async Task SendAsync(byte[] data)
        {
            await SyncWrite.WaitAsync();
            try
            {
                if (CompressionThreshold < 0) await Stream.WriteU8AAsync(data, SizePrefix.S32V);
                else
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
                }
            }
            catch(Exception)
            {
                Dispose();
                throw;
            }
            finally
            {
                SyncWrite.Release();
            }
        }
    }
}
