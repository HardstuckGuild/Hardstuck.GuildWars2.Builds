using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardstuck.GuildWars2.Builds
{
    public class GW2Builds
    {
        private readonly GW2Api api = new GW2Api();

        public async Task<APIBuild> GetAPIBuild_VLATEST(string characterName, GW2GameMode mode)
        {

            APIBuild APIBuild = new APIBuild()
            {
                CharacterName = characterName,
                GameMode = mode
            };

            string modestring = mode.ToString().ToLower();

            dynamic APIData = await api.Request($"v2/characters/{characterName}", "v=latest");
            dynamic ActiveBuild = APIData.build_tabs[(int)APIData.active_build_tab - 1].build;
            dynamic specializations = ActiveBuild.specializations;

            dynamic allProfessions = (await api.Request("v2/professions", "")).ToObject<List<string>>();

            List<int> allStats = (await api.Request("v2/itemstats", "")).ToObject<List<int>>();
            List<JObject> allStatData = new List<JObject>();

            int statCounter = 0;
            int queryCounter = 0;

            while (statCounter < allStats.Count)
            {
                string statQuery = "ids=";

                while (queryCounter < 100 && statCounter < allStats.Count)
                {
                    statQuery += $"{allStats[statCounter]},";

                    queryCounter++;
                    statCounter++;
                }

                dynamic statData = await api.Request("v2/itemstats", statQuery);
                for (int x = 0; x < statData.Count; x++)
                {
                    allStatData.Add(statData[x]);
                }

                queryCounter = 0;
            }

            List<string> statNames = new List<string>();

            for (int x = 0; x < allStats.Count; x++)
            {
                if (allStatData[x]["attributes"].Count() < 3 || (float)allStatData[x]["attributes"][0]["multiplier"] == 0 || (string)allStatData[x]["name"] == "")
                {
                    allStats[x] = -1;
                }
                else
                {
                    statNames.Add((string)allStatData[x]["name"]);
                }
            }

            for (int x = 0; x < allStats.Count; x++)
            {
                if (allStats[x] == -1)
                {
                    allStats.RemoveAt(x);
                    allStatData.RemoveAt(x);
                    x--;
                }
            }

            APIBuild.Profession = (await api.Request($"v2/professions/{APIData.profession}", "")).ToObject<Profession>();
            APIBuild.Profession.relativeId = allProfessions.IndexOf(APIBuild.Profession.name);

            List<APIBuildTrait> allTraits = new List<APIBuildTrait>();

            string traitQuery = "ids=";

            for (int x = 0; x < specializations.Count; x++)
            {
                APIBuildSpecialization spec = new APIBuildSpecialization
                {
                    Id = specializations[x].id,
                    RelativeId = APIBuild.Profession.specializations.ToList().IndexOf((int)specializations[x].id)
                };
                for (int t = 0; t < specializations[x].traits.Count; t++)
                {
                    APIBuildTrait trait;
                    if (specializations[x].traits[t] != null)
                    {
                        trait = new APIBuildTrait
                        {
                            Id = specializations[x].traits[t]
                        };
                        traitQuery += $"{trait.Id},";
                    }
                    else
                    {
                        trait = new APIBuildTrait
                        {
                            Id = -1,
                            Position = TraitPosition.Unselected
                        };

                    }
                    spec.Traits.Add(trait);
                    allTraits.Add(trait);
                }
                APIBuild.Specializations.Add(spec);
            }

            dynamic traitData = await api.Request("v2/traits", traitQuery);

            for (int x = 0; x < traitData.Count; x++)
                allTraits[x].Position = (TraitPosition)traitData[x].order;

            dynamic skills = ActiveBuild.skills;

            //APIBuild.Skills.Heals     = new int[] { skills.heal };
            //APIBuild.Skills.Utilities = new int[] { skills.utilities[0], skills.utilities[1], skills.utilities[2] };
            //APIBuild.Skills.Elites    = new int[] { skills.elite };

            //skills


            //Water / Legend2        -> Shiro
            //Fire  / Legend1        -> Glint
            //Earth / Legend4        -> Mallyx
            //Air   / Legend3        -> Jalis
            //Deathshroud / Legend6  -> Ventari
            //null        / Legend5  -> Kalla (lol) rev

            if (APIBuild.Profession.name == "Revenant")
            {
                string legendQuery = "ids=";
                for (int l = 0; l < skills.legends.Count; l++)
                {
                    APIBuildRevenantLegend legend = APIBuildRevenantLegend.Parse(skills.legends[l].ToString());
                    legendQuery += $"{legend.Name},";
                }

                dynamic legendData = await api.Request("v2/legends", legendQuery);

                for (int l = 0; l < legendData.Count; l++)
                {
                    APIBuild.Skills.Heals.Add(new APIBuildSkill()
                    {
                        Id = legendData[l].heal,
                        RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", legendData[l].heal))
                    });

                    APIBuild.Skills.Utilities.AddRange(new APIBuildSkill[] {
                        new APIBuildSkill() { Id = legendData[l].utilities[0], RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", legendData[l].utilities[0])) },
                        new APIBuildSkill() { Id = legendData[l].utilities[1], RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", legendData[l].utilities[1])) },
                        new APIBuildSkill() { Id = legendData[l].utilities[2], RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", legendData[l].utilities[2])) }
                    });

                    APIBuild.Skills.Elites.Add(new APIBuildSkill()
                    {
                        Id = legendData[l].elite,
                        RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", legendData[l].elite))
                    });
                }
            }
            else
            {
                APIBuild.Skills.Heals.Add(new APIBuildSkill()
                {
                    Id = skills.heal,
                    RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", skills.heal))
                });

                APIBuild.Skills.Utilities.AddRange(new APIBuildSkill[] {
                    new APIBuildSkill() { Id = skills.utilities[0], RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", skills.utilities[0])) },
                    new APIBuildSkill() { Id = skills.utilities[1], RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", skills.utilities[1])) },
                    new APIBuildSkill() { Id = skills.utilities[2], RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", skills.utilities[2])) }
                });

                APIBuild.Skills.Elites.Add(new APIBuildSkill()
                {
                    Id = skills.elite,
                    RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", skills.elite))
                });
            }

            if (mode == GW2GameMode.PvP)
            {
                dynamic apiEquipment = APIData.equipment_tabs[(int)APIData.active_equipment_tab - 1].equipment_pvp;
                APIBuildPvPEquipment equipment = new APIBuildPvPEquipment
                {
                    Amulet = (apiEquipment.amulet == null) ? 0 : (int)apiEquipment.amulet,
                    Rune = (apiEquipment.rune == null) ? 0 : (int)apiEquipment.rune
                };

                for (int x = 0; x < apiEquipment.sigils.Count; x++)
                {
                    if (apiEquipment.sigils[x] != null)
                        equipment.Sigils[x] = apiEquipment.sigils[x];
                    else
                        equipment.Sigils[x] = 0;
                }

                apiEquipment = APIData.equipment_tabs[(int)APIData.active_equipment_tab - 1].equipment;

                var weaponA1 = JSONUtilities.SearchJSONArrayByVariable(apiEquipment, "slot", "WeaponA1");
                var weaponA2 = JSONUtilities.SearchJSONArrayByVariable(apiEquipment, "slot", "WeaponA2");
                var weaponB1 = JSONUtilities.SearchJSONArrayByVariable(apiEquipment, "slot", "WeaponB1");
                var weaponB2 = JSONUtilities.SearchJSONArrayByVariable(apiEquipment, "slot", "WeaponB2");

                var weapons = new JObject[] { weaponA1, weaponA2, weaponB1, weaponB2 };

                string weaponQuery = "ids=";

                for (int x = 0; x < weapons.Length; x++)
                {
                    if (weapons[x] != null)
                        weaponQuery += $"{weapons[x]["id"]},";
                }

                dynamic weaponData = await api.Request("v2/items", weaponQuery);

                for (int x = 0; x < weapons.Length; x++)
                {
                    if (weapons[x] != null)
                    {
                        APIBuildWeapon weapon = new APIBuildWeapon();
                        var item = JSONUtilities.SearchJSONArrayByVariable(weaponData, "id", weapons[x]["id"]);

                        weapon.Id = item.id;
                        weapon.Type = Enum.Parse(typeof(WeaponType), item.details.type.ToString().Substring(0, 1).ToUpper() + item.details.type.ToString().ToLower().Substring(1));

                        //var weaponInfo    = APIBuild.Profession.weapons[weapon.Type.ToString()];
                        int wIndex = 0;
                        foreach (var w in APIBuild.Profession.weapons)
                        {
                            if (w.Key == weapon.Type.ToString())
                                weapon.RelativeId = wIndex;
                            wIndex++;
                        }

                        equipment.Weapons.Add(weapon);
                    }
                    else
                    {
                        equipment.Weapons.Add(null);
                    }
                }

                APIBuild.Equipment = equipment;
            }
            else
            {
                APIBuildPvEEquipment equipment = new APIBuildPvEEquipment();

                dynamic apiEquipment = APIData.equipment_tabs[(int)APIData.active_equipment_tab - 1].equipment;

                var weaponA1 = JSONUtilities.SearchJSONArrayByVariable(apiEquipment, "slot", "WeaponA1");
                var weaponA2 = JSONUtilities.SearchJSONArrayByVariable(apiEquipment, "slot", "WeaponA2");
                var weaponB1 = JSONUtilities.SearchJSONArrayByVariable(apiEquipment, "slot", "WeaponB1");
                var weaponB2 = JSONUtilities.SearchJSONArrayByVariable(apiEquipment, "slot", "WeaponB2");

                var weapons = new JObject[] { weaponA1, weaponA2, weaponB1, weaponB2 };

                string weaponQuery = "ids=";

                for (int x = 0; x < weapons.Length; x++)
                {
                    if (weapons[x] != null)
                        weaponQuery += $"{weapons[x]["id"]},";
                }

                dynamic weaponData = await api.Request("v2/items", weaponQuery);

                for (int x = 0; x < weapons.Length; x++)
                {
                    if (weapons[x] != null)
                    {
                        APIBuildWeapon weapon = new APIBuildWeapon();
                        var item = JSONUtilities.SearchJSONArrayByVariable(weaponData, "id", weapons[x]["id"]);

                        weapon.Id = item.id;
                        weapon.Type = Enum.Parse(typeof(WeaponType), item.details.type.ToString().Substring(0, 1).ToUpper() + item.details.type.ToString().ToLower().Substring(1));

                        //var weaponInfo    = APIBuild.Profession.weapons[weapon.Type.ToString()];
                        int wIndex = 0;
                        foreach (var w in APIBuild.Profession.weapons)
                        {
                            if (w.Key == weapon.Type.ToString())
                                weapon.RelativeId = wIndex;
                            wIndex++;
                        }

                        equipment.Weapons.Add(weapon);
                    }
                    else
                    {
                        equipment.Weapons.Add(null);
                    }
                }

                Dictionary<int, string> itemsToQuery = new Dictionary<int, string>();

                List<string> NonGearTypes = new List<string> { "Pick", "Axe", "Sickle", "HelmAquatic", "WeaponAquaticA", "WeaponAquaticB" };
                List<string> Armor = new List<string> { "Helm", "Shoulders", "Coat", "Gloves", "Leggings", "Boots" };
                List<string> Weapons = new List<string> { "WeaponA1", "WeaponA2", "WeaponB1", "WeaponB2" };
                List<string> Trinkets = new List<string> { "Backpack", "Accessory1", "Accessory2", "Amulet", "Ring1", "Ring2" };

                for (int x = 0; x < apiEquipment.Count; x++)
                {
                    if (!NonGearTypes.Contains((string)apiEquipment[x].slot))
                    {
                        APIBuildItem item = new APIBuildItem
                        {
                            Id = apiEquipment[x].id,
                            Slot = apiEquipment[x].slot
                        };

                        dynamic tryFindItem = JSONUtilities.SearchJSONArrayByVariable(APIData.equipment, "slot", (string)apiEquipment[x].slot);

                        if (tryFindItem != null && tryFindItem["stats"] != null)
                        {
                            string statId = tryFindItem["stats"].id.ToString();
                            dynamic statData = allStatData.FirstOrDefault(s => s["id"].ToString() == tryFindItem["stats"].id.ToString());
                            item.AttributeType = new AttributeType
                            {
                                Id = tryFindItem["stats"].id,
                                Name = statData.name
                            };

                            item.AttributeType.RelativeId = allStats.IndexOf(item.AttributeType.Id);
                        }
                        else
                        {
                            itemsToQuery.Add(x, item.Id.ToString());
                        }

                        if (apiEquipment[x]["upgrades"] != null)
                        {
                            item.Upgrades = apiEquipment[x]["upgrades"].ToObject<int[]>();
                        }

                        int indexOf;

                        if ((indexOf = Armor.IndexOf(item.Slot)) != -1)
                            equipment.Armor[indexOf] = item;
                        else if ((indexOf = Trinkets.IndexOf(item.Slot)) != -1)
                            equipment.Trinkets[indexOf] = item;
                        else
                        {
                            APIBuildWeapon weapon = equipment.Weapons[Weapons.IndexOf(item.Slot)];
                            weapon.Slot = item.Slot;
                            weapon.AttributeType = item.AttributeType;
                            weapon.Upgrades = item.Upgrades;
                        }
                    }
                }

                string itemQuery = $"ids={string.Join(",", itemsToQuery.Values)}";

                dynamic loadedItems = new List<object>();
                if (itemsToQuery.Keys.Count > 0)
                    loadedItems = await api.Request("v2/items", itemQuery);

                foreach (KeyValuePair<int, string> pair in itemsToQuery)
                {
                    APIBuildItem[] items = equipment.AllItems.Where(i => i?.Id.ToString() == pair.Value).ToArray();
                    foreach (APIBuildItem item in items)
                    {
                        var itemData = JSONUtilities.SearchJSONArrayByVariable(loadedItems, "id", item.Id);
                        if (itemData.details.infix_upgrade != null)
                        {
                            item.AttributeType = new AttributeType
                            {
                                Id = itemData.details.infix_upgrade.id
                            };
                            item.AttributeType.RelativeId = allStats.IndexOf(item.AttributeType.Id);
                        }
                        else
                        {
                            item.AttributeType = new AttributeType
                            {
                                Id = 0
                            };
                            item.AttributeType.RelativeId = 0;
                        }
                    }
                }

                foreach (APIBuildItem i in equipment.Armor)
                    if (i != null && i.Upgrades.Length > 0)
                        equipment.Runes.Add(i.Upgrades[0]);

                foreach (APIBuildItem i in equipment.Weapons)
                    if (i != null && i.Upgrades.Length > 0)
                        equipment.Sigils.AddRange(i.Upgrades);

                if (APIBuild.Profession.name == "Ranger")
                {
                    //pet
                    for (int p = 0; p < skills.pets.terrestrial.Count; p++)
                    {
                        APIBuild.Pets.Add(new APIBuildPet() { Id = skills.pets.terrestrial[p] });
                    }
                }

                APIBuild.Equipment = equipment;
            }

            return APIBuild;
        }

        public async Task<APIBuild> GetAPIBuild(string characterName, GW2GameMode mode)
        {

            APIBuild APIBuild = new APIBuild()
            {
                CharacterName = characterName,
                GameMode = mode
            };

            string modestring = mode.ToString().ToLower();

            dynamic APIData = await api.Request($"v2/characters/{characterName}", "");
            dynamic specializations = APIData.specializations[modestring];

            dynamic allProfessions = (await api.Request("v2/professions", "")).ToObject<List<string>>();

            List<int> allStats = (await api.Request("v2/itemstats", "")).ToObject<List<int>>();
            List<JObject> allStatData = new List<JObject>();

            int statCounter = 0;
            int queryCounter = 0;


            //TODO download this on launch instead of every time
            while (statCounter < allStats.Count)
            {
                string statQuery = "ids=";

                while (queryCounter < 100 && statCounter < allStats.Count)
                {
                    statQuery += $"{allStats[statCounter]},";

                    queryCounter++;
                    statCounter++;
                }

                dynamic statData = await api.Request("v2/itemstats", statQuery);
                for (int x = 0; x < statData.Count; x++)
                {
                    allStatData.Add(statData[x]);
                }

                queryCounter = 0;
            }

            List<string> statNames = new List<string>();

            for (int x = 0; x < allStats.Count; x++)
            {
                if (allStatData[x]["attributes"].Count() < 3 || (float)allStatData[x]["attributes"][0]["multiplier"] == 0 || (string)allStatData[x]["name"] == "")
                {
                    allStats[x] = -1;
                }
                else
                {
                    statNames.Add((string)allStatData[x]["name"]);
                }
            }

            for (int x = 0; x < allStats.Count; x++)
            {
                if (allStats[x] == -1)
                {
                    allStats.RemoveAt(x);
                    allStatData.RemoveAt(x);
                    x--;
                }
            }

            APIBuild.Profession = (await api.Request($"v2/professions/{APIData.profession}", "")).ToObject<Profession>();
            APIBuild.Profession.relativeId = allProfessions.IndexOf(APIBuild.Profession.name);

            List<APIBuildTrait> allTraits = new List<APIBuildTrait>();

            string traitQuery = "ids=";

            for (int x = 0; x < specializations.Count; x++)
            {
                APIBuildSpecialization spec = new APIBuildSpecialization
                {
                    Id = specializations[x].id,
                    RelativeId = APIBuild.Profession.specializations.ToList().IndexOf((int)specializations[x].id)
                };
                for (int t = 0; t < specializations[x].traits.Count; t++)
                {
                    APIBuildTrait trait;
                    if (specializations[x].traits[t] != null)
                    {
                        trait = new APIBuildTrait
                        {
                            Id = specializations[x].traits[t]
                        };
                        traitQuery += $"{trait.Id},";
                    }
                    else
                    {
                        trait = new APIBuildTrait
                        {
                            Id = -1,
                            Position = TraitPosition.Unselected
                        };

                    }
                    spec.Traits.Add(trait);
                    allTraits.Add(trait);
                }
                APIBuild.Specializations.Add(spec);
            }

            dynamic traitData = await api.Request("v2/traits", traitQuery);

            for (int x = 0; x < traitData.Count; x++)
                allTraits[x].Position = (TraitPosition)traitData[x].order;

            dynamic skills = APIData.skills[modestring];

            //APIBuild.Skills.Heals     = new int[] { skills.heal };
            //APIBuild.Skills.Utilities = new int[] { skills.utilities[0], skills.utilities[1], skills.utilities[2] };
            //APIBuild.Skills.Elites    = new int[] { skills.elite };

            //skills


            //Water / Legend2        -> Shiro
            //Fire  / Legend1        -> Glint
            //Earth / Legend4        -> Mallyx
            //Air   / Legend3        -> Jalis
            //Deathshroud / Legend6  -> Ventari
            //null        / Legend5  -> Kalla (lol) rev

            if (APIBuild.Profession.name == "Revenant")
            {
                string legendQuery = "ids=";
                for (int l = 0; l < skills.legends.Count; l++)
                {
                    APIBuildRevenantLegend legend = APIBuildRevenantLegend.Parse(skills.legends[l].ToString());
                    legendQuery += $"{legend.Name},";
                }

                dynamic legendData = await api.Request("v2/legends", legendQuery);

                for (int l = 0; l < legendData.Count; l++)
                {
                    APIBuild.Skills.Heals.Add(new APIBuildSkill()
                    {
                        Id = legendData[l].heal,
                        RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", legendData[l].heal))
                    });

                    APIBuild.Skills.Utilities.AddRange(new APIBuildSkill[] {
                        new APIBuildSkill() { Id = legendData[l].utilities[0], RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", legendData[l].utilities[0])) },
                        new APIBuildSkill() { Id = legendData[l].utilities[1], RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", legendData[l].utilities[1])) },
                        new APIBuildSkill() { Id = legendData[l].utilities[2], RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", legendData[l].utilities[2])) }
                    });

                    APIBuild.Skills.Elites.Add(new APIBuildSkill()
                    {
                        Id = legendData[l].elite,
                        RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", legendData[l].elite))
                    });
                }
            }
            else
            {
                APIBuild.Skills.Heals.Add(new APIBuildSkill()
                {
                    Id = skills.heal,
                    RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", skills.heal))
                });

                APIBuild.Skills.Utilities.AddRange(new APIBuildSkill[] {
                    new APIBuildSkill() { Id = skills.utilities[0], RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", skills.utilities[0])) },
                    new APIBuildSkill() { Id = skills.utilities[1], RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", skills.utilities[1])) },
                    new APIBuildSkill() { Id = skills.utilities[2], RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", skills.utilities[2])) }
                });

                APIBuild.Skills.Elites.Add(new APIBuildSkill()
                {
                    Id = skills.elite,
                    RelativeId = APIBuild.Profession.skills.IndexOf(JSONUtilities.SearchJSONArrayByVariable(APIBuild.Profession.skills, "id", skills.elite))
                });
            }

            if (mode == GW2GameMode.PvP)
            {
                dynamic apiEquipment = APIData.equipment_pvp;
                APIBuildPvPEquipment equipment = new APIBuildPvPEquipment
                {
                    Amulet = (apiEquipment.amulet == null) ? 0 : (int)apiEquipment.amulet,
                    Rune = (apiEquipment.rune == null) ? 0 : (int)apiEquipment.rune
                };

                for (int x = 0; x < apiEquipment.sigils.Count; x++)
                {
                    if (apiEquipment.sigils[x] != null)
                        equipment.Sigils[x] = apiEquipment.sigils[x];
                    else
                        equipment.Sigils[x] = 0;
                }

                apiEquipment = APIData.equipment;

                var weaponA1 = JSONUtilities.SearchJSONArrayByVariable(apiEquipment, "slot", "WeaponA1");
                var weaponA2 = JSONUtilities.SearchJSONArrayByVariable(apiEquipment, "slot", "WeaponA2");
                var weaponB1 = JSONUtilities.SearchJSONArrayByVariable(apiEquipment, "slot", "WeaponB1");
                var weaponB2 = JSONUtilities.SearchJSONArrayByVariable(apiEquipment, "slot", "WeaponB2");

                var weapons = new JObject[] { weaponA1, weaponA2, weaponB1, weaponB2 };

                string weaponQuery = "ids=";

                for (int x = 0; x < weapons.Length; x++)
                {
                    if (weapons[x] != null)
                        weaponQuery += $"{weapons[x]["id"]},";
                }

                dynamic weaponData = await api.Request("v2/items", weaponQuery);

                for (int x = 0; x < weapons.Length; x++)
                {
                    if (weapons[x] != null)
                    {
                        APIBuildWeapon weapon = new APIBuildWeapon();
                        var item = JSONUtilities.SearchJSONArrayByVariable(weaponData, "id", weapons[x]["id"]);

                        weapon.Id = item.id;
                        weapon.Type = Enum.Parse(typeof(WeaponType), item.details.type.ToString().Substring(0, 1).ToUpper() + item.details.type.ToString().ToLower().Substring(1));

                        //var weaponInfo    = APIBuild.Profession.weapons[weapon.Type.ToString()];
                        int wIndex = 0;
                        foreach (var w in APIBuild.Profession.weapons)
                        {
                            if (w.Key == weapon.Type.ToString())
                                weapon.RelativeId = wIndex;
                            wIndex++;
                        }

                        equipment.Weapons.Add(weapon);
                    }
                    else
                    {
                        equipment.Weapons.Add(null);
                    }
                }

                APIBuild.Equipment = equipment;
            }
            else
            {
                APIBuildPvEEquipment equipment = new APIBuildPvEEquipment();

                dynamic apiEquipment = APIData.equipment;

                var weaponA1 = JSONUtilities.SearchJSONArrayByVariable(apiEquipment, "slot", "WeaponA1");
                var weaponA2 = JSONUtilities.SearchJSONArrayByVariable(apiEquipment, "slot", "WeaponA2");
                var weaponB1 = JSONUtilities.SearchJSONArrayByVariable(apiEquipment, "slot", "WeaponB1");
                var weaponB2 = JSONUtilities.SearchJSONArrayByVariable(apiEquipment, "slot", "WeaponB2");

                var weapons = new JObject[] { weaponA1, weaponA2, weaponB1, weaponB2 };

                string weaponQuery = "ids=";

                for (int x = 0; x < weapons.Length; x++)
                {
                    if (weapons[x] != null)
                        weaponQuery += $"{weapons[x]["id"]},";
                }

                dynamic weaponData = await api.Request("v2/items", weaponQuery);

                for (int x = 0; x < weapons.Length; x++)
                {
                    if (weapons[x] != null)
                    {
                        APIBuildWeapon weapon = new APIBuildWeapon();
                        var item = JSONUtilities.SearchJSONArrayByVariable(weaponData, "id", weapons[x]["id"]);

                        weapon.Id = item.id;
                        weapon.Type = Enum.Parse(typeof(WeaponType), item.details.type.ToString().Substring(0, 1).ToUpper() + item.details.type.ToString().ToLower().Substring(1));

                        //var weaponInfo    = APIBuild.Profession.weapons[weapon.Type.ToString()];
                        int wIndex = 0;
                        foreach (var w in APIBuild.Profession.weapons)
                        {
                            if (w.Key == weapon.Type.ToString())
                                weapon.RelativeId = wIndex;
                            wIndex++;
                        }

                        equipment.Weapons.Add(weapon);
                    }
                    else
                    {
                        equipment.Weapons.Add(null);
                    }
                }

                Dictionary<int, string> itemsToQuery = new Dictionary<int, string>();

                List<string> NonGearTypes = new List<string> { "Pick", "Axe", "Sickle", "HelmAquatic", "WeaponAquaticA", "WeaponAquaticB" };
                List<string> Armor = new List<string> { "Helm", "Shoulders", "Coat", "Gloves", "Leggings", "Boots" };
                List<string> Weapons = new List<string> { "WeaponA1", "WeaponA2", "WeaponB1", "WeaponB2" };
                List<string> Trinkets = new List<string> { "Backpack", "Accessory1", "Accessory2", "Amulet", "Ring1", "Ring2" };

                for (int x = 0; x < apiEquipment.Count; x++)
                {
                    if (!NonGearTypes.Contains((string)apiEquipment[x].slot))
                    {
                        APIBuildItem item = new APIBuildItem
                        {
                            Id = apiEquipment[x].id,
                            Slot = apiEquipment[x].slot
                        };

                        dynamic tryFindItem = JSONUtilities.SearchJSONArrayByVariable(APIData.equipment, "slot", (string)apiEquipment[x].slot);

                        if (tryFindItem != null && tryFindItem["stats"] != null)
                        {
                            string statId = tryFindItem["stats"].id.ToString();
                            dynamic statData = allStatData.FirstOrDefault(s => s["id"].ToString() == tryFindItem["stats"].id.ToString());
                            item.AttributeType = new AttributeType
                            {
                                Id = tryFindItem["stats"].id,
                                Name = statData.name
                            };

                            item.AttributeType.RelativeId = allStats.IndexOf(item.AttributeType.Id);
                        }
                        else
                        {
                            itemsToQuery.Add(x, item.Id.ToString());
                        }

                        if (apiEquipment[x]["upgrades"] != null)
                        {
                            item.Upgrades = apiEquipment[x]["upgrades"].ToObject<int[]>();
                        }

                        int indexOf;

                        if ((indexOf = Armor.IndexOf(item.Slot)) != -1)
                            equipment.Armor[indexOf] = item;
                        else if ((indexOf = Trinkets.IndexOf(item.Slot)) != -1)
                            equipment.Trinkets[indexOf] = item;
                        else
                        {
                            APIBuildWeapon weapon = equipment.Weapons[Weapons.IndexOf(item.Slot)];
                            weapon.Slot = item.Slot;
                            weapon.AttributeType = item.AttributeType;
                            weapon.Upgrades = item.Upgrades;
                        }
                    }
                }

                string itemQuery = $"ids={string.Join(",", itemsToQuery.Values)}";

                dynamic loadedItems = new List<object>();
                if (itemsToQuery.Keys.Count > 0)
                    loadedItems = await api.Request("v2/items", itemQuery);

                foreach (KeyValuePair<int, string> pair in itemsToQuery)
                {
                    APIBuildItem[] items = equipment.AllItems.Where(i => i?.Id.ToString() == pair.Value).ToArray();
                    foreach (APIBuildItem item in items)
                    {
                        var itemData = JSONUtilities.SearchJSONArrayByVariable(loadedItems, "id", item.Id);
                        if (itemData.details.infix_upgrade != null)
                        {
                            item.AttributeType = new AttributeType
                            {
                                Id = itemData.details.infix_upgrade.id
                            };
                            item.AttributeType.RelativeId = allStats.IndexOf(item.AttributeType.Id);
                        }
                        else
                        {
                            item.AttributeType = new AttributeType
                            {
                                Id = 0
                            };
                            item.AttributeType.RelativeId = 0;
                        }
                    }
                }

                foreach (APIBuildItem i in equipment.Armor)
                    if (i != null && i.Upgrades.Length > 0)
                        equipment.Runes.Add(i.Upgrades[0]);

                foreach (APIBuildItem i in equipment.Weapons)
                    if (i != null && i.Upgrades.Length > 0)
                        equipment.Sigils.AddRange(i.Upgrades);

                if (APIBuild.Profession.name == "Ranger")
                {
                    //pet
                    for (int p = 0; p < skills.pets.terrestrial.Count; p++)
                    {
                        APIBuild.Pets.Add(new APIBuildPet() { Id = skills.pets.terrestrial[p] });
                    }
                }

                APIBuild.Equipment = equipment;
            }

            return APIBuild;
        }

        public void Dispose()
        {
            api?.Dispose();
        }
    }
}
