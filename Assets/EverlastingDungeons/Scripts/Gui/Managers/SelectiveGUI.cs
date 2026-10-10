using Assets.EverlastingDungeons.Scripts.Gui.Managers.Selective;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Gui.Managers
{
    public class SelectiveGUI : BaseGUI
    {
        public GearGUI gear;
        public MasteryGUI mastery;
        public InventoryGUI inventory;
        public SpellAssignmentGUI spellAssignment;
        public SellItemGUI sell;
        public ConsumableAssignmentGUI consumableAssignment;
        public BuyItemGUI buy;
        public LeaderboardGUI leaderboard;
        public RebirthGUI rebirth;
        public WishStatsGUI wishStats;
        public EnterDungeonGUI dungeon;
        public SettingsGUI settings;
        public CreditsGUI credits;
        private BaseSelectiveGUI[] selectiveGuis;

        public override void Init()
        {
            selectiveGuis = CachedReflection.GetInstanceValues<BaseSelectiveGUI>(this);
        }

        public BaseSelectiveGUI Current => selectiveGuis.FirstOrDefault(g => g.gameObject.activeSelf);

        public void Open(BaseSelectiveGUI gui)
        {
            CloseAll();
            gui.Refresh();
            gui.Open();
        }

        private void OnDisable()
        {
            CloseAll();
        }

        public void CloseAll()
        {
            foreach (var gui in selectiveGuis)
                gui.Close();
        }
    }
}
