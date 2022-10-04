using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds
{
    /// <summary>
    /// Used skills in the build
    /// </summary>
    public sealed class APIBuildSkills
    {
        /// <summary>
        /// List of heal skills
        /// </summary>
        public List<APIBuildSkill> Heals { get; set; } = new List<APIBuildSkill>();

        /// <summary>
        /// List of utilities
        /// </summary>
        public List<APIBuildSkill> Utilities { get; set; } = new List<APIBuildSkill>();

        /// <summary>
        /// List of elites
        /// </summary>
        public List<APIBuildSkill> Elites { get; set; } = new List<APIBuildSkill>();

        /// <summary>
        /// List of Profession mechanics
        /// </summary>
        public List<APIBuildSkill> ProfessionMechanics { get; set; } = new List<APIBuildSkill>();
    }
}
