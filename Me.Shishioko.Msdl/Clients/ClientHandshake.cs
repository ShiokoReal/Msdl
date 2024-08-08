using Me.Shishioko.Msdl.Connections;
using Me.Shishioko.Msdl.Data.Protocol;
using Net.Myzuc.ShioLib;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Me.Shishioko.Msdl.Clients
{
    public sealed class ClientHandshake
    {
        private readonly Client Client;
        public Func<int, string, ushort, Task> ReceiveHandshake = (int version, string address, ushort port) => Task.CompletedTask;
        public Func<ClientStatus, Task> SwitchStatus = (ClientStatus client) => Task.CompletedTask;
        public Func<ClientLogin, Task> SwitchLogin = (ClientLogin client) => Task.CompletedTask;
        public ClientHandshake(Stream stream)
        {
            Client = new(stream);
        }
        public async Task StartReceivingAsync()
        {
            while (true)
            {
                using MemoryStream packetIn = new(await Client.ReceiveAsync());
                switch (packetIn.ReadS32V())
                {
                    case Packets.IncomingHandshakeHandshake:
                        {
                            int version = packetIn.ReadS32V();
                            await ReceiveHandshake(version, packetIn.ReadString(SizePrefix.S32V, 255), packetIn.ReadU16());
                            switch (packetIn.ReadS32V())
                            {
                                case 1:
                                    {
                                        await SwitchStatus(new ClientStatus(Client));
                                        return;
                                    }
                                case 2:
                                    {
                                        if (version != 766) Client.Dispose();
                                        else _ = SwitchLogin(new ClientLogin(Client));
                                        break;
                                    }
                                default:
                                    {
                                        throw new ProtocolViolationException();
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            throw new ProtocolViolationException();
                        }
                }
            }
        }
    }
}
