using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Gui.Objects;
using Assets.EverlastingDungeons.Scripts.Managers;
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
    public class SellItemGUI : BaseSelectiveGUI
    {
        public Transform content;
        public Button sell;
        public TextMeshProUGUI price;
        public Button selectAll;
        public Button reset;
        public List<PlayerInventoryItem> selected = new();

        private void Start()
        {
            sell.onClick.AddListener(OnSell);
            reset.onClick.AddListener(Refresh);
            selectAll.onClick.AddListener(OnSelectAll);
        }

        private void OnSelectAll()
        {
            foreach (var slot in content.GetComponentsInChildren<SellItemSlotGUI>())
            {
                if (!selected.Contains(slot.InventoryItem))
                    selected.Add(slot.InventoryItem);
            }
            UpdatePrice();
        }

        private void OnSell()
        {
            for (int i = 0; i < selected.Count; i++)
            {
                selected[i].Sell();
            }

            PrefabManager.Instance.activePlayer.sellSound.Play();

            Refresh();
        }

        public override void Refresh()
        {
            Clear();
            UpdatePrice();

            foreach (var item in PrefabManager.Instance.player.Data.items.Where(x => x.IsSellable))
            {
                var slot = Instantiate(PrefabManager.Instance.sellItemSlot, content);
                slot.SetItem(item);
            }
        }

        private void Clear()
        {
            selected.Clear();
            for (int i = 0;i < content.childCount;i++)
                Destroy(content.GetChild(i).gameObject);
        }

        public void UpdatePrice()
        {
            price.text = selected.Sum(x => x.Price).Format();
            sell.interactable = selected.Count != 0;
            reset.interactable = selected.Count != 0;
        }
    }
}
