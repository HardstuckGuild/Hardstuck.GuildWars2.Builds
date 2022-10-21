﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hardstuck.GuildWars2.Builds
{
    /// <summary>
    /// A class representing the build gathered from the API
    /// </summary>
    public sealed class APIBuild
    {
        /// <summary>
        /// Version of the code
        /// </summary>
        public static int Version => 2;

        /// <summary>
        /// Char representation of the version, A = 1, B = 2, C = 3, etc.
        /// </summary>
        public static char VersionChar => (char)(Version - 1);

        /// <summary>
        /// Name of the character
        /// </summary>
        public string CharacterName { get; set; }

        /// <summary>
        /// Game mode of the build
        /// </summary>
        public GameMode GameMode { get; set; }

        /// <summary>
        /// Profession name
        /// </summary>
        public string Profession { get; set; }

        /// <summary>
        /// Currently equipped specialisations on the character
        /// </summary>
        public List<APIBuildSpecialisation> Specialisations { get; set; } = new List<APIBuildSpecialisation>();

        /// <summary>
        /// Currently equipped skills on the character
        /// </summary>
        public APIBuildSkills Skills { get; set; } = new APIBuildSkills();

        /// <summary>
        /// Currently equipped pets
        /// </summary>
        public List<APIBuildPet> Pets { get; set; } = new List<APIBuildPet>();

        /// <summary>
        /// Currently equipped pieces of equipment
        /// </summary>
        public APIBuildEquipment Equipment { get; set; } = new APIBuildEquipment();

        internal Tools.Profession ProfessionData { get; set; }

        internal APIBuild() { }

        internal const char emptySlot = '0';

        internal static string Letterize(int?[] relativeIds)
        {
            StringBuilder result = new StringBuilder();
            for (int x = 0; x < relativeIds.Length; x++)
            {
                int relativeId = relativeIds[x] ?? -1;
                if (relativeId > 2704)
                {
                    int squaredQuotient = Math.DivRem(relativeId - 2704, 2704, out int squaredRemainder);
                    int remainderQuotient = Math.DivRem(squaredRemainder, 52, out int remainderRemainder);
                    result.Append($"_{Letterize(new int?[] { squaredQuotient, remainderQuotient, remainderRemainder })}");
                }
                else if (relativeId > 51)
                {
                    result.Append($"-{Letterize(new int?[] { (int)Math.Floor((relativeId - 52) / 52d), relativeId % 52 })}");
                }
                else if (relativeId > 25)
                {
                    result.Append(((char)(65 + relativeId - 26)).ToString());
                }
                else if (relativeId < 0)
                {
                    result.Append(emptySlot);
                }
                else
                {
                    result.Append(((char)(97 + relativeId)).ToString());
                }
            }
            return result.ToString();
        }

        internal static int AlphaToInt(char alpha) => alpha - ((alpha < 97) ? 39 : 97);

        internal static int[] Deletterize(string code)
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
                    result.Add((squaredQuotient + 1) * 2704 + remainderQuotient * 52 + remainderRemainder);
                    x += 3;
                }
                else if (l == '-')
                {
                    int quotient = AlphaToInt(letters[x + 1]);
                    int remainder = AlphaToInt(letters[x + 2]);
                    result.Add((quotient + 1) * 52 + remainder);
                    x += 2;
                }
                else if (l == emptySlot)
                {
                    result.Add(-1);
                }
                else
                {
                    result.Add(AlphaToInt(letters[x]));
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Get the build code for the build
        /// </summary>
        /// <returns>Hardstuck Builds code</returns>
        public string GetBuildCode()
        {
            List<int?> relativeIds = new List<int?>
            {
                VersionChar,
                (int)GameMode,
                ProfessionData.RelativeId,
            };

            foreach (APIBuildSpecialisation s in Specialisations)
            {
                relativeIds.Add(s.RelativeId);
                foreach (APIBuildTrait t in s.Traits)
                {
                    relativeIds.Add((int)t.Position);
                }
            }

            foreach (APIBuildSkill s in Skills.Heals)
            {
                relativeIds.Add(s.RelativeId);
            }

            foreach (APIBuildSkill s in Skills.Utilities)
            {
                relativeIds.Add(s.RelativeId);
            }

            int fill = 3 - Skills.Utilities.Count;
            if (fill > 0) // rev will never have this problem and fill will always be -3 on rev, so skip it
            {
                for (int f = 0; f < fill; f++)
                {
                    relativeIds.Add(-1);
                }
            }

            foreach (APIBuildSkill s in Skills.Elites)
            {
                relativeIds.Add(s.RelativeId);
            }

            if (Equipment is APIBuildPvEEquipment PvEEquipment)
            {
                AttributeType curStat = null;
                int curStatCounter = 0;

                foreach (APIBuildWeapon w in PvEEquipment.Weapons)
                {
                    if (!(w is null))
                    {
                        relativeIds.Add(w.RelativeId);
                    }
                    else
                    {
                        relativeIds.Add(-1);
                    }
                }

                foreach (APIBuildItem i in PvEEquipment.AllItems)
                {
                    if (!(i is null))
                    {
                        if (curStat is null)
                        {
                            curStat = i.AttributeType;
                            relativeIds.Add(i.AttributeType.Id);
                        }

                        if (i.AttributeType.Name != curStat.Name)
                        {
                            if (curStatCounter > 0)
                            {
                                relativeIds.Add(curStatCounter);
                            }

                            curStatCounter = 0;
                            relativeIds.Add(i.AttributeType.Id);
                            curStat = i.AttributeType;
                        }
                        curStatCounter++;
                    }
                }

                if (curStatCounter > 0)
                {
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
                        {
                            relativeIds.Add(curRuneCounter);
                        }

                        curRuneCounter = 0;
                        relativeIds.Add(r);
                        curRune = r;
                    }
                    curRuneCounter++;
                }

                if (curRuneCounter > 0)
                {
                    relativeIds.Add(curRuneCounter);
                }

                relativeIds.AddRange(PvEEquipment.Sigils);
            }
            else if (Equipment is APIBuildPvPEquipment PvPEquipment)
            {
                foreach (APIBuildWeapon w in PvPEquipment.Weapons)
                {
                    if (!(w is null))
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

            if (ProfessionData.Name.Equals("Ranger"))
            {
                for (int x = 0; x < Pets.Count; x++)
                {
                    if (!(Pets[x] is null))
                    {
                        relativeIds.Add(Pets[x].Id);
                    }
                }
            }

            return Letterize(relativeIds.ToArray());
        }

        /// <summary>
        /// Get the build code link for the build
        /// </summary>
        /// <returns>Hardstuck.GG/GW2/Builds link with the build code</returns>
        public string GetBuildLink() => $"https://hardstuck.gg/gw2/builds/?build={GetBuildCode()}";
    }
}
