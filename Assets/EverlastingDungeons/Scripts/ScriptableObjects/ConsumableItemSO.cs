using Assets.EverlastingDungeons.Scripts.Entity.Base;
using Assets.EverlastingDungeons.Scripts.Entity.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.EverlastingDungeons.Scripts.ScriptableObjects
{
    public abstract class ConsumableItemSO : BaseItemSO
    {
        public bool consume = true;
        public long cooldown;
        public virtual bool CanUse(EntityBehavior entity, PlayerInventoryItem item) => !entity.AttackHandler.HasCooldown(uuid);
        public bool Use(EntityBehavior entity, PlayerInventoryItem item)
        {
            if (!CanUse(entity, item)) return false;

            if (Run(entity, item))
            {
                if (entity is PlayerBehavior p)
                    p.PlayRandomFrom(p.potSounds);
                entity.AttackHandler.SetCooldown(uuid, cooldown);
                if (consume)
                    item.SubAmount(1);
                return true;
            }
            else
            {
                return false;
            }
        }
        protected abstract bool Run(EntityBehavior entity, PlayerInventoryItem item);
    }
}
