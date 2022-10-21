using Newtonsoft.Json;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class Item
    {
        [JsonProperty("id")]
        internal int Id { get; set; }

        [JsonProperty("details")]
        internal ItemDetails Details { get; set; }
    }
}
