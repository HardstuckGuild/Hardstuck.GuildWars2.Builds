using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal class Legend
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("heal")]
        public int Heal { get; set; }

        [JsonProperty("elite")]
        public int Elite { get; set; }

        [JsonProperty("utilities")]
        public List<int> Utilities { get; set; }
    }
}
