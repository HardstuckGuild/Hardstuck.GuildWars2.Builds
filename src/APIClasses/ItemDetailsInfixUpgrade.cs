using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal class ItemDetailsInfixUpgrade
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("attributes")]
        public List<ItemDetailsInfixUpgradeAttribute> Attributes { get; set; }
    }
}
