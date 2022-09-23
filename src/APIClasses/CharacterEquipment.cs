using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class CharacterEquipment
    {
        [JsonProperty("id")]
        internal int Id { get; set; }

        [JsonProperty("slot")]
        internal string Slot { get; set; }

        [JsonProperty("stats")]
        internal CharacterEquipmentStats Stats { get; set; }

        [JsonProperty("upgrades")]
        internal List<int> Upgrades { get; set; }
    }
}
