using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class TokenInfo
    {
        [JsonPropertyName("id")]
        internal string Id { get; set; }

        [JsonPropertyName("name")]
        internal string Name { get; set; }

        [JsonPropertyName("permissions")]
        internal List<string> Permissions { get; set; }
    }
}
