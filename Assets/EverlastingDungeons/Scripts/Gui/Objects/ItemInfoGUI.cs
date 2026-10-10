using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.EverlastingDungeons.Scripts.Gui.Objects
{
    public class ItemInfoGUI : BaseItemInfoGUI
    {
        public ItemSlotGUI itemSlot;
        public Transform panel;
        public Button destroy;
        public Button favorite;
        public Button equip;
        private bool confirmDestroy;
        public PlayerInventoryItem currentItem;
        private HashSet<Transform> unclearable = new();

        private void Awake()
        {
            displayPrice = false;
            
            for (int i = 0;i < content.childCount; i++)
                unclearable.Add(content.GetChild(i));
        }

        private void Start()
        {
            if (favorite) favorite.onClick.AddListener(OnClickFavorite);
            if (destroy) destroy.onClick.AddListener(OnClickDestroy);
            if (equip) equip.onClick.AddListener(OnClickEquip);
        }

        public void OnClickEquip()
        {
            if (this.currentItem == null)
                return;

            var currentItem = this.currentItem;
            if (currentItem.Item is GearItemSO gear)
            {
                var equipped = PrefabManager.Instance.activePlayer.gear.GetEquippedPiece(gear.gearType);
                currentItem.equipped = true;
                if (equipped != null)
                {
                    equipped.equipped = false;
                    SetItem(equipped);
                }
                else
                    panel.gameObject.SetActive(false);
                
                GUIManager.Instance.selective.gear.itemInfoGUI.SetItem(currentItem);
                GUIManager.Instance.selective.gear.Refresh();
            }
        }

        private void OnClickFavorite()
        {
            currentItem.locked = !currentItem.locked;
            ResetButtonData();
            GUIManager.Instance.selective.inventory.Refresh();
            itemSlot.Refresh();
        }

        private void OnClickDestroy()
        {
            if (!confirmDestroy)
            {
                destroy.GetComponentInChildren<TextMeshProUGUI>().text = "Confirm";
                confirmDestroy = true;
            }
            else
            {
                PrefabManager.Instance.activePlayer.Data.items.Remove(currentItem);
                currentItem = null;
                GUIManager.Instance.selective.inventory.Refresh();
                panel.gameObject.SetActive(false);
            }
        }

        public override void SetItem(PlayerInventoryItem inventoryItem)
        {
            currentItem = inventoryItem;
            ClearStats();
            base.SetItem(inventoryItem);
            ResetButtonData();
            if (itemSlot)
                itemSlot.SetItem(inventoryItem);
            panel.gameObject.SetActive(true);
        }

        private void ResetButtonData()
        {
            if (destroy && favorite && currentItem != null)
            {
                confirmDestroy = false;
                destroy.GetComponentInChildren<TextMeshProUGUI>().text = "Destroy";
                destroy.interactable = !currentItem.locked && !currentItem.equipped;
                favorite.GetComponentInChildren<TextMeshProUGUI>().text = $"Favorite{(currentItem.locked ? "d" : string.Empty)}";
            }
        }

        private void OnEnable()
        {
            ResetButtonData();
        }

        private void ClearStats()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                var child = content.GetChild(i);
                if (unclearable.Contains(child))
                    continue;
                Destroy(child.gameObject);
            }
        }
    }
}
