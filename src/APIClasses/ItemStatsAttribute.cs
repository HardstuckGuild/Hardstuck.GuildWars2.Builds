using Newtonsoft.Json;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal class ItemStatsAttribute
    {
        [JsonProperty("attribute")]
        public string Attribute { get; set; }

        [JsonProperty("multiplier")]
        public float Multiplier { get; set; }

        [JsonProperty("value")]
        public int? Value { get; set; }
    }
}
