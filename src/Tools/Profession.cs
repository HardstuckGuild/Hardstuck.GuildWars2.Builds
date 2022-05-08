using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.Tools
{
    internal class Profession
    {
        public int RelativeId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("icon_big")]
        public string IconBig { get; set; }

        [JsonProperty("specializations")]
        internal List<int> Specialisations { get; set; }

        [JsonProperty("weapons")]
        internal JObject Weapons { get; set; }

        [JsonProperty("skills")]
        internal List<APIClasses.ProfessionSkill> Skills { get; set; }
    }
}
