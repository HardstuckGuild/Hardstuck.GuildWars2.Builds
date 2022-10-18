using System.Text.Json.Serialization;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class Trait
    {
        [JsonPropertyName("id")]
        internal int Id { get; set; }

        [JsonPropertyName("order")]
        internal int Order { get; set; }
    }
}
