using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class CharacterSpecialisation
    {
        [JsonPropertyName("id")]
        internal int Id { get; set; }

        [JsonPropertyName("traits")]
        internal List<int?> Traits { get; set; }
    }
}
