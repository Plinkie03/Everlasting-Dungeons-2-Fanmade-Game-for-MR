using Assets.EverlastingDungeons.Scripts.Entity.Base;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.ScriptableObjects.Spells
{
    [CreateAssetMenu(fileName = "New Multi-Target Spell", menuName = "Everlasting Dungeons/Spell/Multi-Target", order = 1)]
    public class MultiTargetSO : SpellSO
    {
        public float maxDistance;
        public long maxTargets;

        protected override bool Run(EntityBehavior entity)
        {
            long i = 0;
            var targets = FindObjectsByType<EntityBehavior>().Where(x => x.AttackHandler.CanAttack(entity) && Vector2.Distance(entity.transform.position, x.transform.position) < maxDistance);
            if (!targets.Any()) return false;

            foreach (var target in targets)
            {
                if (i == maxTargets)
                    break;
                var hit = new Hit(entity, target, new HitOptions { damageMultiplier = damageMultiplier });
                target.TakeDamage(hit);
                i++;
            }
            return true;
        }
    }
}
