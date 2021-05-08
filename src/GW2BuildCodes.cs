using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardstuck.GuildWars2.Builds
{
    public static class GW2BuildCodes
    {
        public static int[] ByteTo2BitInts(byte b)
        {
            bool[] bits = BitUtilities.ByteToBitArray(b);
            return new int[] { BitUtilities.TwoBitToInt(bits[0], bits[1]), BitUtilities.TwoBitToInt(bits[2], bits[3]), BitUtilities.TwoBitToInt(bits[4], bits[5]), BitUtilities.TwoBitToInt(bits[6], bits[7]) };
        }

        public static int RealIdFromPalette(int paletteId, dynamic palette)
        {
            for (int x = 0; x < palette.Count; x++)
            {
                if (palette[x][0] == paletteId)
                {
                    return palette[x][1];
                }
            }
            return -1;
        }

        public static int PaletteIdFromReal(int realId, dynamic palette)
        {
            for (int x = 0; x < palette.Count; x++)
            {
                if (palette[x][1] == realId)
                {
                    return palette[x][0];
                }
            }

            if (SkillPalette.FullPalette.ContainsKey(realId))
            {
                return SkillPalette.FullPalette[realId].PaletteId;
            }

            return -1;
        }

        public static APIBuildSkill SkillFromBytes(byte b1, byte b2, dynamic palette)
        {
            int paletteId = BitUtilities.JoinBytes(b1, b2);
            int realId = RealIdFromPalette(paletteId, palette);

            return new APIBuildSkill()
            {
                PaletteId = paletteId,
                Id = realId
            };
        }

        public static APIBuildSpecialization SpecializationFromBytes(byte id, byte traits)
        {
            int[] traitPositions = ByteTo2BitInts(traits);
            return new APIBuildSpecialization()
            {
                Id = id,
                Traits = new List<APIBuildTrait>()
                {
                    new APIBuildTrait(){ Position = (TraitPosition)(traitPositions[0]-1) },
                    new APIBuildTrait(){ Position = (TraitPosition)(traitPositions[1]-1) },
                    new APIBuildTrait(){ Position = (TraitPosition)(traitPositions[2]-1) }
                }
            };
        }

        #region Build Code Format
        /*
        Build chat links are base64 encoded data surrounded by [&  ].  

        byte	0xd
        byte	profession

        byte	spec1
        byte	traits1
        byte	spec2
        byte	traits2
        byte	spec3
        byte	traits3

        word	terrestrialHealingSkill
        word	aquaticHealingSkill
        word	terrestrialUtilitySkill1
        word	aquaticUtilitySkill1
        word	terrestrialUtilitySkill2
        word	aquaticUtilitySkill2
        word	terrestrialUtilitySkill3
        word	aquaticUtilitySkill3
        word	terrestrialEliteSkill
        word	aquaticSkillSkill

        16 bytes profession data

        traits:
        0b00ccbbaa

        aa adept traits
        bb master traits
        cc grandmaster traits

        trait values:
        00 no trait selected
        01 top trait
        02 middle trait
        03 bottom trait

        profession data:

	        revenants:
	        byte	activeTerriestralLegend
	        byte	inactiveTerrestrialLegend
	        byte 	activeAquaticLegend
	        byte 	inactiveAquaticLegend
	        word 	inactiveTerrestrialLegendUtilitySkill1
	        word 	inactiveTerrestrialLegendUtilitySkill2
	        word 	inactiveTerrestrialLegendUtilitySkill3
	        word 	inactiveAquaticLegendUtilitySkill1
	        word 	inactiveAquaticLegendUtilitySkill2
	        word 	inactiveAquaticLegendUtilitySkill3

	        rangers:
	        byte 	terrestrialPet1
	        byte	terrestrialPet2
	        byte 	aquaticPet1
	        byte	aquaticPet2
	        12 bytes 0x00 00 00 00   00 00 00 00   00 00 00 00
	
	        all other classes:
	        16 bytes 0x00 00 00 00   00 00 00 00   00 00 00 00   00 00 00 00
	
        revenant legends:
        0x0d Glint
        0x0e Shiro
        0x0f Jalis
        0x10 Mallyx
        0x11 Kalla
        0x12 Ventari

        profession:
        0x01 Guardian
        0x02 Warrior
        0x03 Engineer
        0x04 Ranger
        0x05 Thief
        0x06 Elementalist
        0x07 Mesmer
        0x08 Necromancer
        0x09 Revenant

        words are stored little endian
        so 0xef is stored ef 00


        Example:
        [&DQQIBwAANzZ5AHgAqwEAALUApQEAALwA7QDtABg9AAEAAAAAAAAAAAAAAAA=]
        Marksmanship	Bottom, Top, Empty
        Empty
        Soulbeast		Middle, Top, Bottom

        Terrestrial Skills:
        "We Heal As One!"
        Signet of Stone
        Frost Trap
        Empty
        "Strength of the Pack!"

        Aquatic Skills:
        Troll Unguent
        Empty
        "Sic 'Em!"
        Cold Snap
        "Strength of the Pack!"

        Terrestrial Pets:
        Polar Bear
        Fanged Iboga

        Aquatic Pets:
        Empty
        Jungle Stalker
        */
        #endregion

        public static async Task<APIBuild> GetBuildFromCode(string code)
        {

            using (var api = new GW2Api())
            {
                dynamic profData = await api.Request("v2/professions", "id=Ranger&v=latest");
                var palette = profData.skills_by_palette;
                //byte[] bytes   = Convert.FromBase64String("DQQIBwAANzZ5AHgAqwEAALUApQEAALwA7QDtABg9AAEAAAAAAAAAAAAAAAA=");
                byte[] bytes = Convert.FromBase64String(code);

                APIBuild build = new APIBuild();

                int profession = bytes[1];

                build.Profession = new Profession() { relativeId = profession - 1 };

                APIBuildSpecialization Spec1 = SpecializationFromBytes(bytes[2], bytes[3]);
                APIBuildSpecialization Spec2 = SpecializationFromBytes(bytes[4], bytes[5]);
                APIBuildSpecialization Spec3 = SpecializationFromBytes(bytes[6], bytes[7]);

                build.Specializations = new List<APIBuildSpecialization>() { Spec1, Spec2, Spec3 };

                APIBuildSkill HealSkillLand = SkillFromBytes(bytes[8], bytes[9], palette);
                //APIBuildSkill HealSkillWater = SkillFromBytes(bytes[10], bytes[11], palette);
                APIBuildSkill Utility1Land = SkillFromBytes(bytes[12], bytes[13], palette);
                //APIBuildSkill Utility1Water  = SkillFromBytes(bytes[14], bytes[15], palette);
                APIBuildSkill Utility2Land = SkillFromBytes(bytes[16], bytes[17], palette);
                //APIBuildSkill Utility2Water  = SkillFromBytes(bytes[18], bytes[19], palette);
                APIBuildSkill Utility3Land = SkillFromBytes(bytes[20], bytes[21], palette);
                //APIBuildSkill Utility3Water  = SkillFromBytes(bytes[22], bytes[23], palette);
                APIBuildSkill EliteLand = SkillFromBytes(bytes[24], bytes[25], palette);
                //APIBuildSkill EliteWater     = SkillFromBytes(bytes[26], bytes[27], palette);

                build.Skills.Heals = new List<APIBuildSkill>() { HealSkillLand };
                build.Skills.Utilities = new List<APIBuildSkill>() { Utility1Land, Utility2Land, Utility3Land };
                build.Skills.Elites = new List<APIBuildSkill>() { EliteLand };

                if (profession == 3)
                {
                    //Pets
                    int landPet1 = bytes[28];
                    int landPet2 = bytes[29];
                    //int waterPet1 = bytes[30];
                    //int waterPet2 = bytes[31];

                    build.Pets = new List<APIBuildPet>() { new APIBuildPet() { Id = landPet1 }, new APIBuildPet() { Id = landPet2 } };
                }
                else if (profession == 8)
                {
                    //rev
                    int activeLegendLand = bytes[28];
                    int inactiveLegendLand = bytes[29];
                    //int activeLegendWater   = bytes[30];
                    //int inactiveLegendWater = bytes[31];

                    APIBuildRevenantLegend Legend1 = APIBuildRevenantLegend.Parse(activeLegendLand.ToString());
                    APIBuildRevenantLegend Legend2 = APIBuildRevenantLegend.Parse(inactiveLegendLand.ToString());

                    string legendQuery = "";

                    if (Legend1 != null)
                        legendQuery += $"{Legend1.Name},";
                    if (Legend2 != null)
                        legendQuery += Legend2.Name;

                    if (legendQuery != "")
                    {
                        legendQuery = $"ids={legendQuery}";
                        dynamic legendData = await api.Request("v2/legends", legendQuery);

                        build.Skills.Heals = new List<APIBuildSkill>();
                        build.Skills.Utilities = new List<APIBuildSkill>();
                        build.Skills.Elites = new List<APIBuildSkill>();

                        for (int x = 0; x < legendData.Count; x++)
                        {
                            build.Skills.Heals.Add(new APIBuildSkill() { Id = (int)legendData[x].heal, PaletteId = PaletteIdFromReal((int)legendData[x].heal, palette), Type = "Heal" });

                            build.Skills.Utilities.Add(new APIBuildSkill() { Id = (int)legendData[x].utilities[0], PaletteId = PaletteIdFromReal((int)legendData[x].utilities[0], palette), Type = "Slot" });
                            build.Skills.Utilities.Add(new APIBuildSkill() { Id = (int)legendData[x].utilities[1], PaletteId = PaletteIdFromReal((int)legendData[x].utilities[1], palette), Type = "Slot" });
                            build.Skills.Utilities.Add(new APIBuildSkill() { Id = (int)legendData[x].utilities[2], PaletteId = PaletteIdFromReal((int)legendData[x].utilities[2], palette), Type = "Slot" });

                            build.Skills.Elites.Add(new APIBuildSkill() { Id = (int)legendData[x].elite, PaletteId = PaletteIdFromReal((int)legendData[x].elite, palette), Type = "Elite" });
                        }
                    }
                }

                return build;
            }
        }

        public static async void GetGW2CodeFromBuild(APIBuild build)
        {
            build = await GetBuildFromCode("DQQIBwAANzZ5AHgAqwEAALUApQEAALwA7QDtABg9AAEAAAAAAAAAAAAAAAA=");

            byte[] bytes = Convert.FromBase64String("DQQIBwAANzZ5AHgAqwEAALUApQEAALwA7QDtABg9AAEAAAAAAAAAAAAAAAA=");

            string testo = Convert.ToBase64String(bytes);

            string test = Convert.ToBase64String(new byte[]{
                13,
                (byte)build.Profession.relativeId,
                (byte)build.Specializations[0].RelativeId,

            });
        }
    }
}
