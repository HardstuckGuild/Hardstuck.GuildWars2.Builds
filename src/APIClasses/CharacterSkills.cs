using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class CharacterSkills
    {
        [JsonPropertyName("heal")]
        internal int Heal { get; set; }

        [JsonPropertyName("utilities")]
        internal List<int?> Utilities { get; set; }

        [JsonPropertyName("elite")]
        internal int? Elite { get; set; }

        [JsonPropertyName("pets")]
        internal Dictionary<string, List<int>> Pets { get; set; }

        [JsonPropertyName("legends")]
        internal List<string> Legends { get; set; }
    }
}
