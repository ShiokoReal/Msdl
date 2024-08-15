using Net.Myzuc.PurpleStainedGlass.Protocol.Data.Chat;
using Net.Myzuc.PurpleStainedGlass.Protocol.Objects;
using Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Handshake;
using System;
using System.Threading.Tasks;

namespace Net.Myzuc.PurpleStainedGlass.Extensions
{
    public sealed class ExtensionTest : Extension
    {
        public ExtensionTest()
        {
            Console.WriteLine("test initialized");
        }
        public override Task OnStatusAsync(Status status, ServerboundHandshake handshake)
        {
            status.Description = new ChatText($"ballsv3\n{handshake.OriginAddress}:{handshake.OriginPort}");
            return Task.CompletedTask;
        }
        public override async Task OnLoginAsync(LoginRequest login)
        {
            await login.DisconnectAsync(new ChatText("no gay people allowed"));
            Console.WriteLine($"{login.Name} is logging in");
        }
        ~ExtensionTest()
        {
            Console.WriteLine("test deinitialized");
        }
    }
}
