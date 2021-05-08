using Newtonsoft.Json;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal class Item
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("details")]
        public ItemDetails Details { get; set; }
    }
}
