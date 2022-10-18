using System.Text.Json.Serialization;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class ItemDetails
    {
        [JsonPropertyName("type")]
        internal string Type { get; set; }

        [JsonPropertyName("infix_upgrade")]
        internal ItemDetailsInfixUpgrade InfixUpgrade { get; set; }
    }
}
