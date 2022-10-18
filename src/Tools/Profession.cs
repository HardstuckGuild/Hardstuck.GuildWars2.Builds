using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hardstuck.GuildWars2.Builds.Tools
{
    internal sealed class Profession
    {
        internal int RelativeId { get; set; }

        [JsonPropertyName("name")]
        internal string Name { get; set; }

        [JsonPropertyName("icon")]
        internal string Icon { get; set; }

        [JsonPropertyName("icon_big")]
        internal string IconBig { get; set; }

        [JsonPropertyName("specializations")]
        internal List<int> Specialisations { get; set; }

        [JsonPropertyName("weapons")]
        internal Dictionary<string, object> Weapons { get; set; }

        [JsonPropertyName("skills")]
        internal List<APIClasses.ProfessionSkill> Skills { get; set; }
    }
}
