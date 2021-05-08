using System;
using System.Collections.Generic;

namespace Hardstuck.GuildWars2.Builds
{
    public class APIBuild
    {
        public string CharacterName { get; set; }
        public GW2GameMode GameMode { get; set; } = GW2GameMode.PvE;
        internal Profession Profession { get; set; }
        public List<APIBuildSpecialization> Specializations { get; set; } = new List<APIBuildSpecialization>();
        public APIBuildSkills Skills { get; set; } = new APIBuildSkills();
        public List<APIBuildPet> Pets { get; set; } = new List<APIBuildPet>();
        public APIBuildEquipment Equipment { get; set; } = new APIBuildEquipment();

        public static string Letterize(int[] stuff)
        {
            string result = "";
            for (int x = 0; x < stuff.Length; x++)
                if (stuff[x] > 2652)
                {
                    int squaredQuotient = Math.DivRem(stuff[x] - 2653, 2653, out int squaredRemainder);
                    int remainderQuotient = Math.DivRem(squaredRemainder - 52, 52, out int remainderRemainder);
                    result += "_" + Letterize(new int[] { squaredQuotient, remainderQuotient, remainderRemainder });
                }
                else if (stuff[x] > 51)
                    result += "-" + Letterize(new int[] { (int)Math.Floor((stuff[x] - 52) / 52d), stuff[x] % 52 });
                else if (stuff[x] > 25)
                    result += ((char)(65 + stuff[x] - 26)).ToString();
                else
                    result += ((char)(97 + stuff[x])).ToString();

            return result;
        }

        public static int AlphaToInt(char alpha)
        {
            int result = alpha - 97;
            if (result < 0)
                result += 58;

            return result;
        }

        public static int[] Deletterize(string code)
        {
            List<int> result = new List<int>();
            char[] letters = code.ToCharArray();

            for (int x = 0; x < letters.Length; x++)
            {
                char l = letters[x];
                if (l == '_')
                {
                    int squaredQuotient = AlphaToInt(letters[x + 1]);
                    int remainderQuotient = AlphaToInt(letters[x + 2]);
                    int remainderRemainder = AlphaToInt(letters[x + 3]);
                    result.Add((squaredQuotient + 1) * 2653 + (remainderQuotient + 1) * 52 + remainderRemainder);
                    x += 3;
                }
                else if (l == '-')
                {
                    int quotient = AlphaToInt(letters[x + 1]);
                    int remainder = AlphaToInt(letters[x + 2]);
                    result.Add(quotient * 52 + remainder);
                    x += 2;
                }
                else
                {
                    result.Add(AlphaToInt(letters[x]));
                }
            }

            return result.ToArray();
        }

        public string GetBuildCode()
        {
            List<int> relativeIds = new List<int>
            {
                (int)GameMode,
                Profession.RelativeId
            };

            foreach (APIBuildSpecialization s in Specializations)
            {
                relativeIds.Add(s.RelativeId);
                foreach (APIBuildTrait t in s.Traits)
                    relativeIds.Add((int)t.Position);
            }

            foreach (APIBuildSkill s in Skills.Heals)
                relativeIds.Add(s.RelativeId);

            foreach (APIBuildSkill s in Skills.Utilities)
                relativeIds.Add(s.RelativeId);

            foreach (APIBuildSkill s in Skills.Elites)
                relativeIds.Add(s.RelativeId);

            if (Equipment.GetType() == typeof(APIBuildPvEEquipment))
            {
                APIBuildPvEEquipment PvEEquipment = Equipment as APIBuildPvEEquipment;
                AttributeType curStat = null;
                int curStatCounter = 0;


                foreach (APIBuildWeapon w in PvEEquipment.Weapons)
                {
                    if (w != null)
                    {
                        relativeIds.Add(w.RelativeId);
                    }
                }

                foreach (APIBuildItem i in PvEEquipment.AllItems)
                {
                    if (i != null)
                    {
                        if (curStat == null)
                        {
                            curStat = i.AttributeType;
                            relativeIds.Add(i.AttributeType.Id);
                        }

                        if (i.AttributeType.Name != curStat.Name)
                        {
                            if (curStatCounter > 0)
                                relativeIds.Add(curStatCounter);

                            //MightyTeabot.MainForm.Log(curStatCounter + " pieces of " + curStat.Name);

                            curStatCounter = 0;
                            relativeIds.Add(i.AttributeType.Id);
                            curStat = i.AttributeType;
                        }
                        curStatCounter++;
                    }
                }

                if (curStatCounter > 0)
                {
                    //MightyTeabot.MainForm.Log(curStatCounter + " pieces of " + curStat.Name);
                    relativeIds.Add(curStatCounter);
                }

                int curRune = -1;
                int curRuneCounter = 0;

                foreach (int r in PvEEquipment.Runes)
                {
                    if (curRune == -1)
                    {
                        curRune = r;
                        relativeIds.Add(r);
                    }

                    if (r != curRune)
                    {
                        if (curRuneCounter > 0)
                            relativeIds.Add(curRuneCounter);

                        curRuneCounter = 0;
                        relativeIds.Add(r);
                        curRune = r;
                    }
                    curRuneCounter++;
                }

                if (curRuneCounter > 0)
                    relativeIds.Add(curRuneCounter);

                relativeIds.AddRange(PvEEquipment.Sigils);
            }
            else
            {
                APIBuildPvPEquipment PvPEquipment = Equipment as APIBuildPvPEquipment;

                foreach (APIBuildWeapon w in PvPEquipment.Weapons)
                {
                    if (w != null)
                    {
                        relativeIds.Add(w.RelativeId);
                    }
                }

                relativeIds.Add(PvPEquipment.Amulet);
                relativeIds.Add(PvPEquipment.Rune);
                for (int x = 0; x < PvPEquipment.Sigils.Length; x++)
                {
                    relativeIds.Add(PvPEquipment.Sigils[x]);
                }
            }

            if (Profession.Name == "Ranger")
            {
                for (int x = 0; x < Pets.Count; x++)
                {
                    if (Pets[x] != null)
                        relativeIds.Add(Pets[x].Id);
                }
            }

            return Letterize(relativeIds.ToArray());
        }
    }
}
