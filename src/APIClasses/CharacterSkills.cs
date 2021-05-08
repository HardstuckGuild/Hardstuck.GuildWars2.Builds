using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal class CharacterSkills
    {
        [JsonProperty("heal")]
        public int Heal { get; set; }

        [JsonProperty("utilities")]
        public List<int?> Utilities { get; set; }

        [JsonProperty("elite")]
        public int Elite { get; set; }

        [JsonProperty("pets")]
        public Dictionary<string, List<int>> Pets { get; set; }

        [JsonProperty("legends")]
        public List<string> Legends { get; set; }
    }
}
