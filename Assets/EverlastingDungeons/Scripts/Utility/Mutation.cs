using Assets.EverlastingDungeons.Scripts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Utility
{
    public enum MutationType
    {
        None,
        Raging,
        Feeble,
        Elite,
        Swift,
        Magical,
        Ranged,
        Godlike,
        Champion
    }

    public class Mutation
    {
        private readonly static Mutation[] MutationsArray =
        {
            new(MutationType.Champion, 0.01f, 3.5f, 50f, 25f, new List<Stat> {
                Stat.Multiplier(StatType.MaxHealth, 100f),
                Stat.Multiplier(StatType.Distance, 10f),
                Stat.Multiplier(StatType.Speed, 0.5f),
                Stat.Multiplier(StatType.Defense, 0f),
                Stat.Multiplier(StatType.Melee, 10f),
                Stat.Multiplier(StatType.Magic, 10f),
            }, rarityMultiplier: 25f),
            new(MutationType.Godlike, 0.05f, 1.66f, 5f, 2.5f, new List<Stat> {
                Stat.Multiplier(StatType.MaxHealth, 6f),
                Stat.Multiplier(StatType.Distance, 3f),
                Stat.Multiplier(StatType.Speed, 0.66f),
                Stat.Multiplier(StatType.Defense, 1.66f),
                Stat.Multiplier(StatType.Melee, 3f),
                Stat.Multiplier(StatType.Magic, 3f),
            }, rarityMultiplier: 6f),
            new(MutationType.Elite, 0.05f, 1.33f, 3f, 1.75f, new List<Stat> {
                Stat.Multiplier(StatType.MaxHealth, 3f),
                Stat.Multiplier(StatType.Distance, 2f),
                Stat.Multiplier(StatType.Defense, 1.25f),
                Stat.Multiplier(StatType.Melee, 2f),
                Stat.Multiplier(StatType.Magic, 2f),
            }, rarityMultiplier: 4f),
            new(MutationType.Feeble, 0.1f, 0.66f, 1.75f, 1.25f, new List<Stat> {
                Stat.Multiplier(StatType.MaxHealth, 0.5f),
                Stat.Multiplier(StatType.Speed, 0.8f),
                Stat.Multiplier(StatType.Distance, 0.5f),
                Stat.Multiplier(StatType.Defense, 0.5f),
                Stat.Multiplier(StatType.Melee, 0.5f),
                Stat.Multiplier(StatType.Magic, 0.5f),
            }, rarityMultiplier: 2f),
            new(MutationType.Raging, 0.1f, 1f, 1f, 1f, new List<Stat> {
                Stat.Multiplier(StatType.AttackRate, 0.7f),
                Stat.Multiplier(StatType.Melee, 1.33f)
            }, Color.red, EntityClassType.Warrior),
            new(MutationType.Ranged, 0.1f, 1f, 1f, 1f, new List<Stat> {
                Stat.Multiplier(StatType.Distance, 1.33f),
                Stat.Multiplier(StatType.Defense, 0.7f)
            }, Color.green, EntityClassType.Archer),
            new(MutationType.Magical, 0.1f, 1f, 1f, 1f, new List<Stat> {
                Stat.Multiplier(StatType.Magic, 1.33f),
                Stat.Multiplier(StatType.Defense, 0.7f)
            }, Color.magenta, EntityClassType.Mage),
            new(MutationType.Swift, 0.1f, 0.75f, 1f, 1f, new List<Stat> {
                Stat.Multiplier(StatType.Speed, 2f)
            }),
            new(MutationType.None, 1f, 1f, 1f, 1f, new List<Stat> {}),
        };

        private static readonly Dictionary<MutationType, Mutation> MutationsDict = MutationsArray.ToDictionary(m => m.type);

        public readonly MutationType type;
        public readonly double chance;
        public readonly List<Stat> statMultipliers;
        public readonly double scaleMultiplier;
        public readonly double rewardMultiplier;
        public readonly double dropMultiplier;
        public readonly EntityClassType? classType;
        public readonly Color color;
        public readonly double rarityMultiplier;

        private Mutation(MutationType type, double chance, double scaleMultiplier, double rewardMultiplier, double dropMultiplier, List<Stat> statMultipliers, Color? color = null, EntityClassType? classType = null, float rarityMultiplier = 1f)
        {
            this.classType = classType;
            this.rarityMultiplier = rarityMultiplier;
            this.color = color.GetValueOrDefault(Color.white);
            this.type = type;
            this.chance = chance;
            this.statMultipliers = statMultipliers;
            this.scaleMultiplier = scaleMultiplier;
            this.rewardMultiplier = rewardMultiplier;
            this.dropMultiplier = dropMultiplier;
        }

        public static Mutation Get(MutationType type) => MutationsDict[type];
        public static Mutation RandomMutation => MutationsArray.First(m => m.chance.IsChance());
    }
}
