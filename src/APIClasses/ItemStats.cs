using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class ItemStats
    {
        [JsonProperty("id")]
        internal int Id { get; set; }

        [JsonProperty("name")]
        internal string Name { get; set; }

        [JsonProperty("attributes")]
        internal List<ItemStatsAttribute> Attributes { get; set; }
    }
}
