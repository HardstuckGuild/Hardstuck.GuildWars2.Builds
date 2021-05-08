using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds
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
        public List<int> Specializations { get; set; }

        [JsonProperty("weapons")]
        internal JObject Weapons { get; set; }

        [JsonProperty("skills")]
        internal List<APIClasses.ProfessionSkill> Skills { get; set; }
    }
}
