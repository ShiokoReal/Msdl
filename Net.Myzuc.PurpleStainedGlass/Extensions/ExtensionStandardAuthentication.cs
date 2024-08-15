using Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Handshake;
using System.Threading.Tasks;

namespace Net.Myzuc.PurpleStainedGlass.Extensions
{
    public sealed class ExtensionStandardAuthentication : Extension
    {
        public override async Task OnLoginAsync(LoginRequest login)
        {
            await login.EncryptAsync(string.Empty, true);
            Logs.Info($"{login.Guid} \"{login.Name}\" has authenticated.");
        }
    }
}

