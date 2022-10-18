using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class Legend
    {
        [JsonPropertyName("id")]
        internal string Id { get; set; }

        [JsonPropertyName("heal")]
        internal int Heal { get; set; }

        [JsonPropertyName("elite")]
        internal int Elite { get; set; }

        [JsonPropertyName("utilities")]
        internal List<int> Utilities { get; set; }
    }
}
