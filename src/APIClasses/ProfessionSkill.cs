using Newtonsoft.Json;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal class ProfessionSkill
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("slot")]
        public string Slot { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
