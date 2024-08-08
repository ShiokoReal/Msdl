using Me.Shishioko.Msdl.Connections;
using Me.Shishioko.Msdl.Data;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Me.Shishioko.Msdl.Clients
{
    public sealed class ClientStatus
    {
        private readonly Client Client;
        public Func<Task> ReceiveStatusRequest = () => Task.CompletedTask;
        public Func<long, Task> ReceivePingRequest = (long sequence) => Task.CompletedTask;
        internal ClientStatus(Client client)
        {
            Client = client;
        }
        public async Task StartReceivingAsync()
        {
            while (true)
            {
                using MemoryStream packetIn = new(await Client.ReceiveAsync());
                switch (packetIn.ReadS32V())
                {
                    case Packets.IncomingStatusStatusRequest:
                        {
                            await ReceiveStatusRequest();
                            break;
                        }
                    case Packets.IncomingStatusPingRequest:
                        {
                            await ReceivePingRequest(packetIn.ReadS64());
                            break;
                        }
                    default:
                        {
                            throw new ProtocolViolationException();
                        }
                }
            }
        }
        public Task SendStatusResponseAsync(ServerStatus status)
        {
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingStatusStatusResponse);
            packetOut.WriteString(status.TextSerialize(), SizePrefix.S32V);
            return Client.SendAsync(packetOut.ToArray());
        }
        public Task SendPingResponseAsync(long sequence)
        {
            using MemoryStream packetOut = new();
            packetOut.WriteS32V(Packets.OutgoingStatusPingResponse);
            packetOut.WriteS64(sequence);
            return Client.SendAsync(packetOut.ToArray());
        }
    }
}
