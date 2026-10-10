using Assets.EverlastingDungeons.Scripts.Entity.Base;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Entity.Player
{
    public class PlayerAttackHandler : EntityAttackHandler
    {
        private SpriteRenderer selector;

        public override void SetTarget(EntityBehavior entity)
        {
            if (selector)
            {
                Destroy(selector.gameObject);
                selector = null;
            }

            base.SetTarget(entity);

            if (Target)
            {
                selector = Instantiate(PrefabManager.Instance.targetSelector, Target.transform);
            }
        }

        public override bool BasicAttack()
        {
            var result = base.BasicAttack();
            if (result)
                entity.PlayRandomFrom(entity.basicHitSounds);
            return result;
        }
    }
}
