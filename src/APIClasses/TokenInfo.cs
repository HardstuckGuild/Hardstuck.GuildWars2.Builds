using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class TokenInfo
    {
        [JsonProperty("id")]
        internal string Id { get; set; }

        [JsonProperty("name")]
        internal string Name { get; set; }

        [JsonProperty("permissions")]
        internal List<string> Permissions { get; set; }
    }
}
