using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class CharacterSpecialisation
    {
        [JsonProperty("id")]
        internal int Id { get; set; }

        [JsonProperty("traits")]
        internal List<int?> Traits { get; set; }
    }
}
