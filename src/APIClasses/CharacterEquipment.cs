using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class CharacterEquipment
    {
        [JsonPropertyName("id")]
        internal int Id { get; set; }

        [JsonPropertyName("slot")]
        internal string Slot { get; set; }

        [JsonPropertyName("stats")]
        internal CharacterEquipmentStats Stats { get; set; }

        [JsonPropertyName("upgrades")]
        internal List<int> Upgrades { get; set; }
    }
}
