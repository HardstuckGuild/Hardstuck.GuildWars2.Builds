using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds
{
    /// <summary>
    /// PvE build equipment
    /// </summary>
    public class APIBuildPvEEquipment : APIBuildEquipment
    {
        /// <summary>
        /// List of equipped weapons
        /// </summary>
        public List<APIBuildWeapon> Weapons { get; set; } = new List<APIBuildWeapon>();

        /// <summary>
        /// List of equipped armor
        /// </summary>
        public List<APIBuildItem> Armor { get; set; } = new List<APIBuildItem>() { null, null, null, null, null, null };

        /// <summary>
        /// List of equipped trinkets
        /// </summary>
        public List<APIBuildItem> Trinkets { get; set; } = new List<APIBuildItem>() { null, null, null, null, null, null };

        /// <summary>
        /// List of equipped runes
        /// </summary>
        public List<int> Runes { get; set; } = new List<int>();

        /// <summary>
        /// List of equipped sigils
        /// </summary>
        public List<int> Sigils { get; set; } = new List<int>();

        /// <summary>
        /// List of all items currently equipped
        /// </summary>
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
