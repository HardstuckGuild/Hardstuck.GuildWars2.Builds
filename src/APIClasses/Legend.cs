using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class Legend
    {
        [JsonProperty("id")]
        internal string Id { get; set; }

        [JsonProperty("heal")]
        internal int Heal { get; set; }

        [JsonProperty("elite")]
        internal int Elite { get; set; }

        [JsonProperty("utilities")]
        internal List<int> Utilities { get; set; }
    }
}
