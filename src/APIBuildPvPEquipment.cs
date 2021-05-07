using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds
{
    public class APIBuildPvPEquipment : APIBuildEquipment
    {
        public List<APIBuildWeapon> Weapons { get; set; } = new List<APIBuildWeapon>();

        public int Amulet { get; set; }
        public int Rune { get; set; }
        public int[] Sigils { get; set; } = new int[4];
    }
}
