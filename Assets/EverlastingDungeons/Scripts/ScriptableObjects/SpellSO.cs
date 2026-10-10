using Assets.EverlastingDungeons.Scripts.Entity.Base;
using Assets.EverlastingDungeons.Scripts.Entity.Monster;
using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriInspector;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Basic Spell", menuName = "Everlasting Dungeons/Basic Spell")]
    public class SpellSO : BaseSO<SpellSO>
    {
        public Sprite sprite;
        public string description;
        public bool aimTarget;
        public bool useShape;
        [Range(0f, 1f)]
        public double chance;
        [ShowIf(nameof(useShape), true)] public ShapeType shape;
        public long cooldown;
        public float manaCost;
        public float damageMultiplier = 1f;
        public EntityClassType classType;
        public bool canPlayerUse = false;
        [ShowIf(nameof(canPlayerUse), true)]
        public bool unlockedByDefault = false;

        public bool CanEquip(EntityBehavior entity) => entity is MonsterBehavior || (entity is PlayerBehavior player && canPlayerUse && classType == player.GetClass().classType);
        public bool CanExecute(EntityBehavior entity)
        {
            if (entity is PlayerBehavior p && p.mana < manaCost)
                return false;

            if (entity is MonsterBehavior && !entity.AttackHandler.IsTargetInRange)
                return false;

            if (aimTarget && !entity.AttackHandler.IsTargetInRange)
                return false;

            if (entity.AttackHandler.HasCooldown(uuid))
                return false;

            return true;
        }

        public bool Execute(EntityBehavior entity)
        {
            if (!CanEquip(entity) || !CanExecute(entity))
                return false;

            if (useShape)
            {
                var shape = CachedReflection.GetStaticValue<ShapeBuilder>(typeof(Shapes), this.shape.ToString());
                var entities = shape.Draw(EntityClass.GetMarkerOf(entity.GetClass().classType), aimTarget ? entity.AttackHandler.Target.transform.position : entity.transform.position, entity.animator.Animation)
                    .Where(x => x.AttackHandler.CanAttack(entity));
                foreach (var hitEntity in entities)
                {
                    var hit = new Hit(entity, hitEntity, new HitOptions { damageMultiplier = damageMultiplier, disableAnimations = true });
                    hitEntity.TakeDamage(hit);
                }

                LastStep(entity);
            }
            else
            {
                if (Run(entity))
                {
                    LastStep(entity);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void LastStep(EntityBehavior entity)
        {
            PrefabManager.Instance.CreateFloatingText(entity.transform.position + Vector3.up * entity.GetTextOffset(), name, Color.lightBlue, 1.2f);
            entity.AttackHandler.SetCooldown(uuid, cooldown);
            if (entity is PlayerBehavior p)
            {
                p.mana -= manaCost;
                p.PlayRandomFrom(p.specialAttackSounds);
            }
        }

        protected virtual bool Run(EntityBehavior entity)
        {
            return true;
        }
    }
}
