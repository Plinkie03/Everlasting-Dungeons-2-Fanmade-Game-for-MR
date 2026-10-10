using Assets.EverlastingDungeons.Scripts.Entity.Base;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Entity.Player
{
    public class PlayerBaseStats : EntityBaseStats
    {
        public static readonly double LevelMultiplier = 0.1d;
        private static readonly Dictionary<StatType, double> DefaultStats = new()
        {
            { StatType.MaxHealth, 100 },
            { StatType.MaxMana, 100 },
            { StatType.Magic, 5 },
            { StatType.Melee, 5 },
            { StatType.Distance, 5 },
            { StatType.Defense, 0 },
            { StatType.Speed, DefaultSpeed },
            { StatType.CriticalRate, 0.1f },
            { StatType.CriticalMultiplier, 1.5f },
            { StatType.BlockRate, 0.1f },
            { StatType.BlockReduction, 0.25f },
            { StatType.AttackRate, 1f }
        };

        private static readonly Dictionary<StatType, double> StatUpgrades = new()
        {
            { StatType.MaxHealth, 1f },
            { StatType.MaxMana, 0.5f },
            { StatType.Magic, 3 },
            { StatType.Melee, 3 },
            { StatType.Distance, 3 },
            { StatType.Defense, 1 },
            { StatType.Speed, 0.015f }
        };

        private static readonly Dictionary<StatType, double> StatCap = new()
        {
            { StatType.BlockReduction, 0.9f },
            { StatType.MagicalResistance, 0.9f },
            { StatType.DamageResistance, 0.9f },
            { StatType.MeleeResistance, 0.9f },
            { StatType.DistanceResistance, 0.9f },
            { StatType.Speed, 10f }
        };

        public PlayerBehavior player;
        public override EntityBehavior Entity => player;

        public override double GetStat(StatType stat)
        {
            var defaultValue = DefaultStats.TryGetValue(stat, out var value) ? value : default;
            if (StatUpgrades.TryGetValue(stat, out var upgrade))
                defaultValue *= 1 + (player.Data.level - 1 ) * upgrade * LevelMultiplier;
            defaultValue += player.gear.GetStatAbsolute(stat);

            if (Stat.IsAbsolute(stat))
            {
                defaultValue *= 1 + player.gear.GetStatMultiplier(stat);
                defaultValue *= player.Data.GetRebirthMultiplier();
            }
            else
                defaultValue += player.gear.GetStatMultiplier(stat);

            switch (stat)
            {
                case StatType.MaxHealth:
                    defaultValue *= player.Data.masteries.endurance.GetMultiplier();
                    break;
                case StatType.Defense:
                    defaultValue *= player.Data.masteries.defense.GetMultiplier();
                    break;
                case StatType.Melee:
                    defaultValue *= player.Data.masteries.melee.GetMultiplier();
                    break;
                case StatType.Distance:
                    defaultValue *= player.Data.masteries.distance.GetMultiplier();
                    break;
                case StatType.Magic:
                    defaultValue *= player.Data.masteries.magic.GetMultiplier();
                    break;
                default:
                    break;
            }

            if (StatCap.TryGetValue(stat, out var cap))
                defaultValue = Math.Min(defaultValue, cap);

            return defaultValue;
        }
    }
}
