using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class CharacterSkills
    {
        [JsonProperty("heal")]
        internal int Heal { get; set; }

        [JsonProperty("utilities")]
        internal List<int?> Utilities { get; set; }

        [JsonProperty("elite")]
        internal int? Elite { get; set; }

        [JsonProperty("pets")]
        internal Dictionary<string, List<int>> Pets { get; set; }

        [JsonProperty("legends")]
        internal List<string> Legends { get; set; }
    }
}
