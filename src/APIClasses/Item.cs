using System.Text.Json.Serialization;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class Item
    {
        [JsonPropertyName("id")]
        internal int Id { get; set; }

        [JsonPropertyName("details")]
        internal ItemDetails Details { get; set; }
    }
}
