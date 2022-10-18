using System.Text.Json.Serialization;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class ProfessionSkill
    {
        [JsonPropertyName("id")]
        internal int Id { get; set; }

        [JsonPropertyName("slot")]
        internal string Slot { get; set; }

        [JsonPropertyName("type")]
        internal string Type { get; set; }
    }
}
