using Me.Shishioko.Msdl.Data.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Me.Shishioko.Msdl.Data
{
    public sealed class ServerStatus
    {
        public sealed class StatusPlayerInfo
        {
            public int? Max;
            public int? Online;
            public IEnumerable<StatusPlayer>? Samples;
            public StatusPlayerInfo()
            {
                Max = null;
                Online = null;
                Samples = null;
            }
        }
        public sealed class StatusPlayer
        {
            public string Name;
            public Guid Id;
            public StatusPlayer(string name, Guid id)
            {
                Name = name;
                Id = id;
            }
        }
        public sealed class StatusVersion
        {
            public string Name;
            public int Protocol;
            public StatusVersion(string name, int protocol)
            {
                Name = name;
                Protocol = protocol;
            }
        }
        public StatusVersion? Version;
        public StatusPlayerInfo? Players;
        public ChatComponent? Description;
        public string? Favicon;
        public bool? EnforcesSecureChat;
        public ServerStatus()
        {
            Version = null;
            Players = null;
            Description = null;
            Favicon = null;
            EnforcesSecureChat = null;
        }
        internal string TextSerialize()
        {
            StringBuilder json = new();
            json.Append('{');
            bool empty = true;
            if (Version is not null)
            {
                json.Append("\"version\":{\"name\":\"");
                json.Append(HttpUtility.JavaScriptStringEncode(Version.Name, false));
                json.Append("\",\"protocol\":");
                json.Append(Version.Protocol);
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
                    foreach(StatusPlayer player in Players.Samples)
                    {
                        json.Append("{\"name\":\"");
                        json.Append(HttpUtility.JavaScriptStringEncode(player.Name, false));
                        json.Append("\",\"id\":\"");
                        json.Append(player.Id.ToString());
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
                json.Append(Description.TextSerialize());
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
