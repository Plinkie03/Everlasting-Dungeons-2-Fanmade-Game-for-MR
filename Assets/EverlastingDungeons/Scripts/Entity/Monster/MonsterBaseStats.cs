using Assets.EverlastingDungeons.Scripts.Entity.Base;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.EverlastingDungeons.Scripts.Entity.Monster
{
    public class MonsterBaseStats : EntityBaseStats
    {
        public MonsterBehavior monster;
        public override EntityBehavior Entity => monster;
        public override double GetStat(StatType stat)
        {
            var got = monster.data.stats.FirstOrDefault(x => x.type == stat);
            if (got != null) return got.value;
            return default;
        }
    }
}
