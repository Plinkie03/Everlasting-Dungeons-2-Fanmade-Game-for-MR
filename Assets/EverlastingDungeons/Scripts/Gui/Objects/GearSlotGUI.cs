using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.EverlastingDungeons.Scripts.Gui.Objects
{
    public class GearSlotGUI : ItemSlotGUI
    {
        public Sprite placeholder;
        public GearType type;

        public new void Start()
        {
            base.Start();
            button.onClick.AddListener(() =>
            {
                if (GUIManager.Instance.selective.gear.selectedGearType == type)
                    return;
                GUIManager.Instance.selective.gear.selectedGearType = type;
                GUIManager.Instance.selective.gear.otherItemInfoGUI.currentItem = null;
                GUIManager.Instance.selective.gear.otherItemInfoGUI.panel.gameObject.SetActive(false);
                GUIManager.Instance.selective.gear.Refresh();
            });
        }

        public override void SetItem(PlayerInventoryItem item)
        {
            base.SetItem(item);
            if (item == null)
            {
                icon.sprite = placeholder;
                button.GetComponent<Image>().color = Color.white;
            }
        }

        public void Update()
        {
            selected.gameObject.SetActive(GUIManager.Instance.selective.gear.selectedGearType == type);
        }
    }
}
