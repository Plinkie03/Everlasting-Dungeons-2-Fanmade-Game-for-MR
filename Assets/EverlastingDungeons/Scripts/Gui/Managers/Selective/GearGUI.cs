using Assets.EverlastingDungeons.Scripts.Gui.Objects;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Gui.Managers.Selective
{
    public class GearGUI : BaseSelectiveGUI
    {
        public GearSlotGUI[] slots;
        public Transform itemContent;
        public ItemInfoGUI itemInfoGUI;
        public ItemInfoGUI otherItemInfoGUI;
        public GearType? selectedGearType;

        private void Start()
        {
        }

        public override void Refresh()
        {
            Clear();

            foreach (var slot in slots)
            {
                var itm = PrefabManager.Instance.player.Data.items.FirstOrDefault(x => x.equipped && x.Item is GearItemSO gear && gear.gearType == slot.type);
                slot.SetItem(itm);
            }

            if (selectedGearType.HasValue)
            {
                foreach (var item in PrefabManager.Instance.activePlayer.Data.items)
                {
                    if (!item.equipped && item.Item is GearItemSO gear && gear.gearType == selectedGearType.Value)
                    {
                        var slot = Instantiate(PrefabManager.Instance.itemSlot, itemContent);
                        slot.itemInfo = otherItemInfoGUI;
                        slot.SetItem(item);
                    }
                }
            }
        }

        private void OnDisable()
        {
            itemInfoGUI.panel.gameObject.SetActive(false);
            otherItemInfoGUI.panel.gameObject.SetActive(false);
            selectedGearType = null;
        }

        private void Clear()
        {
            for (int i = 0;i < itemContent.childCount;i++)
                Destroy(itemContent.GetChild(i).gameObject);
        }
    }
}
