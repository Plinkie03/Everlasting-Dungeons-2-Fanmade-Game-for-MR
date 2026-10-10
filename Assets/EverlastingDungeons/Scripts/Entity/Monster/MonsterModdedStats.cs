using Assets.EverlastingDungeons.Scripts.Entity.Base;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Assets.EverlastingDungeons.Scripts.Entity.Monster
{
    public class MonsterModdedStats : EntityModdedStats
    {
        public MonsterBehavior monster;
        public override EntityBehavior Entity => monster;

        public override double GetStat(StatType stat)
        {
            var mult = monster.mutation.statMultipliers.FirstOrDefault(x => x.type == stat);
            var value = base.GetStat(stat) * (mult == null ? 1f : mult.value);
            if (Stat.IsAbsolute(stat))
            {
                value *= monster.level;
                value = Math.Pow(value, monster.Aura.exponent);
            }

            // REMOVE DMG RES IF NOT THE REAL BOSS
            if (stat is StatType.DamageResistance && !monster.allowedLadderSpawn)
                value = 0;

            return Stat.IsAbsolute(stat) ? Math.Floor(value) : value;
        }
    }
}
