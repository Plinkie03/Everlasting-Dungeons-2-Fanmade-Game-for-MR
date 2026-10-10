using Assets.EverlastingDungeons.Scripts.Entity.Base;
using Assets.EverlastingDungeons.Scripts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.EverlastingDungeons.Scripts.Entity.Monster
{
    public class MonsterAttackHandler : EntityAttackHandler
    {
        public MonsterBehavior monster;

        public new void FixedUpdate()
        {
            base.FixedUpdate();

            if (Target && monster.data.spells.Count != 0)
            {
                foreach (var spell in monster.data.spells)
                {
                    if (spell.chance.IsChance() && spell.Execute(monster))
                        break;
                }
            }
        }
    }
}
