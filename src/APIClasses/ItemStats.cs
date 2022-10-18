using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class ItemStats
    {
        [JsonPropertyName("id")]
        internal int Id { get; set; }

        [JsonPropertyName("name")]
        internal string Name { get; set; }

        [JsonPropertyName("attributes")]
        internal List<ItemStatsAttribute> Attributes { get; set; }
    }
}
