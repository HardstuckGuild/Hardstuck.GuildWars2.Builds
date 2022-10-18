using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class Character
    {
        [JsonPropertyName("name")]
        internal string Name { get; set; }

        [JsonPropertyName("profession")]
        internal string Profession { get; set; }

        [JsonPropertyName("equipment")]
        internal List<CharacterEquipment> Equipment { get; set; }

        [JsonPropertyName("skills")]
        internal Dictionary<string, CharacterSkills> Skills { get; set; }

        [JsonPropertyName("equipment_pvp")]
        internal CharacterEquipmentPvP EquipmentPvP { get; set; }

        [JsonPropertyName("specializations")]
        internal Dictionary<string, List<CharacterSpecialisation>> Specialisations { get; set; }
    }
}
