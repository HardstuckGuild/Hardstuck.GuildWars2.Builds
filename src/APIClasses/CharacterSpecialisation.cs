using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal class CharacterSpecialisation
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("traits")]
        public List<int?> Traits { get; set; }
    }
}
