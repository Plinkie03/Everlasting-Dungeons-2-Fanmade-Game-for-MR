using Assets.EverlastingDungeons.Scripts.Entity.Base;
using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Gui.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.ScriptableObjects.Consumables
{
    [CreateAssetMenu(menuName = "Everlasting Dungeons/Items/Consumable/Potion", fileName = "New Potion")]
    public class PotionItemSO : ConsumableItemSO
    {
        public double manaRegen;
        public double healthRegen;

        public override bool IsStackable => true;
        public override bool IsSellable => true;

        protected override bool Run(EntityBehavior entity, PlayerInventoryItem item)
        {
            entity.Heal(healthRegen);
            entity.HealMana(manaRegen);
            return true;
        }

        public override void FillInfo(BaseItemInfoGUI info, PlayerInventoryItem item)
        {
            if (healthRegen != 0)
                info.AddText($"+{healthRegen.Format()} HP", Color.green);
            if (manaRegen != 0)
                info.AddText($"+{manaRegen.Format()} MP", Color.blue);
        }
    }
}
