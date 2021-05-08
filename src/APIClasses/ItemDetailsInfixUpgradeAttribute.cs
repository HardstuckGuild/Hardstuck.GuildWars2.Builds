using Newtonsoft.Json;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal class ItemDetailsInfixUpgradeAttribute
    {
        [JsonProperty("attribute")]
        public string Attribute { get; set; }

        [JsonProperty("multiplier")]
        public int Modifier { get; set; }
    }
}
