using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.Utility;
using Assets.EverlastingDungeons.Scripts.World;
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
    public class ItemDropGUI : MonoBehaviour
    {
        public Button pickup;
        public Image image;
        public new TextMeshProUGUI name;
        private WorldItem worldItem;

        private void Start()
        {
            pickup.onClick.AddListener(OnClick);
        }

        public void PickUp()
        {
            PrefabManager.Instance.CreateFloatingText(worldItem.transform.position, $"+{worldItem.inventoryItem.DisplayName}", Color.white, 1.2f);
            PrefabManager.Instance.activePlayer.Data.AddItem(worldItem.inventoryItem);
            PrefabManager.Instance.activePlayer.PlayRandomFrom(PrefabManager.Instance.activePlayer.lootSounds);
            Destroy(worldItem.gameObject);
        }

        private void OnClick() => PickUp();

        public void SetItem(WorldItem worldItem)
        {
            this.worldItem = worldItem;
            image.sprite = worldItem.inventoryItem.Item.sprite;
            image.color = worldItem.inventoryItem.SpriteColor;
            name.text = worldItem.inventoryItem.DisplayName;
        }
    }
}
