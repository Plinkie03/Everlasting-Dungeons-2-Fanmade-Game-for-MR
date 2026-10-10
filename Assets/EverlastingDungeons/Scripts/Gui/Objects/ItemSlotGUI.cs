using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.EverlastingDungeons.Scripts.Gui.Objects
{
    public class ItemSlotGUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Button button;
        public TextMeshProUGUI amount;
        public Image icon;
        public ItemInfoGUI itemInfo;
        public Image selected;
        public TextMeshProUGUI equipped;
        public bool hideSelected;
        public bool hideEquipped;
        private FloatingItemInfoGUI floatingInfo;
        private PlayerInventoryItem item;

        public void Start()
        {
            UpdateData();
            button.onClick.AddListener(() =>
            {
                if (itemInfo)
                {
                    if (item == null)
                    {
                        itemInfo.currentItem = null;
                        itemInfo.panel.gameObject.SetActive(false);
                    }
                    else
                    {
                        itemInfo.SetItem(item);
                    }
                }
                else if (GUIManager.Instance.selective.Current == GUIManager.Instance.selective.consumableAssignment)
                {
                    var current = PrefabManager.Instance.activePlayer.Data.GetItemByIndex(GUIManager.Instance.selective.consumableAssignment.selectedHotkeyIndex);
                    if (current != null)
                        current.index = -1;
                    InventoryItem.index = GUIManager.Instance.selective.consumableAssignment.selectedHotkeyIndex;
                    GUIManager.Instance.selective.consumableAssignment.selectedUUID = InventoryItem.itemUUID;
                }
                else if (GUIManager.Instance.selective.Current == GUIManager.Instance.selective.buy)
                {
                    GUIManager.Instance.selective.buy.SetItem(item);
                }
            });
        }

        public virtual void SetItem(PlayerInventoryItem item)
        {
            this.item = item;
            Refresh();
        }

        public virtual void Refresh()
        {
            if (item == null)
                return;
            icon.sprite = InventoryItem.Item.sprite;
            icon.color = InventoryItem.SpriteColor;
            amount.text = InventoryItem.amount > 1 ? InventoryItem.amount.Format() : string.Empty;
            button.GetComponent<Image>().color = InventoryItem.locked ? Color.yellow : Color.white;
        }

        public PlayerInventoryItem InventoryItem => item;

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (floatingInfo)
                Destroy(floatingInfo.gameObject);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (item == null)
                return;
            floatingInfo = PrefabManager.Instance.CreateFloatingItemInfo(GUIManager.Instance.selective.Current.transform, InventoryItem);
        }

        private void FixedUpdate()
        {
            UpdateData();
        }

        protected virtual void UpdateData()
        {
            equipped.gameObject.SetActive(item != null && !hideEquipped && InventoryItem.equipped);
            var state = false;
            if (!hideSelected && item != null)
            {
                if (itemInfo)
                    state = itemInfo.currentItem == InventoryItem;
                else if (GUIManager.Instance.selective.Current == GUIManager.Instance.selective.consumableAssignment)
                    state = GUIManager.Instance.selective.consumableAssignment.selectedUUID == InventoryItem.itemUUID;
                else if (GUIManager.Instance.selective.Current == GUIManager.Instance.selective.buy)
                    state = GUIManager.Instance.selective.buy.selectedUUID == InventoryItem.itemUUID;
            }
            selected.gameObject.SetActive(state);
        }
    }
}
