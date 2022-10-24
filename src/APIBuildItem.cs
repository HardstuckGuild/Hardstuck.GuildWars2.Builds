using System;

namespace Hardstuck.GuildWars2.Builds
{
    /// <summary>
    /// Item equipped
    /// </summary>
    public class APIBuildItem
    {
        /// <summary>
        /// Id of the item
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// In which slot the item belongs to
        /// </summary>
        public string Slot { get; set; }

        /// <summary>
        /// What attributes are selected
        /// </summary>
        public AttributeType AttributeType { get; set; }

        /// <summary>
        /// What upgrades are installed
        /// </summary>
        public int?[] Upgrades { get; set; } = Array.Empty<int?>();
    }
}
