using Newtonsoft.Json;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class ItemDetails
    {
        [JsonProperty("type")]
        internal string Type { get; set; }

        [JsonProperty("infix_upgrade")]
        internal ItemDetailsInfixUpgrade InfixUpgrade { get; set; }
    }
}
