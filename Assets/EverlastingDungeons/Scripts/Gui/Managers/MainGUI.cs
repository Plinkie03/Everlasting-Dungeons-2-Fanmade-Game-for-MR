using Assets.EverlastingDungeons.Scripts.Entity.Monster;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Gui.Objects;
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

namespace Assets.EverlastingDungeons.Scripts.Gui.Managers
{
    public class MainGUI : BaseGUI
    {
        public static readonly float MaxBossDistance = 15f;

        public Transform itemDropContent;
        public SpellHotkeySlotGUI[] spellHotkeys;
        public ConsumableHotkeySlotGUI[] consumableHotkeys;
        public Transform notifications;
        public GUIBar healthBar, manaBar, bossBar;
        public TextMeshProUGUI floor;
        public TextMeshProUGUI level, money, gems, bossName;
        public Button leaveDungeonBtn;
        public Button rarity;
        public TextMeshProUGUI rarityText;

        private Dictionary<WorldItem, ItemDropGUI> availableItemDrops = new();

        private void Start()
        {
            leaveDungeonBtn.onClick.AddListener(OnLeaveDungeon);
            rarity.onClick.AddListener(() => OnSwapRarity());
        }

        private void OnSwapRarity()
        {
            var current = PrefabManager.Instance.activePlayer.Data.minimumRarity;

            int next = (int)current + 1;

            if (!Enum.IsDefined(typeof(RarityType), next))
                next = 0;

            PrefabManager.Instance.activePlayer.Data.minimumRarity = (RarityType)next;
        }

        private void OnLeaveDungeon() => _ = SceneManager.Instance.MoveToScene(SceneType.TownScene);

        public override void Init()
        {

        }

        public void Notify(string text, Color color)
        {
            var notif = Instantiate(PrefabManager.Instance.notification, notifications);
            notif.text = text;
            notif.color = color;
            Destroy(notif, 3f);
        }

        public void SetBossbarData(MonsterBehavior m)
        {
            if (!m)
            {
                bossBar.transform.parent.gameObject.SetActive(false);
                return;
            }

            bossBar.transform.parent.gameObject.SetActive(!m.IsDead && Vector2.Distance(PrefabManager.Instance.activePlayer.transform.position, m.transform.position) < 15f);
            bossBar.SetValue(m.health, m.ModdedStats.GetStat(StatType.MaxHealth));
            bossName.text = m.GetName();
        }

        public void AddItemDrop(WorldItem worldItem)
        {
            var itemDrop = Instantiate(PrefabManager.Instance.itemDrop, itemDropContent);
            itemDrop.SetItem(worldItem);
            availableItemDrops.Add(worldItem, itemDrop);
        }

        public void RemoveItemDrop(WorldItem worldItem)
        {
            if (availableItemDrops.TryGetValue(worldItem, out var itemDrop))
            {
                Destroy(itemDrop.gameObject);
                availableItemDrops.Remove(worldItem);
            }
        }

        private void FixedUpdate()
        {
            if (!PrefabManager.Instance.activePlayer)
                return;
            notifications.gameObject.SetActive(notifications.childCount != 0);
            healthBar.SetValue(PrefabManager.Instance.activePlayer.health, PrefabManager.Instance.activePlayer.ModdedStats.GetStat(StatType.MaxHealth));
            manaBar.SetValue(PrefabManager.Instance.activePlayer.mana, PrefabManager.Instance.activePlayer.ModdedStats.GetStat(StatType.MaxMana));
            level.text = PrefabManager.Instance.activePlayer.Data.level.Format();
            money.text = PrefabManager.Instance.activePlayer.Data.money.Format();
            gems.text = PrefabManager.Instance.activePlayer.Data.gems.Format();
            floor.text = $"Floor {PrefabManager.Instance.activePlayer.Data.currentFloor.Format()}";
            leaveDungeonBtn.gameObject.SetActive(SceneManager.Instance.Current is SceneType.DungeonScene);
            bossBar.gameObject.SetActive(SceneManager.Instance.Current is SceneType.DungeonScene);
            rarityText.text = PrefabManager.Instance.activePlayer.Data.MinimumRarity.Color($"{PrefabManager.Instance.activePlayer.Data.minimumRarity.ToString()} (T{((int)PrefabManager.Instance.activePlayer.Data.minimumRarity) + 1})");
        }
    }
}
