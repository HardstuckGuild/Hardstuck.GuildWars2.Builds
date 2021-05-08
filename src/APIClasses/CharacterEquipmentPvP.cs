using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal class CharacterEquipmentPvP
    {
        [JsonProperty("amulet")]
        public int? Amulet { get; set; }

        [JsonProperty("rune")]
        public int? Rune { get; set; }

        [JsonProperty("sigils")]
        public List<int?> Sigils { get; set; }
    }
}
