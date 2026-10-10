using Assets.EverlastingDungeons.Scripts.Entity.Base;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.ScriptableObjects.Spells
{
    [CreateAssetMenu(fileName = "New Volley Spell", menuName = "Everlasting Dungeons/Spell/Volley", order = 1)]
    public class VolleySpellSO : SpellSO
    {
        public int projectileCount;
        public float projectileDelay;

        protected override bool Run(EntityBehavior entity)
        {
            entity.StartCoroutine(RunProjectiles(entity));
            return true;
        }

        private IEnumerator RunProjectiles(EntityBehavior entity)
        {
            for (int i =0;i < projectileCount && !entity.IsDead && entity.AttackHandler.Target != null;i++)
            {
                var hit = new Hit(entity, entity.AttackHandler.Target, new HitOptions { damageMultiplier = damageMultiplier });
                hit.target.TakeDamage(hit);
                yield return new WaitForSeconds(projectileDelay);
            }
        }
    }
}
