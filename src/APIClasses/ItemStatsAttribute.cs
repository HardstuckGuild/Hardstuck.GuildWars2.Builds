using Newtonsoft.Json;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class ItemStatsAttribute
    {
        [JsonProperty("attribute")]
        internal string Attribute { get; set; }

        [JsonProperty("multiplier")]
        internal float Multiplier { get; set; }

        [JsonProperty("value")]
        internal int? Value { get; set; }
    }
}
