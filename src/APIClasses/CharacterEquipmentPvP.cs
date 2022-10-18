using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class CharacterEquipmentPvP
    {
        [JsonPropertyName("amulet")]
        internal int? Amulet { get; set; }

        [JsonPropertyName("rune")]
        internal int? Rune { get; set; }

        [JsonPropertyName("sigils")]
        internal List<int?> Sigils { get; set; }
    }
}
