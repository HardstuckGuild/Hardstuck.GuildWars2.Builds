using Newtonsoft.Json;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal class Trait
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }
    }
}
