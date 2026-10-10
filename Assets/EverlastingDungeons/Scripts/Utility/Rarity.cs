using Assets.EverlastingDungeons.Scripts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.EverlastingDungeons.Scripts.Utility
{
    public enum RarityType
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythical,
        Godly,
        Chaotic,
        Eternal,
        Champion,
        Ascendant, 
        Celestial, 
        Transcendent, 
        Infinite
    }

    public class Rarity
    {
        private static readonly Rarity[] RarityArray = new Rarity[]
        {
            new Rarity(RarityType.Infinite, 0.00000001d, 120f, 180f),
            new Rarity(RarityType.Transcendent, 0.0000001d, 60f, 90f),
            new Rarity(RarityType.Celestial, 0.000001d, 40f, 50f),
            new Rarity(RarityType.Ascendant, 0.00001d, 20f, 25f),
            new Rarity(RarityType.Champion, 0.0001d, 10f, 15f),
            new Rarity(RarityType.Eternal, 0.001d, 4f, 5f),
            new Rarity(RarityType.Chaotic, 0.0025d, 3f, 4f),
            new Rarity(RarityType.Godly, 0.005d, 2.5f, 3f),
            new Rarity(RarityType.Mythical, 0.01d, 2.2f, 2.5f),
            new Rarity(RarityType.Legendary, 0.02f, 1.8f, 2.2f),
            new Rarity(RarityType.Epic, 0.03f, 1.5f, 1.8f),
            new Rarity(RarityType.Rare, 0.04f, 1.2f, 1.5f),
            new Rarity(RarityType.Uncommon, 0.05f, 1f, 1.2f),
            new Rarity(RarityType.Common, 1f, 0.8f, 1f),
        };
        private static readonly Dictionary<RarityType, Rarity> RarityDictionary = RarityArray.ToDictionary(r => r.type, r => r);

        public readonly RarityType type;
        public readonly double chance;
        public readonly (float, float) multiplier;
        public string Color(string txt) => $"<gradient={type}>{txt}</gradient>";

        private Rarity(RarityType type, double chance, float min, float max)
        {
            this.chance = chance;
            this.type = type;
            multiplier = (min, max);
        }

        public long AdditionalStatCount => 1 + (long)type;
        public float RandomMultiplier => UnityEngine.Random.Range(multiplier.Item1, multiplier.Item2);
        public float MinMultiplier => multiplier.Item1;
        public float MaxMultiplier => multiplier.Item2;

        public static Rarity Get(RarityType type) => RarityDictionary[type];

        public static Rarity GetRandomRarity(double mult) => RarityArray.First(r => (r.chance * mult).IsChance());
    }
}
