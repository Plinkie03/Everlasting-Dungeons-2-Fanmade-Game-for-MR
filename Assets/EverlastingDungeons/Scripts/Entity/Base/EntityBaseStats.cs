using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Entity.Base
{
    public abstract class EntityBaseStats : MonoBehaviour
    {
        public static readonly double DefaultSpeed = 3f;

        public abstract EntityBehavior Entity { get; }

        public abstract double GetStat(StatType stat);

        public double GetStrength() => GetStat(Entity.GetClass().CombatStat);

        public double GetResistanceAgainst(EntityBehavior entity) => entity.GetClass().classType switch
        {
            EntityClassType.Warrior => GetStat(StatType.MeleeResistance),
            EntityClassType.Archer => GetStat(StatType.DistanceResistance),
            EntityClassType.Mage => GetStat(StatType.MagicalResistance),
            _ => default,
        };
    }
}
