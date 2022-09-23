using Newtonsoft.Json;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class ItemDetailsInfixUpgradeAttribute
    {
        [JsonProperty("attribute")]
        internal string Attribute { get; set; }

        [JsonProperty("multiplier")]
        internal int Modifier { get; set; }
    }
}
