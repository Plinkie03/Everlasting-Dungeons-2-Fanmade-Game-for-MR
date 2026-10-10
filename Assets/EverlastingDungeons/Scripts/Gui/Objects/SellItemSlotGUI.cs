using Assets.EverlastingDungeons.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.EverlastingDungeons.Scripts.Gui.Objects
{
    public class SellItemSlotGUI : ItemSlotGUI
    {
        public new void Start()
        {
            base.Start();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (GUIManager.Instance.selective.sell.selected.Contains(InventoryItem))
            {
                GUIManager.Instance.selective.sell.selected.Remove(InventoryItem);
            }
            else
            {
                GUIManager.Instance.selective.sell.selected.Add(InventoryItem);
            }

            GUIManager.Instance.selective.sell.UpdatePrice();
        }

        protected override void UpdateData()
        {
            selected.gameObject.SetActive(GUIManager.Instance.selective.sell.selected.Contains(InventoryItem));
        }
    }
}
