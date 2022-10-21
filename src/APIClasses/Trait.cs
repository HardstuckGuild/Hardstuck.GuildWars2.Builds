using Newtonsoft.Json;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class Trait
    {
        [JsonProperty("id")]
        internal int Id { get; set; }

        [JsonProperty("order")]
        internal int Order { get; set; }
    }
}
