using Handshake = Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Handshake;
using Status = Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Status;
using Login = Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Login;
using System.Net;
using System.IO;
using System;
using System.Threading.Tasks;
using Net.Myzuc.PurpleStainedGlass.Protocol;
using System.Reflection;
using Net.Myzuc.PurpleStainedGlass.Extensions;

namespace Net.Myzuc.PurpleStainedGlass
{
    public static class Server
    {
        private static async Task Main(string[] args)
        {
            await ExtensionRegistry.RegisterAsync(new ExtensionStandardListener());
            await ExtensionRegistry.RegisterAsync(new ExtensionStandardCompression());
            await ExtensionRegistry.RegisterAsync(new ExtensionStandardAuthentication());
            await ExtensionRegistry.RegisterAsync(new ExtensionTest());
            await Task.Delay(-1);
        }
        public static void RegisterConnection(Stream stream)
        {
            _ = HandleConnectionAsync(stream);
        }
        private static async Task HandleConnectionAsync(Stream stream)
        {
            try
            {
                using Client client = new(stream);
                Handshake.ServerboundHandshake handshake = await client.ReceiveAsync() as Handshake.ServerboundHandshake ?? throw new ProtocolViolationException();
                if (handshake.State == Client.EnumState.Status)
                {
                    if (await client.ReceiveAsync() is not Status.ServerboundStatusRequest) throw new ProtocolViolationException();
                    AssemblyName assembly = Assembly.GetExecutingAssembly().GetName();
                    Protocol.Objects.Status status = new()
                    {
                        Version = new($"{assembly.Name} {assembly.Version}", 766), //TODO: const
                    };
                    await ExtensionRegistry.ExecuteOrderlyAsync(extension => extension.OnStatusAsync(status, handshake));
                    client.Send(new Status.ClientboundStatusResponse(status));
                    Status.ServerboundPingRequest ping = await client.ReceiveAsync() as Status.ServerboundPingRequest ?? throw new ProtocolViolationException();
                    client.Send(new Status.ClientboundPingResponse(ping.Sequence));
                    await Task.Delay(1000); //TODO: method to wait for queue to empty before disconnect
                    return;
                }
                if (handshake.State == Client.EnumState.Login)
                {
                    Login.ServerboundStart login = await client.ReceiveAsync() as Login.ServerboundStart ?? throw new ProtocolViolationException();
                    LoginRequest request = new(client, handshake, login);
                    Task task = request.StartReceivingAsync();
                    await ExtensionRegistry.ExecuteOrderlyAsync(extension => extension.OnLoginAsync(request));
                    await request.StopReceivingAsync();
                    await task;
                    //play
                    await Task.Delay(1000);
                }
            }
            catch(Exception ex)
            {
                //TODO: log
            }
        }
    }
}
