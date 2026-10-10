using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Utility
{
    public enum StatType
    {
        MaxHealth,
        MaxMana,
        Lifesteal,
        AttackRate,
        CriticalRate,
        Speed,
        CriticalMultiplier,
        BlockRate,
        BlockReduction,
        Distance,
        Magic,
        Melee,
        MagicalResistance,
        MeleeResistance,
        DistanceResistance,
        DamageResistance,
        Defense,
        DodgeRate
    }

    public enum StatModifierType
    {
        Absolute,
        Multiplier
    }

    [Serializable]
    public class StatVariance
    {
        public float minAbsolute = 1f;
        public float maxAbsolute = 1f;
        public float minMultiplier = 1f;
        public float maxMultiplier = 1f;
    }

    [Serializable]
    public class Stat
    {
        public static readonly float LevelMultiplier = 0.05f;

        public static readonly StatType[] MultiplierOnlyStats = new StatType[]
        {
            StatType.CriticalRate,
            StatType.BlockRate,
            StatType.DodgeRate,
            StatType.BlockReduction,
            StatType.CriticalMultiplier,
            StatType.DamageResistance,
            StatType.Lifesteal,
            StatType.MagicalResistance,
            StatType.AttackRate,
            StatType.DistanceResistance,
            StatType.MeleeResistance,
            StatType.Speed
        };

        public static readonly StatType[] CombatStats = new StatType[]
        {
            StatType.Melee,
            StatType.Magic,
            StatType.Distance
        };

        public static readonly Dictionary<StatType, float> StatMultipliers = new()
        {
            {StatType.MaxHealth, 4f},
            {StatType.MaxMana, 1.5f},
            {StatType.Lifesteal, 0.2f},
            {StatType.AttackRate, -0.3f},
            {StatType.CriticalRate, 0.2f},
            {StatType.Speed, 0.1f},
            {StatType.CriticalMultiplier, 0.5f},
            {StatType.BlockRate, 0.15f},
            {StatType.BlockReduction, 0.075f},
            {StatType.Distance, 1.75f},
            {StatType.Magic, 1.75f},
            {StatType.Melee, 1.75f},
            {StatType.MagicalResistance, 0.33f},
            {StatType.MeleeResistance, 0.33f},
            {StatType.DistanceResistance, 0.33f},
            {StatType.DamageResistance, 0.1f},
            {StatType.Defense, 0.4f},
            {StatType.DodgeRate, 0.1f},
        };

        public StatType type;

        [EnumToggleButtons]
        public StatModifierType modifier;
        public double value;

        public static bool IsAbsolute(StatType type) => !MultiplierOnlyStats.Contains(type);
        public static bool IsMultiplier(StatType type) => MultiplierOnlyStats.Contains(type);

        public static bool IsCombatStat(StatType type) => CombatStats.Contains(type);
        public static Stat[] CreateStats(BaseItemSO rawItem, Rarity rarity, double level, float multiplier)
        {
            if (rawItem is not GearItemSO item || rarity.AdditionalStatCount == 0) return Array.Empty<Stat>();

            Stat[] stats = new Stat[rarity.AdditionalStatCount];
            Stat combatStat = default;

            for (int i = 0;i < rarity.AdditionalStatCount;i++)
            {
                var stat = new Stat();

                // First must be a combat stat always
                stat.type = i == 0 ? PrefabManager.Instance.activePlayer.gear.GetEquippedPiece(GearType.Weapon) != null ? PrefabManager.Instance.activePlayer.GetClass().CombatStat : CombatStats.Random() : PrefabManager.Instance.activePlayer.Data.GetWishStats(item.gearType).Count == 0 ? Util.GetRandomEnumValue<StatType>() : PrefabManager.Instance.activePlayer.Data.GetWishStats(item.gearType).Random();

                // Force combat stat to the first ever combat stat in this gear
                if (Stat.IsCombatStat(stat.type))
                {
                    combatStat ??= item.defaultStats.FirstOrDefault(x => Stat.IsCombatStat(x.type)) ?? stats.FirstOrDefault(x => x != null && Stat.IsCombatStat(x.type));
                    if (combatStat != null)
                        stat.type = combatStat.type;
                }

                stat.modifier = IsMultiplier(stat.type) ? StatModifierType.Multiplier : Util.GetRandomEnumValue<StatModifierType>();
                stat.value = (stat.modifier == StatModifierType.Absolute ? UnityEngine.Random.Range(item.statVariance.minAbsolute, item.statVariance.maxAbsolute) : UnityEngine.Random.Range(item.statVariance.minMultiplier, item.statVariance.maxMultiplier));
                
                // Only absolute stats are affected by level and multiplier
                if (Stat.IsAbsolute(stat.type))
                    stat.value *= (1 + level * LevelMultiplier) * multiplier;

                if (!(Stat.IsAbsolute(stat.type) && stat.modifier == StatModifierType.Multiplier))
                    stat.value *= StatMultipliers[stat.type];

                stats[i] = stat;
            }

            return stats;
        }

        public static string GetStatName(StatType statType) => statType switch
        {
            StatType.MaxHealth => "Max HP",
            StatType.MaxMana => "Max MP",
            StatType.Lifesteal => "Lifesteal",
            StatType.AttackRate => "ATK Rate",
            StatType.CriticalRate => "CRIT Rate",
            StatType.Speed => "Speed",
            StatType.CriticalMultiplier => "CRIT Mult",
            StatType.BlockRate => "Block Rate",
            StatType.BlockReduction => "Block Reduction",
            StatType.Distance => "Distance DMG",
            StatType.Magic => "Magic DMG",
            StatType.Melee => "Melee DMG",
            StatType.MagicalResistance => "Magical Res",
            StatType.MeleeResistance => "Melee Res",
            StatType.DistanceResistance => "Distance Res",
            StatType.DamageResistance => "DMG Res",
            StatType.Defense => "Defense",
            StatType.DodgeRate => "Dodge Rate",
            _ => "_"
        };

        public static Color GetStatColor(StatType type) => type switch
        {
            StatType.MaxHealth => Color.green,
            StatType.MaxMana => Color.blue,
            StatType.Lifesteal => Color.magenta,
            StatType.AttackRate => Color.yellow,
            StatType.CriticalRate => Color.orange,
            StatType.Speed => Color.white,
            StatType.CriticalMultiplier => Color.orangeRed,
            StatType.BlockRate => Color.aquamarine,
            StatType.BlockReduction => Color.darkCyan,
            StatType.Distance => Color.mediumPurple,
            StatType.Magic => Color.darkOrange,
            StatType.Melee => Color.cyan,
            StatType.MagicalResistance => Color.yellowNice,
            StatType.MeleeResistance => Color.yellowNice,
            StatType.DistanceResistance => Color.yellowNice,
            StatType.DamageResistance => Color.softYellow,
            StatType.Defense => Color.springGreen,
            StatType.DodgeRate => Color.gray,
            _ => Color.white
        };

        public static Stat Absolute(StatType stat, float value) => new Stat { modifier = StatModifierType.Absolute, type = stat, value = value };
        public static Stat Multiplier(StatType stat, float value) => new Stat { modifier = StatModifierType.Multiplier, type = stat, value = value };
    }
}
