using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal class Character
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("profession")]
        public string Profession { get; set; }

        [JsonProperty("equipment")]
        public List<CharacterEquipment> Equipment { get; set; }

        [JsonProperty("skills")]
        public Dictionary<string, CharacterSkills> Skills { get; set; }

        [JsonProperty("equipment_pvp")]
        public CharacterEquipmentPvP EquipmentPvP { get; set; }

        [JsonProperty("specializations")]
        public Dictionary<string, List<CharacterSpecialisation>> Specialisations { get; set; }
    }
}
