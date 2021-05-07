using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds
{
    public class APIBuildSpecialization
    {
        public int Id { get; set; }
        public int RelativeId { get; set; }
        public List<APIBuildTrait> Traits { get; set; } = new List<APIBuildTrait>();
    }
}
