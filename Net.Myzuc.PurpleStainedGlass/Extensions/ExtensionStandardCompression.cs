using System.IO.Compression;
using System.Threading.Tasks;

namespace Net.Myzuc.PurpleStainedGlass.Extensions
{
    public sealed class ExtensionStandardCompression : Extension
    {
        public override Task OnLoginAsync(LoginRequest login)
        {
            return login.SetCompressionAsync(256, CompressionLevel.Optimal);
        }
    }
}
