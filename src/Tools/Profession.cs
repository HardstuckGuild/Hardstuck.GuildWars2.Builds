using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.Tools
{
    internal sealed class Profession
    {
        internal int RelativeId { get; set; }

        [JsonProperty("name")]
        internal string Name { get; set; }

        [JsonProperty("icon")]
        internal string Icon { get; set; }

        [JsonProperty("icon_big")]
        internal string IconBig { get; set; }

        [JsonProperty("specializations")]
        internal List<int> Specialisations { get; set; }

        [JsonProperty("weapons")]
        internal JObject Weapons { get; set; }

        [JsonProperty("skills")]
        internal List<APIClasses.ProfessionSkill> Skills { get; set; }
    }
}
