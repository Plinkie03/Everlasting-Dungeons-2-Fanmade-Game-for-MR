using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using Assets.EverlastingDungeons.Scripts.Utility;
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
    public class WishStatsGUI : BaseSelectiveGUI
    {
        public static readonly string CostPlaceholder = "<sprite name=\"gem\"><color=red>{0}";

        public Button selectBtn;
        public Transform content;
        public TextMeshProUGUI cost;
        public GearType selectedGearType;
        public Transform buttons;
        private Button copy;

        public double GetGemCost() => Math.Pow(10, GetCheckedToggles().Length);
        public StatType[] GetCheckedStats() => GetCheckedToggles().Select(x => Enum.Parse<StatType>(x.name)).ToArray();
        public Toggle[] GetCheckedToggles() => GetToggles().Where(x => x.isOn).ToArray();
        public Toggle[] GetToggles() => content.GetComponentsInChildren<Toggle>();

        private void Start()
        {
            var btn = buttons.GetComponentInChildren<Button>();
            copy = Instantiate(btn);

            Destroy(btn.gameObject);

            foreach (var raw in Enum.GetValues(typeof(GearType)))
            {
                var gearType = (GearType)raw;
                var newBtn = Instantiate(copy);
                newBtn.transform.SetParent(buttons);
                newBtn.GetComponentInChildren<TextMeshProUGUI>().text = gearType.ToString();
                newBtn.onClick.AddListener(() => OnSelectGearType(newBtn, gearType));
            }

            buttons.GetComponentInChildren<Button>().image.color = Color.yellow;

            selectBtn.onClick.AddListener(OnSelect);
        }

        private void OnSelectGearType(Button btn, GearType type)
        {
            selectedGearType = type;
            
            foreach (var btn2 in buttons.GetComponentsInChildren<Button>())
            {
                btn2.image.color = btn2 == btn ? Color.yellow : Color.white;
            }

            Refresh();
        }

        private void OnSelect()
        {
            PrefabManager.Instance.activePlayer.Data.gems -= GetGemCost();
            PrefabManager.Instance.activePlayer.Data.wishStats[selectedGearType.ToString()] = GetCheckedStats().ToList();
            PrefabManager.Instance.activePlayer.wishStatsSound.Play();
        }

        public override void Refresh()
        {
            Clear();

            foreach (var raw in Enum.GetValues(typeof(StatType))) {
                var stat = (StatType)raw;
                var toggle = Instantiate(PrefabManager.Instance.statToggle, content);
                toggle.isOn = PrefabManager.Instance.activePlayer.Data.GetWishStats(selectedGearType).Contains(stat);
                toggle.name = stat.ToString();

                var txt = toggle.GetComponentInChildren<TextMeshProUGUI>();
                txt.color = Stat.GetStatColor(stat);
                txt.text = Stat.GetStatName(stat);
            }
        }

        private void Clear()
        {
            for (int i = 0;i < content.childCount;i++) 
                Destroy(content.GetChild(i).gameObject);
        }

        private void FixedUpdate()
        {
            if (!PrefabManager.Instance.activePlayer)
                return;
            cost.text = CostPlaceholder.Replace("{0}", GetGemCost().Format());
            selectBtn.interactable = PrefabManager.Instance.activePlayer.Data.gems >= GetGemCost() && 
                (GetCheckedToggles().Length != PrefabManager.Instance.activePlayer.Data.GetWishStats(selectedGearType).Count || 
                !GetCheckedStats().All(x => PrefabManager.Instance.activePlayer.Data.GetWishStats(selectedGearType).Contains(x)));
        }
    }
}
