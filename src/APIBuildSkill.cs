namespace Hardstuck.GuildWars2.Builds
{
    /// <summary>
    /// A build skill
    /// </summary>
    public sealed class APIBuildSkill
    {
        /// <summary>
        /// Id of the skill
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Relative Id of the skill
        /// </summary>
        public int RelativeId { get; set; }

        /// <summary>
        /// Id from the palette
        /// </summary>
        public int PaletteId { get; set; }

        /// <summary>
        /// Name of the skill
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of the skill
        /// </summary>
        public string Type { get; set; }
    }
}
