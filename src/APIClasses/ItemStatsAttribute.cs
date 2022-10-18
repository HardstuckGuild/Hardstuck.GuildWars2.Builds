using System.Text.Json.Serialization;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class ItemStatsAttribute
    {
        [JsonPropertyName("attribute")]
        internal string Attribute { get; set; }

        [JsonPropertyName("multiplier")]
        internal float Multiplier { get; set; }

        [JsonPropertyName("value")]
        internal int? Value { get; set; }
    }
}
