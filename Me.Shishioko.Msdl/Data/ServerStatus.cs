using Me.Shishioko.Msdl.Data.Chat;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Me.Shishioko.Msdl.Data
{
    public sealed class ServerStatus
    {
        public sealed class StatusPlayerInfo
        {
            [JsonProperty("max")]
            public int? Max { get; set; }
            [JsonProperty("online")]
            public int? Online { get; set; }
            [JsonProperty("sample")]
            public IEnumerable<StatusPlayer>? Sample { get; set; }
            public StatusPlayerInfo()
            {
                Max = null;
                Online = null;
                Sample = null;
            }
        }
        public sealed class StatusPlayer
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("id")]
            public Guid Id { get; set; }
            public StatusPlayer(string name, Guid id)
            {
                Name = name;
                Id = id;
            }
        }
        public sealed class StatusVersion
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("protocol")]
            public int Protocol { get; set; }
            public StatusVersion(string name, int protocol)
            {
                Name = name;
                Protocol = protocol;
            }
        }
        [JsonProperty("version")]
        public StatusVersion? Version { get; set; }
        [JsonProperty("players")]
        public StatusPlayerInfo? Players { get; set; }
        [JsonProperty("description")]
        public ChatComponent? Description { get; set; }
        [JsonProperty("favicon")]
        public string? Favicon { get; set; }
        [JsonProperty("enforcesSecureChat")]
        public bool? EnforcesSecureChat { get; set; }
        public ServerStatus()
        {
            Version = null;
            Players = null;
            Description = null;
            Favicon = null;
            EnforcesSecureChat = null;
        }
    }
}
