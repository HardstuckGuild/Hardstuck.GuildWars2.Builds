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

        /// <summary>
        /// How many slots does the weapon take
        /// </summary>
        public WeaponTypeSlotCount SlotCount
        {
            get
            {
                switch (Type)
                {
                    case WeaponType.Axe:
                    case WeaponType.Dagger:
                    case WeaponType.Focus:
                    case WeaponType.Mace:
                    case WeaponType.Pistol:
                    case WeaponType.Scepter:
                    case WeaponType.Shield:
                    case WeaponType.Sword:
                    case WeaponType.Torch:
                    case WeaponType.Warhorn:
                        return WeaponTypeSlotCount.OneHanded;
                    case WeaponType.Greatsword:
                    case WeaponType.Hammer:
                    case WeaponType.Harpoongun:
                    case WeaponType.Longbow:
                    case WeaponType.Rifle:
                    case WeaponType.Shortbow:
                    case WeaponType.Spear:
                    case WeaponType.Staff:
                    case WeaponType.Trident:
                        return WeaponTypeSlotCount.TwoHanded;
                    default:
                        return WeaponTypeSlotCount.Unrecognised;
                }
            }
        }
    }
}
