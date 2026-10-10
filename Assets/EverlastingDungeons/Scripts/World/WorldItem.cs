using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using Assets.EverlastingDungeons.Scripts.Utility;
using NUnit.Framework.Internal.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.World
{
    public class WorldItem : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public TextMeshProUGUI text;

        public PlayerInventoryItem inventoryItem;

        public void SetItem(BaseItemSO item, double level, double amount, double rarityMultiplier)
        {
            this.inventoryItem = new(item.uuid, level, amount, rarityMultiplier);
            if (PrefabManager.Instance.activePlayer.Data.minimumRarity > inventoryItem.Rarity.type)
            {
                Destroy(gameObject);
                return;
            }

            spriteRenderer.color = Aura.GetAuraForLevel(level).color;
            spriteRenderer.sprite = item.sprite;
            this.text.text = amount == 1 ? string.Empty : amount.Format();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerBehavior player))
            {
                GUIManager.Instance.main.AddItemDrop(this);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerBehavior player))
            {
                GUIManager.Instance.main.RemoveItemDrop(this);
            }
        }

        private void OnDestroy()
        {
            GUIManager.Instance.main.RemoveItemDrop(this);
        }
    }
}
