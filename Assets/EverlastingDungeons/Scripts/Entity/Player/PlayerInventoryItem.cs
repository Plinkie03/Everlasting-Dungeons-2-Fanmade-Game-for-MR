using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using Assets.EverlastingDungeons.Scripts.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Entity.Player
{
    public class PlayerInventoryItem
    {
        public static readonly float PriceMultiplier = 0.1f;

        public double amount;
        public string itemUUID;

        public Stat[] stats;
        public RarityType rarityType;
        public float multiplier = 1f;
        public bool equipped, locked;
        public double level = 1;
        public int index = -1;

        [JsonIgnore]
        public Rarity Rarity => Rarity.Get(rarityType);

        [JsonIgnore]
        public BaseItemSO Item => ResourceManager.Instance.items.Get(itemUUID);

        [JsonIgnore]
        public int Index => PrefabManager.Instance.player.Data.items.IndexOf(this);

        public PlayerInventoryItem() { }
        public PlayerInventoryItem(string itemUUID, double level = 1, double amount = 1, double rarityMult = 1f)
        {
            this.itemUUID = itemUUID;
            this.amount = amount;
            this.level = level;
            if (Item is GearItemSO)
            {
                rarityType = Rarity.GetRandomRarity(level * rarityMult).type;
                multiplier = this.Rarity.RandomMultiplier;
                stats = Stat.CreateStats(Item, Rarity, level, multiplier);
            }
        }

        [JsonIgnore]
        public Aura Aura => Aura.GetAuraForLevel(level);
        public double Price => Math.Round(Math.Pow(Item.price * multiplier * (1 + level * PriceMultiplier) * amount, Aura.exponent));
        public bool IsSellable => !locked && !equipped && Item.IsSellable;
        public void Sell()
        {
            PrefabManager.Instance.player.Data.money += Price;
            Destroy();
        }

        public void Destroy()
        {
            index = -1;
            PrefabManager.Instance.player.Data.items.Remove(this);
        }
        public void SubAmount(long amount)
        {
            this.amount -= amount;
            if (this.amount <= 0)
                Destroy();
        }

        [JsonIgnore]
        public Color SpriteColor => Aura.color;
        [JsonIgnore]
        public string DisplayName => Rarity.Color($"{(Aura.type is AuraType.None ? string.Empty : $"{Aura.type} ")}{Item.name} (T{((int)rarityType) + 1})");
        public double GetStatMultiplier(StatType stat)
        {
            if (Item is not GearItemSO gear)
                return default;
            return Math.Pow(stats.Concat(gear.defaultStats).Where(x => x.type == stat && x.modifier == StatModifierType.Multiplier).Sum(x => x.value), Stat.IsAbsolute(stat) ? Aura.exponent : 1);
        }

        public double GetStatAbsolute(StatType stat)
        {
            if (Item is not GearItemSO gear)
                return default;
            return Math.Pow(stats.Concat(gear.defaultStats).Where(x => x.type == stat && x.modifier == StatModifierType.Absolute).Sum(x => x.value), Aura.exponent);
        }
    }
}
