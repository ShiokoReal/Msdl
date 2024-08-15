using Net.Myzuc.PurpleStainedGlass.Protocol.Objects.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Net.Myzuc.PurpleStainedGlass.Protocol.Objects
{
    public sealed class Status
    {
        public sealed class StatusPlayerInfo
        {
            public int? Max { get; init; }
            public int? Online { get; init; }
            private readonly (Guid id, string name)[]? InternalSamples = null;
            public (Guid id, string name)[]? Samples
            {
                get => [.. InternalSamples];
                init => InternalSamples = value is not null ? [.. value] : null;
            }
            public StatusPlayerInfo()
            {
                Max = null;
                Online = null;
                Samples = null;
            }
        }
        public (string name, int protocol)? Version { get; init; } = null;
        public StatusPlayerInfo? Players { get; init; } = null;
        public ChatComponent? Description { get; init; } = null;
        public string? Favicon { get; init; } = null;
        public bool? EnforcesSecureChat { get; init; } = null;
        public Status()
        {

        }
        internal string TextSerialize()
        {
            StringBuilder json = new();
            json.Append('{');
            bool empty = true;
            if (Version.HasValue)
            {
                json.Append("\"version\":{\"name\":\"");
                json.Append(HttpUtility.JavaScriptStringEncode(Version.Value.name, false));
                json.Append("\",\"protocol\":");
                json.Append(Version.Value.protocol);
                json.Append("},");
                empty = false;
            }
            if (Players is not null)
            {
                json.Append("\"players\":{");
                if (Players.Max.HasValue)
                {
                    json.Append("\"max\":");
                    json.Append(Players.Max.Value);
                    json.Append(',');
                }
                if (Players.Online.HasValue)
                {
                    json.Append("\"online\":");
                    json.Append(Players.Online.Value);
                    json.Append(',');
                }
                if (Players.Samples is not null)
                {
                    json.Append("\"sample\":[");
                    foreach ((Guid id, string name) player in Players.Samples)
                    {
                        json.Append("{\"name\":\"");
                        json.Append(HttpUtility.JavaScriptStringEncode(player.name, false));
                        json.Append("\",\"id\":\"");
                        json.Append(player.id.ToString());
                        json.Append("\"}");
                    }
                    json.Append("],");
                }
                json.Append("},");
                empty = false;
            }
            if (Description is not null)
            {
                json.Append("\"description\":");
                json.Append(Description.JsonSerialize());
                json.Append(',');
                empty = false;
            }
            if (Favicon is not null)
            {
                json.Append("\"favicon\":");
                json.Append(HttpUtility.JavaScriptStringEncode(Favicon, false));
                json.Append("\",");
                empty = false;
            }
            if (EnforcesSecureChat.HasValue)
            {
                json.Append("\"enforcesSecureChat\":");
                json.Append(EnforcesSecureChat.Value ? "true" : "false");
                json.Append(',');
                empty = false;
            }
            if (!empty) json.Remove(json.Length - 1, 1);
            json.Append('}');
            return json.ToString();
        }
    }
}
