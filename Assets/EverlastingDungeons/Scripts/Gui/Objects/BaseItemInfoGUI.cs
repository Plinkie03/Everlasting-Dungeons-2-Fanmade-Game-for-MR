using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Gui.Objects
{
    public class BaseItemInfoGUI : MonoBehaviour
    {
        public Transform itemName;
        public Transform content;
        public int startIndex;
        public bool displayPrice = true;

        public virtual void SetItem(PlayerInventoryItem inventoryItem)
        {
            var txt = itemName.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = inventoryItem.DisplayName;

            inventoryItem.Item.FillInfo(this, inventoryItem);

            var lv = Instantiate(PrefabManager.Instance.itemLevelSlot, content);
            lv.GetComponentInChildren<TextMeshProUGUI>().text = $"Level {inventoryItem.level.Format()}";
            lv.SetSiblingIndex(startIndex);

            if (displayPrice)
            {
                var price = Instantiate(PrefabManager.Instance.itemPriceSlot, content);
                price.GetComponentInChildren<TextMeshProUGUI>().text = inventoryItem.Price.Format();
            }
        }

        public void AddText(string text, Color color)
        {
            var obj = Instantiate(PrefabManager.Instance.itemStatSlot, content).GetComponentInChildren<TextMeshProUGUI>();
            obj.transform.parent.SetSiblingIndex(startIndex);
            obj.text = text;
            obj.color = color;
        }
    }
}
