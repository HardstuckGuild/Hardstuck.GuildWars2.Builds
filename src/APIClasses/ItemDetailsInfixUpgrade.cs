using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class ItemDetailsInfixUpgrade
    {
        [JsonProperty("id")]
        internal int Id { get; set; }

        [JsonProperty("attributes")]
        internal List<ItemDetailsInfixUpgradeAttribute> Attributes { get; set; }
    }
}
