using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal class CharacterEquipment
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("slot")]
        public string Slot { get; set; }

        [JsonProperty("stats")]
        public CharacterEquipmentStats Stats { get; set; }

        [JsonProperty("upgrades")]
        public List<int> Upgrades { get; set; }
    }
}
