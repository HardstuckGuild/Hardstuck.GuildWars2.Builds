using System;
using System.Collections.Generic;
using System.Linq;

namespace Hardstuck.GuildWars2.Builds
{
    public class APIBuildRevenantLegend
    {
        public int Id { get; private set; }
        public int RelativeId { get; private set; }
        public string Name { get; private set; }

        //Water / Legend2        -> Shiro
        //Fire  / Legend1        -> Glint
        //Earth / Legend4        -> Mallyx
        //Air   / Legend3        -> Jalis
        //Deathshroud / Legend6  -> Ventari
        //null        / Legend5  -> Kalla (lol) rev

        readonly static Dictionary<object, Tuple<string, int>> LegendDictionary = new Dictionary<object, Tuple<string, int>>()
        {
            { "Fire"       , new Tuple<string, int>("Legend1", 1) },
            { "Water"      , new Tuple<string, int>("Legend2", 2) },
            { "Air"        , new Tuple<string, int>("Legend3", 3) },
            { "Earth"      , new Tuple<string, int>("Legend4", 4) },
            { "Kalla"      , new Tuple<string, int>("Legend5", 5) },
            { "Deathshroud", new Tuple<string, int>("Legend6", 6) }
        };

        public static APIBuildRevenantLegend Parse(string input)
        {
            if (input == null)
                input = "Kalla";

            if (int.TryParse(input, out int legendId))
            {
                KeyValuePair<object, Tuple<string, int>> pair = LegendDictionary.FirstOrDefault(p => p.Value.Item1 == "Legend" + legendId.ToString());

                if (!pair.Equals(default(KeyValuePair<string, Tuple<string, int>>)))
                {
                    Tuple<string, int> legend = pair.Value;
                    return new APIBuildRevenantLegend()
                    {
                        Name = legend.Item1,
                        Id = legend.Item2,
                        RelativeId = legend.Item2
                    };
                }
            }
            else if (LegendDictionary.TryGetValue(input, out Tuple<string, int> legend))
            {
                return new APIBuildRevenantLegend()
                {
                    Name = legend.Item1,
                    Id = legend.Item2,
                    RelativeId = legend.Item2
                };
            }

            return null;
        }

        public APIBuildRevenantLegend()
        {

        }
    }
}
