using Newtonsoft.Json;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class ProfessionSkill
    {
        [JsonProperty("id")]
        internal int Id { get; set; }

        [JsonProperty("slot")]
        internal string Slot { get; set; }

        [JsonProperty("type")]
        internal string Type { get; set; }
    }
}
