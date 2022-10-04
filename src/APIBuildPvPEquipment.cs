using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds
{
    /// <summary>
    /// PvP build equipment
    /// </summary>
    public sealed class APIBuildPvPEquipment : APIBuildEquipment
    {
        /// <summary>
        /// List of equipped weapons
        /// </summary>
        public List<APIBuildWeapon> Weapons { get; set; } = new List<APIBuildWeapon>();

        /// <summary>
        /// Equipped amulet
        /// </summary>
        public int Amulet { get; set; }

        /// <summary>
        /// Equipped rune
        /// </summary>
        public int Rune { get; set; }

        /// <summary>
        /// Equipped sigils
        /// </summary>
        public int[] Sigils { get; set; } = new int[4];
    }
}
