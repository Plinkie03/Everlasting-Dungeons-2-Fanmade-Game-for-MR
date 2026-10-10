using Assets.EverlastingDungeons.Scripts.Entity.Monster;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Entity.Base
{
    public class EntityAttackHandler : MonoBehaviour
    {
        private static readonly string BasicAttackId = System.Guid.NewGuid().ToString();

        public EntityBehavior entity;
        private EntityBehavior target;
        public Dictionary<string, long> cooldowns = new();

        public virtual void SetTarget(EntityBehavior entity)
        {
            if (entity != null && entity.IsDead)
                return;

            this.target = entity;
        }

        public bool CanAttack(EntityBehavior entity) => entity.GetType() != this.entity.GetType();

        public void SetCooldown(string id, long cd)
        {
            if (cooldowns.TryGetValue(id, out long value))
            {
                cooldowns[id] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + cd;
            }
            else
            {
                cooldowns.Add(id, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + cd);
            }
        }

        public long GetCooldownTime(string id)
        {
            if (!cooldowns.TryGetValue(id, out long value))
            {
                return 0;
            }
            else
            {
                return Math.Max(0, value - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            }
        }

        public bool HasCooldown(string id) => cooldowns.TryGetValue(id, out long value) && value > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        public EntityBehavior Target
        {
            get
            {
                {
                    try
                    {
                        if (target.IsDead)
                        {
                            target = null;
                        }

                        return target;
                    }
                    catch
                    {
                        target = null;
                        return target;
                    }
                }
            }
        }

        public bool IsTargetInRange => Target != null && Vector3.Distance(entity.transform.position, Target.transform.position) <= entity.GetClass().attackRange;

        public virtual bool BasicAttack()
        {
            if (!IsTargetInRange || HasCooldown(BasicAttackId))
                return false;

            SetCooldown(BasicAttackId, (long)(entity.ModdedStats.GetStat(StatType.AttackRate) * 1000f));

            var hit = new Hit(entity, target);

            target.TakeDamage(hit);

            return true;
        }

        public void FixedUpdate()
        {
            BasicAttack();
        }
    }
}
