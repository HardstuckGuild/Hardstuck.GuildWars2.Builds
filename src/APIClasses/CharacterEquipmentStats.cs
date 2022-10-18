using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class CharacterEquipmentStats
    {
        [JsonPropertyName("id")]
        internal int Id { get; set; }

        [JsonPropertyName("name")]
        internal string Name { get; set; }

        [JsonPropertyName("attributes")]
        internal Dictionary<string, int> Attributes { get; set; }
    }
}
