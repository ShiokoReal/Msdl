using Net.Myzuc.PurpleStainedGlass.Protocol.Objects;
using Net.Myzuc.PurpleStainedGlass.Protocol.Packets.Handshake;
using System.Threading.Tasks;

namespace Net.Myzuc.PurpleStainedGlass
{
    public abstract class Extension
    {
        public Extension()
        {

        }
        public virtual Task OnRegisterAsync()
        {
            return Task.CompletedTask;
        }
        public virtual Task OnStatusAsync(Status status, ServerboundHandshake handshake)
        {
            return Task.CompletedTask;
        }
        public virtual Task OnLoginAsync(LoginRequest login)
        {
            return Task.CompletedTask;
        }
    }
}
