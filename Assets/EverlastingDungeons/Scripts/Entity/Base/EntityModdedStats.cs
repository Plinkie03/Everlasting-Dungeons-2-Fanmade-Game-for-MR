using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.EverlastingDungeons.Scripts.Entity.Base
{
    public abstract class EntityModdedStats : EntityBaseStats
    {
        public static readonly (float, float) DamageBounds = (0.8f, 1.2f);

        public override double GetStat(StatType stat)
        {
            var clazz = Entity.GetClass();
            var value = Entity.BaseStats.GetStat(stat);
            if (Stat.IsCombatStat(stat))
                value *= clazz.damageMultiplier;
            else if (stat is StatType.Defense)
                value *= clazz.defenseMultiplier;
            else if (stat is StatType.MaxHealth)
                value *= clazz.healthMultiplier;
            if (Stat.IsAbsolute(stat))
                value = Math.Round(value);
            return value;
        }

        public double GetRandomDamage() => GetStrength() * (double)UnityEngine.Random.Range(DamageBounds.Item1, DamageBounds.Item2);
    }
}
