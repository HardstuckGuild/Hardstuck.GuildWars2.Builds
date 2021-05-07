namespace Hardstuck.GuildWars2.Builds
{
    public class APIBuildWeapon : APIBuildItem
    {
        public int RelativeId { get; set; }
        public int[] Skills { get; set; }
        public WeaponType Type { get; set; }
    }
}
