using Assets.EverlastingDungeons.Scripts.Gui.Objects;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using Assets.EverlastingDungeons.Scripts.Utility;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Entity.Player
{
    public class PlayerData
    {
        public static readonly float RebirthMultiplier = 0.25f;

        [BsonId, JsonIgnore]
        public ObjectId id = ObjectId.GenerateNewId();

        public string username;
        public string password;

        public double level = 1;
        public double xp;
        public double rebirthCount = 0;

        public double money;
        public double gems;

        public double highestFloor = 1;
        public double currentFloor = 1;
        public bool passedTutorial;

        public RarityType minimumRarity;

        public PlayerMasteries masteries = new();

        public List<PlayerInventoryItem> items = new();
        public List<PlayerSpell> spells = new();
        public Dictionary<string, List<StatType>> wishStats = new();

        public PlayerData()
        {
        }

        [JsonIgnore]
        public Rarity MinimumRarity => Rarity.Get(PrefabManager.Instance.activePlayer.Data.minimumRarity);

        public PlayerData(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public List<StatType> GetWishStats(GearType gear)
        {
            if (!wishStats.TryGetValue(gear.ToString(), out var stats))
            {
                var d = new List<StatType>();
                wishStats.Add(gear.ToString(), d);
                return d;
            }
            else
            {
                return stats;
            }
        }

        public void ForceDefaultData()
        {
            // Add wish stats
            wishStats ??= new();

            // Add Default Spells
            foreach (var spell in ResourceManager.Instance.spells.Raw.Where(x => x.canPlayerUse && x.unlockedByDefault))
            {
                AddSpell(spell.uuid, 1);
            }
        }

        public double GetRebirthCost() => Math.Pow(10, 3 + rebirthCount);
        public double GetRebirthMultiplier() => 1 + RebirthMultiplier * rebirthCount;
        
        public void Rebirth()
        {
            gems -= GetRebirthCost();
            items.Clear();
            level = 1;
            xp = 0;
            money = 0;
            currentFloor = 1;
            rebirthCount++;
        }

        public void AddSpell(string uuid, long level = 1)
        {
            var current = GetSpell(uuid);
            if (current != null)
            {
                current.level += level;
            }
            else
            {
                spells.Add(new(uuid, level));
            }
        }

        public double GetFloorLevel() => currentFloor;

        public PlayerInventoryItem GetItemByIndex(int index) => items.FirstOrDefault(x => x.index == index);

        public void AddItem(PlayerInventoryItem item, bool forceNoStack = false)
        {
            if (item.Item.IsStackable && !forceNoStack)
            {
                var index = items.FindIndex(x => x.itemUUID == item.itemUUID);
                if (index == -1)
                {
                    AddItem(item, true);
                    return;
                }
                else
                {
                    items[index].amount += item.amount;
                }
            }
            else
            {
                items.Add(item);
            }
        }

        public void SetSpellIndex(string uuid, int index)
        {
            var current = GetSpellByIndex(index);
            if (current != null)
                current.index = -1;
            GetSpell(uuid).index = index;
        }

        public PlayerSpell GetSpellByIndex(int index) => spells.FirstOrDefault(x => x.index == index);
        public PlayerSpell GetSpell(string uuid) => spells.FirstOrDefault(x => x.spellUUID == uuid);
    }
}
