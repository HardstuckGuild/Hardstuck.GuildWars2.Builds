using Hardstuck.GuildWars2.Builds.APIClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardstuck.GuildWars2.Builds
{
    /// <summary>
    /// GW2Build class handles getting the current build from the API.
    /// </summary>
    public sealed class GW2BuildParser : IDisposable
    {
        #region definitions
        /// <summary>
        /// The API key used to call to GW2 API
        /// </summary>
        public string ApiKey
        {
            get => api.ApiKey;
            set
            {
                ChangeApiKey(value);
            }
        }
        private static readonly HashSet<string> approvedAPIKeys = new HashSet<string>();
        private readonly GW2Api api;

        private List<string> allProfessions = null;
        private List<int> allStats = null;
        private List<ItemStats> allStatData = null;
        private List<string> statNames = null;
        #endregion

        /// <summary>
        /// Create GW2BuildParser class without the specified API key.
        /// Make sure you set the key later or NoAPIKeySetException will be thrown.
        /// </summary>
        public GW2BuildParser()
        {
            api = new GW2Api();
        }

        /// <summary>
        /// Create GW2BuildParser class with the specified API key and check for API key permissions.
        /// </summary>
        /// <param name="apiKey">API key</param>
        /// <param name="checkPerms">Whether to check perms and raise an error if the key has not enough permissions</param>
        /// <exception cref="NoAPIKeySetException">Thrown when an error is raised by not supplying an API key.</exception>
        /// <exception cref="NotEnoughPermissionsException">Thrown when an error is raised by missing API key permissions.</exception>
        public GW2BuildParser(string apiKey, bool checkPerms = true)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new NoAPIKeySetException();
            }
            if (approvedAPIKeys.Contains(apiKey))
            {
                checkPerms = false;
            }
            api = new GW2Api(apiKey, checkPerms);
            approvedAPIKeys.Add(apiKey);
        }

        /// <summary>
        /// Change the currently active API key.
        /// </summary>
        /// <param name="apiKey">API key</param>
        /// <exception cref="NoAPIKeySetException">Thrown when an error is raised by not supplying an API key.</exception>
        /// <exception cref="NotEnoughPermissionsException">Thrown when an error is raised by missing API key permissions.</exception>
        public void ChangeApiKey(string apiKey)
        {
            bool checkPerms = true;
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new NoAPIKeySetException();
            }
            if (approvedAPIKeys.Contains(apiKey))
            {
                checkPerms = false;
            }
            api.ChangeApiKey(apiKey, checkPerms);
            approvedAPIKeys.Add(apiKey);
        }

        /// <summary>
        /// Gets the information about the exact build a given character in a given game mode is running.
        /// </summary>
        /// <param name="characterName">name of the character</param>
        /// <param name="mode">given game mode</param>
        /// <returns>information about the exact build a given character in a given game mode</returns>
        /// <exception cref="NoAPIKeySetException">Thrown when an error is raised by not setting up API key.</exception>
        public async Task<APIBuild> GetAPIBuildAsync(string characterName, GameMode mode)
        {
            if (!api.ApiKeySet)
            {
                throw new NoAPIKeySetException();
            }

            characterName = characterName.ToCharacterName();

            APIBuild APIBuild = new APIBuild()
            {
                CharacterName = characterName,
                GameMode = mode,
            };

            string gameModeString = Enum.GetName(typeof(GameMode), mode).ToLower();

            Character APIData = await api.Request<Character>($"v2/characters/{characterName}");
            List<CharacterSpecialisation> specialisations = APIData.Specialisations[gameModeString];

            if (allProfessions is null)
            {
                allProfessions = await api.Request<List<string>>("v2/professions");
            }

            if (allStats is null)
            {
                allStats = await api.Request<List<int>>("v2/itemstats");
            }

            if (allStatData is null)
            {
                allStatData = new List<ItemStats>();

                int statCounter = 0;
                int queryCounter = 0;

                while (statCounter < allStats.Count)
                {
                    StringBuilder statQuery = new StringBuilder("ids=");

                    while ((queryCounter < 100) && (statCounter < allStats.Count))
                    {
                        statQuery.Append($"{allStats[statCounter]},");

                        queryCounter++;
                        statCounter++;
                    }

                    List<ItemStats> statData = await api.Request<List<ItemStats>>("v2/itemstats", statQuery.ToString());
                    allStatData.AddRange(statData);

                    queryCounter = 0;
                }
            }

            if (statNames is null)
            {
                statNames = new List<string>();

                for (int x = 0; x < allStats.Count; x++)
                {
                    if ((allStatData[x].Attributes.Count < 3) || allStatData[x].Attributes[0].Multiplier.Equals(0f) || allStatData[x].Name.Equals(""))
                    {
                        allStats[x] = -1;
                    }
                    else
                    {
                        statNames.Add(allStatData[x].Name);
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
            }

            APIBuild.ProfessionData = await api.Request<Tools.Profession>($"v2/professions/{APIData.Profession}");
            APIBuild.Profession = APIBuild.ProfessionData.Name;
            APIBuild.ProfessionData.RelativeId = allProfessions.IndexOf(APIBuild.ProfessionData.Name);

            List<APIBuildTrait> allTraits = new List<APIBuildTrait>();

            StringBuilder traitQuery = new StringBuilder("ids=");

            EliteSpecialisation eliteSpec = EliteSpecialisation.None;

            for (int x = 0; x < specialisations.Count; x++)
            {
                if (x == 2) // elite spec
                {
                    eliteSpec = (EliteSpecialisation)specialisations[x].Id;
                }
                APIBuildSpecialisation spec = new APIBuildSpecialisation
                {
                    Id = specialisations[x].Id,
                    RelativeId = APIBuild.ProfessionData.Specialisations.ToList().IndexOf(specialisations[x].Id),
                };
                for (int t = 0; t < specialisations[x].Traits.Count; t++)
                {
                    APIBuildTrait trait;
                    if (!(specialisations[x].Traits[t] is null))
                    {
                        trait = new APIBuildTrait
                        {
                            Id = (int)specialisations[x].Traits[t],
                        };
                        traitQuery.Append($"{trait.Id},");
                    }
                    else
                    {
                        trait = new APIBuildTrait
                        {
                            Id = -1,
                            Position = TraitPosition.Unselected,
                        };

                    }
                    spec.Traits.Add(trait);
                    allTraits.Add(trait);
                }
                APIBuild.Specialisations.Add(spec);
            }

            List<Trait> traitData = await api.Request<List<Trait>>("v2/traits", traitQuery.ToString());

            for (int x = 0; x < traitData.Count; x++)
            {
                allTraits[x].Position = (TraitPosition)traitData[x].Order;
            }

            CharacterSkills skills = APIData.Skills[gameModeString];

            // skills

            // Water        / Legend2               -> Shiro
            // Fire         / Legend1               -> Glint
            // Earth        / Legend4               -> Mallyx
            // Air          / Legend3               -> Jalis
            // Deathshroud  / Legend6               -> Ventari
            // null         / Legend5               -> Kalla (lol)
            // null         / (not in API at all)   -> Vindicator (lol)
            if (APIBuild.ProfessionData.Name.Equals("Revenant"))
            {
                StringBuilder legendQuery = new StringBuilder("ids=");
                for (int l = 0; l < skills.Legends.Count; l++)
                {
                    APIBuildRevenantLegend legend = APIBuildRevenantLegend.Parse(skills.Legends[l].ToString(), eliteSpec);
                    legendQuery.Append($"{legend.Name},");
                }

                List<Legend> legendData = await api.Request<List<Legend>>("v2/legends", legendQuery.ToString());

                if (skills.Legends.Count == 1)
                {
                    // the api is doing a thing, construct the first (currently equipped) legend from the skills.heal skills.util skills.elite stuff
                    legendData.Add(new Legend()
                    {
                        Heal = skills.Heal,
                        Utilities = skills.Utilities.Where(i => i.HasValue).Select(i => i.Value).ToList(),
                        Elite = (int)skills.Elite,
                    });
                }

                for (int l = 0; l < legendData.Count; l++)
                {
                    APIBuild.Skills.Heals.Add(new APIBuildSkill()
                    {
                        Id = legendData[l].Heal,
                        RelativeId = APIBuild.ProfessionData.Skills.IndexOf(APIBuild.ProfessionData.Skills.FirstOrDefault(s => s.Id.Equals(legendData[l].Heal))),
                    });

                    APIBuild.Skills.Utilities.AddRange(new APIBuildSkill[] {
                        new APIBuildSkill() { Id = legendData[l].Utilities[0], RelativeId = APIBuild.ProfessionData.Skills.IndexOf(APIBuild.ProfessionData.Skills.FirstOrDefault(s => s.Id.Equals(legendData[l].Utilities[0]))) },
                        new APIBuildSkill() { Id = legendData[l].Utilities[1], RelativeId = APIBuild.ProfessionData.Skills.IndexOf(APIBuild.ProfessionData.Skills.FirstOrDefault(s => s.Id.Equals(legendData[l].Utilities[1]))) },
                        new APIBuildSkill() { Id = legendData[l].Utilities[2], RelativeId = APIBuild.ProfessionData.Skills.IndexOf(APIBuild.ProfessionData.Skills.FirstOrDefault(s => s.Id.Equals(legendData[l].Utilities[2]))) },
                    });

                    APIBuild.Skills.Elites.Add(new APIBuildSkill()
                    {
                        Id = legendData[l].Elite,
                        RelativeId = APIBuild.ProfessionData.Skills.IndexOf(APIBuild.ProfessionData.Skills.FirstOrDefault(s => s.Id.Equals(legendData[l].Elite))),
                    });
                }
            }
            else
            {
                APIBuild.Skills.Heals.Add(new APIBuildSkill()
                {
                    Id = skills.Heal,
                    RelativeId = APIBuild.ProfessionData.Skills.IndexOf(APIBuild.ProfessionData.Skills.FirstOrDefault(s => s.Id.Equals(skills.Heal))),
                });

                APIBuild.Skills.Utilities.AddRange(new APIBuildSkill[] {
                    new APIBuildSkill() { Id = (int)skills.Utilities[0], RelativeId = APIBuild.ProfessionData.Skills.IndexOf(APIBuild.ProfessionData.Skills.FirstOrDefault(s => s.Id.Equals(skills.Utilities[0]))) },
                    new APIBuildSkill() { Id = (int)skills.Utilities[1], RelativeId = APIBuild.ProfessionData.Skills.IndexOf(APIBuild.ProfessionData.Skills.FirstOrDefault(s => s.Id.Equals(skills.Utilities[1]))) },
                    new APIBuildSkill() { Id = (int)skills.Utilities[2], RelativeId = APIBuild.ProfessionData.Skills.IndexOf(APIBuild.ProfessionData.Skills.FirstOrDefault(s => s.Id.Equals(skills.Utilities[2]))) },
                });

                // few missing cases, they actually fixed jaunt though because of the mes bug - neat!
                if ((skills.Elite is null) && APIBuild.Profession.Equals("Engineer"))
                {
                    skills.Elite = 30800;
                }

                APIBuild.Skills.Elites.Add(new APIBuildSkill()
                {
                    Id = (int)skills.Elite,
                    RelativeId = APIBuild.ProfessionData.Skills.IndexOf(APIBuild.ProfessionData.Skills.FirstOrDefault(s => s.Id.Equals(skills.Elite))),
                });
            }

            if (mode.Equals(GameMode.PvP))
            {
                CharacterEquipmentPvP apiEquipmentPvP = APIData.EquipmentPvP;
                APIBuildPvPEquipment equipment = new APIBuildPvPEquipment
                {
                    Amulet = (apiEquipmentPvP.Amulet is null) ? 0 : (int)apiEquipmentPvP.Amulet,
                    Rune = (apiEquipmentPvP.Rune is null) ? 0 : (int)apiEquipmentPvP.Rune,
                };

                for (int x = 0; x < apiEquipmentPvP.Sigils.Count; x++)
                {
                    equipment.Sigils[x] = (apiEquipmentPvP.Sigils[x] is null) ? 0 : (int)apiEquipmentPvP.Sigils[x];
                }

                List<CharacterEquipment> apiEquipment = APIData.Equipment;

                List<CharacterEquipment> weapons = new List<CharacterEquipment>()
                {
                    apiEquipment.FirstOrDefault(s => s.Slot.Equals("WeaponA1")),
                    apiEquipment.FirstOrDefault(s => s.Slot.Equals("WeaponA2")),
                    apiEquipment.FirstOrDefault(s => s.Slot.Equals("WeaponB1")),
                    apiEquipment.FirstOrDefault(s => s.Slot.Equals("WeaponB2"))
                };

                StringBuilder weaponQuery = new StringBuilder("ids=");

                for (int x = 0; x < weapons.Count; x++)
                {
                    if (!(weapons[x] is null))
                    {
                        weaponQuery.Append($"{weapons[x].Id},");
                    }
                }

                List<Item> weaponData = await api.Request<List<Item>>("v2/items", weaponQuery.ToString());

                for (int x = 0; x < weapons.Count; x++)
                {
                    if (!(weapons[x] is null))
                    {
                        APIBuildWeapon weapon = new APIBuildWeapon();
                        Item item = weaponData.FirstOrDefault(s => s.Id.Equals(weapons[x].Id));

                        weapon.Id = item.Id;
                        weapon.Type = (WeaponType)Enum.Parse(typeof(WeaponType), item.Details.Type.ToString().Substring(0, 1).ToUpper() + item.Details.Type.ToString().ToLower().Substring(1));

                        int wIndex = 0;
                        foreach (var w in APIBuild.ProfessionData.Weapons)
                        {
                            if (w.Key.Equals(weapon.Type.ToString()))
                            {
                                weapon.RelativeId = wIndex;
                            }
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
            else // PvE & WvW
            {
                APIBuildPvEEquipment equipment = new APIBuildPvEEquipment();

                List<CharacterEquipment> apiEquipment = APIData.Equipment;

                List<CharacterEquipment> weapons = new List<CharacterEquipment>()
                {
                    apiEquipment.FirstOrDefault(s => s.Slot.Equals("WeaponA1")),
                    apiEquipment.FirstOrDefault(s => s.Slot.Equals("WeaponA2")),
                    apiEquipment.FirstOrDefault(s => s.Slot.Equals("WeaponB1")),
                    apiEquipment.FirstOrDefault(s => s.Slot.Equals("WeaponB2"))
                };

                StringBuilder weaponQuery = new StringBuilder("ids=");

                for (int x = 0; x < weapons.Count; x++)
                {
                    if (!(weapons[x] is null))
                    {
                        weaponQuery.Append($"{weapons[x].Id},");
                    }
                }

                List<Item> weaponData = await api.Request<List<Item>>("v2/items", weaponQuery.ToString());

                for (int x = 0; x < weapons.Count; x++)
                {
                    if (!(weapons[x] is null))
                    {
                        APIBuildWeapon weapon = new APIBuildWeapon();
                        Item item = weaponData.FirstOrDefault(s => s.Id.Equals(weapons[x].Id));

                        weapon.Id = item.Id;
                        weapon.Type = (WeaponType)Enum.Parse(typeof(WeaponType), item.Details.Type.ToString().Substring(0, 1).ToUpper() + item.Details.Type.ToString().ToLower().Substring(1));

                        int wIndex = 0;
                        foreach (var w in APIBuild.ProfessionData.Weapons)
                        {
                            if (w.Key.Equals(weapon.Type.ToString()))
                            {
                                weapon.RelativeId = wIndex;
                            }
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
                    if (!NonGearTypes.Contains(apiEquipment[x].Slot))
                    {
                        APIBuildItem item = new APIBuildItem
                        {
                            Id = apiEquipment[x].Id,
                            Slot = apiEquipment[x].Slot,
                        };

                        CharacterEquipment tryFindItem = APIData.Equipment.FirstOrDefault(s => s.Slot.Equals(apiEquipment[x].Slot));

                        if (!(tryFindItem is null) && !(tryFindItem.Stats is null))
                        {
                            ItemStats statData = allStatData.FirstOrDefault(s => s.Id.ToString().Equals(tryFindItem.Stats.Id.ToString()));
                            item.AttributeType = new AttributeType
                            {
                                Id = tryFindItem.Stats.Id,
                                Name = statData.Name,
                            };

                            item.AttributeType.RelativeId = allStats.IndexOf(item.AttributeType.Id);
                        }
                        else
                        {
                            itemsToQuery.Add(x, item.Id.ToString());
                        }

                        if (!(apiEquipment[x].Upgrades is null))
                        {
                            item.Upgrades = apiEquipment[x].Upgrades.ToArray();
                        }

                        int indexOf;

                        if ((indexOf = Armor.IndexOf(item.Slot)) != -1)
                        {
                            equipment.Armor[indexOf] = item;
                        }
                        else if ((indexOf = Trinkets.IndexOf(item.Slot)) != -1)
                        {
                            equipment.Trinkets[indexOf] = item;
                        }
                        else
                        {
                            APIBuildWeapon weapon = equipment.Weapons[Weapons.IndexOf(item.Slot)];
                            if (!(weapon is null))
                            {
                                weapon.Slot = item.Slot;
                                weapon.AttributeType = item.AttributeType;
                                weapon.Upgrades = item.Upgrades;
                            }
                        }
                    }
                }

                string itemQuery = $"ids={string.Join(",", itemsToQuery.Values)}";

                List<Item> loadedItems = new List<Item>();
                if (itemsToQuery.Keys.Count > 0)
                {
                    loadedItems = await api.Request<List<Item>>("v2/items", itemQuery);
                }

                foreach (KeyValuePair<int, string> pair in itemsToQuery)
                {
                    APIBuildItem[] items = equipment.AllItems.Where(i => i?.Id.ToString() == pair.Value).ToArray();
                    foreach (APIBuildItem item in items)
                    {
                        Item itemData = loadedItems.FirstOrDefault(s => s.Id.Equals(item.Id));
                        if (!(itemData.Details.InfixUpgrade is null))
                        {
                            item.AttributeType = new AttributeType
                            {
                                Id = (item.AttributeType is null) ? 0 : itemData.Details.InfixUpgrade.Id,
                                RelativeId = allStats.IndexOf(item.AttributeType?.Id ?? 0),
                            };
                        }
                        else
                        {
                            item.AttributeType = new AttributeType
                            {
                                Id = 0,
                                RelativeId = 0,
                            };
                        }
                    }
                }

                foreach (APIBuildItem i in equipment.Armor)
                {
                    if (!(i is null) && (i.Upgrades.Length > 0))
                    {
                        equipment.Runes.Add(i.Upgrades[0]);
                    }
                    else if (i is null)
                    {
                        equipment.Runes.Add(null);
                    }
                }

                foreach (APIBuildItem i in equipment.Weapons)
                {
                    if (!(i is null) && (i.Upgrades.Length > 0))
                    {
                        equipment.Sigils.AddRange(i.Upgrades);
                    }
                }

                APIBuild.Equipment = equipment;
            }

            if (APIBuild.ProfessionData.Name.Equals("Ranger"))
            {
                // pet
                for (int p = 0; p < skills.Pets["terrestrial"].Count; p++)
                {
                    APIBuild.Pets.Add(new APIBuildPet() { Id = skills.Pets["terrestrial"][p] });
                }
            }

            return APIBuild;
        }

        /// <summary>
        /// Dispose of all resoruces held by the class.
        /// </summary>
        public void Dispose()
        {
            api?.Dispose();
        }
    }
}
