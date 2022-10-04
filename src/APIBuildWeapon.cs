namespace Hardstuck.GuildWars2.Builds
{
    /// <summary>
    /// A build weapon
    /// </summary>
    public sealed class APIBuildWeapon : APIBuildItem
    {
        /// <summary>
        /// Relative id of the weapon
        /// </summary>
        public int RelativeId { get; set; }

        /// <summary>
        /// Weapon skills
        /// </summary>
        public int[] Skills { get; set; }

        /// <summary>
        /// Type of the weapon
        /// </summary>
        public WeaponType Type { get; set; }
    }
}
