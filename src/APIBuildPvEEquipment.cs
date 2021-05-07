using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds
{
    public class APIBuildPvEEquipment : APIBuildEquipment
    {
        public List<APIBuildWeapon> Weapons { get; set; } = new List<APIBuildWeapon>();
        public List<APIBuildItem> Armor { get; set; } = new List<APIBuildItem>() { null, null, null, null, null, null };
        public List<APIBuildItem> Trinkets { get; set; } = new List<APIBuildItem>() { null, null, null, null, null, null };
        public List<int> Runes { get; set; } = new List<int>();
        public List<int> Sigils { get; set; } = new List<int>();
        public List<APIBuildItem> AllItems
        {
            get
            {
                List<APIBuildItem> allItems = new List<APIBuildItem>();
                allItems.AddRange(Weapons);
                allItems.AddRange(Armor);
                allItems.AddRange(Trinkets);
                return allItems;
            }
        }
    }
}
