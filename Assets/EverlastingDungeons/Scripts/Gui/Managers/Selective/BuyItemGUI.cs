using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Gui.Objects;
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

namespace Assets.EverlastingDungeons.Scripts.Gui.Managers.Selective
{
    public class BuyItemGUI : BaseSelectiveGUI
    {
        public Transform itemContent;
        public ItemInfoGUI itemInfo;
        public Button buy;
        public Slider slider;
        public TextMeshProUGUI price;
        public TextMeshProUGUI counter;
        public BaseItemSO[] items;
        public string selectedUUID;
        public BaseItemSO SelectedItem => selectedUUID == null ? default : ResourceManager.Instance.items.Get(selectedUUID);

        private void Start()
        {
            buy.onClick.AddListener(OnBuy);
            slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(float newValue)
        {
            counter.text = newValue.Format();
            price.text = Price.Format();
        }

        private void OnBuy()
        {
            var amount = (long)slider.value;
            PrefabManager.Instance.activePlayer.Data.money -= Price;
            PrefabManager.Instance.activePlayer.Data.AddItem(new(selectedUUID, amount: amount));
            PrefabManager.Instance.activePlayer.buySound.Play();
            OnDisable();
        }

        public double Price => SelectedItem.price * Math.Round(slider.value);
        public void SetItem(PlayerInventoryItem item)
        {
            itemInfo.SetItem(item);
            selectedUUID = item.itemUUID;
            slider.value = 1;
            slider.maxValue = (float)Math.Max(Math.Floor(PrefabManager.Instance.activePlayer.Data.money / SelectedItem.price), 1);
            slider.gameObject.SetActive(item.Item.IsStackable);
            OnValueChanged(slider.value);
        }

        private void OnDisable()
        {
            itemInfo.panel.gameObject.SetActive(false);
            itemInfo.currentItem = null;
            selectedUUID = null;
            slider.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (selectedUUID != null)
            {
                buy.interactable = slider.value != 0 && PrefabManager.Instance.activePlayer.Data.money >= Price;
            }
        }

        public override void Refresh()
        {
            Clear();

            foreach (var item in items)
            {
                var slot = Instantiate(PrefabManager.Instance.itemSlot, itemContent);
                slot.SetItem(new(item.uuid));
            }
        }

        private void Clear()
        {
            for (int i = 0;i < itemContent.childCount;i++) 
                Destroy(itemContent.GetChild(i).gameObject);
        }
    }
}
