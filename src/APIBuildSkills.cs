using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds
{
    public class APIBuildSkills
    {
        public List<APIBuildSkill> Heals { get; set; } = new List<APIBuildSkill>();
        public List<APIBuildSkill> Utilities { get; set; } = new List<APIBuildSkill>();
        public List<APIBuildSkill> Elites { get; set; } = new List<APIBuildSkill>();
        public List<APIBuildSkill> ProfessionMechanics { get; set; } = new List<APIBuildSkill>();
    }
}
