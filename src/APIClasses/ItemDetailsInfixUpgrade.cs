using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class ItemDetailsInfixUpgrade
    {
        [JsonPropertyName("id")]
        internal int Id { get; set; }

        [JsonPropertyName("attributes")]
        internal List<ItemDetailsInfixUpgradeAttribute> Attributes { get; set; }
    }
}
