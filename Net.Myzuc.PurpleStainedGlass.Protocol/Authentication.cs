using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Numerics;
using System.Text.Json;
using System.Text;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Net.Myzuc.PurpleStainedGlass.Protocol
{
    public static class Authentication
    {
        public static async Task<(Guid guid, string name, (string name, string value, string? signature)[] properties)?> AuthenticateAsync(string name, string server, byte[] secret, RSA rsa)
        {
            BigInteger number = new(SHA1.HashData(Encoding.ASCII.GetBytes(server).Concat(secret).Concat(rsa.ExportSubjectPublicKeyInfo()).ToArray()).Reverse().ToArray());
            string url = $"https://sessionserver.mojang.com/session/minecraft/hasJoined?username={name}&serverId={(number < 0 ? "-" + (-number).ToString("x") : number.ToString("x"))}";
            using HttpClient http = new();
            using HttpRequestMessage request = new(HttpMethod.Get, url);
            using HttpResponseMessage response = await http.SendAsync(request);
            if (response.StatusCode != HttpStatusCode.OK) throw new UnauthorizedAccessException();
            using Stream stream = await response.Content.ReadAsStreamAsync();
            using JsonDocument auth = JsonDocument.Parse(stream);
            string realName = auth.RootElement.GetProperty("name").GetString()!;
            Guid realGuid = Guid.ParseExact(auth.RootElement.GetProperty("id").GetString()!, "N");
            (string name, string value, string? signature)[] properties = auth.RootElement.GetProperty("properties")!.EnumerateArray().Select(property => (property.GetProperty("name").GetString()!, property.GetProperty("value").GetString()!, property.GetProperty("signature").GetString())).ToArray();
            return (realGuid, realName, properties);
        }
    }
}
