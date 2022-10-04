using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds
{
    /// <summary>
    /// Build specialisation
    /// </summary>
    public sealed class APIBuildSpecialisation
    {
        /// <summary>
        /// Id of the specialisation
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Relative Id of the specialisation
        /// </summary>
        public int RelativeId { get; set; }

        /// <summary>
        /// List of traits
        /// </summary>
        public List<APIBuildTrait> Traits { get; set; } = new List<APIBuildTrait>();
    }
}
