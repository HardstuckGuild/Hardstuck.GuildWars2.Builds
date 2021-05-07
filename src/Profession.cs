using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hardstuck.GuildWars2.Builds
{
    public class Profession
    {
        public int relativeId;
        public string name;
        public string icon;
        public string icon_big;
        public int[] specializations;
        public JObject weapons;
        public JArray skills;
    }
}
