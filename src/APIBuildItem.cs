namespace Hardstuck.GuildWars2.Builds
{
    public class APIBuildItem
    {
        public int Id { get; set; }
        public string Slot { get; set; }
        public AttributeType AttributeType { get; set; }
        public int[] Upgrades { get; set; } = new int[0];
    }
}
