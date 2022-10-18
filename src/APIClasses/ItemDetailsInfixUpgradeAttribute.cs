using System.Text.Json.Serialization;

namespace Hardstuck.GuildWars2.Builds.APIClasses
{
    internal sealed class ItemDetailsInfixUpgradeAttribute
    {
        [JsonPropertyName("attribute")]
        internal string Attribute { get; set; }

        [JsonPropertyName("multiplier")]
        internal int Modifier { get; set; }
    }
}
