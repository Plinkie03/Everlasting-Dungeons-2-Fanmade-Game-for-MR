using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Utility
{
    public enum AuraType
    {
        None,
        Crimson,
        Chaos
    }

    public class Aura
    {
        private static readonly Aura[] All = {
            new(AuraType.None, 1d, Color.white, 1),
            new(AuraType.Crimson, 2d, Color.red, 0.25d),
            new(AuraType.Chaos, 3.5d, Color.magenta, 0.1d)
        };
        private static readonly Dictionary<AuraType, Aura> AllKeyed = All.ToDictionary(x => x.type);
        
        public static readonly double AuraModulo = 100;

        public readonly AuraType type;
        public readonly double exponent;
        public readonly Color color;
        public readonly double dropChance;

        private Aura(AuraType type, double exponent, Color color, double dropChance)
        {
            this.type = type;
            this.dropChance = dropChance;
            this.color = color;
            this.exponent = exponent;
        }

        public static Aura GetAuraForLevel(double level) => All.ElementAtOrDefault((int)Math.Floor(level / AuraModulo)) ?? All[All.Length - 1];

        public static double GetFloorLevel(double currentFloor) => currentFloor % AuraModulo;
    }
}