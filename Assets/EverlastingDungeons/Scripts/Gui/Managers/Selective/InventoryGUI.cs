using Assets.EverlastingDungeons.Scripts.Gui.Objects;
using Assets.EverlastingDungeons.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Gui.Managers.Selective
{
    public class InventoryGUI : BaseSelectiveGUI
    {
        public Transform itemContent;
        public ItemInfoGUI itemInfo;

        private void OnDisable()
        {
            itemInfo.panel.gameObject.SetActive(false);
            itemInfo.currentItem = null;
        }

        public override void Refresh()
        {
            Clear();
            for (int i = 0;i < PrefabManager.Instance.activePlayer.Data.items.Count; i++)
            {
                var slot = Instantiate(PrefabManager.Instance.itemSlot, itemContent);
                slot.itemInfo = itemInfo;
                slot.SetItem(PrefabManager.Instance.activePlayer.Data.items[i]);
            }
        }

        private void Clear()
        {
            for (int i = 0; i < itemContent.childCount; i++)
                Destroy(itemContent.GetChild(i).gameObject);
        }
    }
}
