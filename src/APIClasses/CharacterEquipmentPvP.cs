using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class CharacterEquipmentPvP
    {
        [JsonProperty("amulet")]
        internal int? Amulet { get; set; }

        [JsonProperty("rune")]
        internal int? Rune { get; set; }

        [JsonProperty("sigils")]
        internal List<int?> Sigils { get; set; }
    }
}
