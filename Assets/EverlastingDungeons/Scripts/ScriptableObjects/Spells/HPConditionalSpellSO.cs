using Assets.EverlastingDungeons.Scripts.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.ScriptableObjects.Spells
{

    [CreateAssetMenu(fileName = "New HP Conditional Spell", menuName = "Everlasting Dungeons/Spell/HP Conditional", order = 1)]
    public class HPConditionalSpellSO : SpellSO
    {
        [Range(0f, 1f)]
        public double requiredHpPercent;
        public bool aboveHp = true;

        protected override bool Run(EntityBehavior entity)
        {
            var target = entity.AttackHandler.Target;
            var currentHpPercent = target.health / target.ModdedStats.GetStat(Utility.StatType.MaxHealth);
            if ((aboveHp && requiredHpPercent > currentHpPercent) || (!aboveHp && currentHpPercent > requiredHpPercent))
                return false;

            target.TakeDamage(new Utility.Hit(entity, target, new Utility.HitOptions { damageMultiplier = damageMultiplier }));
            return true;
        }
    }
}
