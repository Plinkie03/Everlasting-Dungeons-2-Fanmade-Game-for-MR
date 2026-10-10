using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Gui.Objects
{
    public class ConsumableHotkeySlotGUI : BaseHotkeySlotGUI<PlayerInventoryItem>
    {
        public TextMeshProUGUI amount;

        public ConsumableItemSO Item => Element.Item is ConsumableItemSO consumable ? consumable : null;
        public override PlayerInventoryItem Element => PrefabManager.Instance.activePlayer.Data.GetItemByIndex(index);

        protected override Sprite Sprite => Item.sprite;
        protected override long TimeLeft => PrefabManager.Instance.activePlayer.AttackHandler.GetCooldownTime(Element.itemUUID);
        protected override bool IsAvailable => Item.CanUse(PrefabManager.Instance.activePlayer, Element);
        protected override bool IsAssigned => GUIManager.Instance.selective.consumableAssignment.selectedHotkeyIndex == index;
        protected override void OnClick()
        {
            if (!selected)
            {
                if (Item)
                    Item.Use(PrefabManager.Instance.activePlayer, Element);
            }
            else
            {
                GUIManager.Instance.selective.consumableAssignment.selectedHotkeyIndex = index;
                GUIManager.Instance.selective.consumableAssignment.selectedUUID = Element?.itemUUID;
            }
        }

        protected override void UpdateData()
        {
            base.UpdateData();
            if (amount)
            {
                amount.gameObject.SetActive(Element != null);
                if (Element != null)
                    amount.text = Element.amount == 1 ? string.Empty : Element.amount.Format();
            }
        }
    }
}
