using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class CharacterEquipmentStats
    {
        [JsonProperty("id")]
        internal int Id { get; set; }

        [JsonProperty("name")]
        internal string Name { get; set; }

        [JsonProperty("attributes")]
        internal Dictionary<string, int> Attributes { get; set; }
    }
}
