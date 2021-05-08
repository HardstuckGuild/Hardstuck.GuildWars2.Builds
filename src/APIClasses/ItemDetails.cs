using Newtonsoft.Json;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal class ItemDetails
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("infix_upgrade")]
        public ItemDetailsInfixUpgrade InfixUpgrade { get; set; }
    }
}
