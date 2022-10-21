using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class Character
    {
        [JsonProperty("name")]
        internal string Name { get; set; }

        [JsonProperty("profession")]
        internal string Profession { get; set; }

        [JsonProperty("equipment")]
        internal List<CharacterEquipment> Equipment { get; set; }

        [JsonProperty("skills")]
        internal Dictionary<string, CharacterSkills> Skills { get; set; }

        [JsonProperty("equipment_pvp")]
        internal CharacterEquipmentPvP EquipmentPvP { get; set; }

        [JsonProperty("specializations")]
        internal Dictionary<string, List<CharacterSpecialisation>> Specialisations { get; set; }
    }
}
