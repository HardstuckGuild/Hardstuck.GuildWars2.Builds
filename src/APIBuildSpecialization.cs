using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds
{
    /// <summary>
    /// Build specialization
    /// </summary>
    public class APIBuildSpecialization
    {
        /// <summary>
        /// Id of the specialization
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Relative Id of the specialization
        /// </summary>
        public int RelativeId { get; set; }

        /// <summary>
        /// List of traits
        /// </summary>
        public List<APIBuildTrait> Traits { get; set; } = new List<APIBuildTrait>();
    }
}
