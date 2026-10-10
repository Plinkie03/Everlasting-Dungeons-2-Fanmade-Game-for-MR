using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

namespace Assets.EverlastingDungeons.Scripts.Gui.Managers.Selective
{
    public class EnterDungeonGUI : BaseSelectiveGUI
    {
        public static readonly string InfoPlaceholder = "Select the floor you want to start from, remember this is limited to the highest floor you have reached";
        public static readonly string CostPlaceholder = "Everything comes with a cost though, to start from floor <color=yellow>{0}</color> you will have to pay <sprite name=\"money\"><color=yellow>{1}</color>.";
        public double priceMultiplier = 50d;

        public TextMeshProUGUI info, cost;
        public Button button;
        public Slider slider;

        private void Start()
        {
            button.onClick.AddListener(OnEnterDungeon);
        }

        private void OnEnterDungeon()
        {
            PrefabManager.Instance.activePlayer.Data.money -= GetEnterCost();
            PrefabManager.Instance.activePlayer.Data.currentFloor = (long)slider.value;
            _ = SceneManager.Instance.MoveToScene(SceneType.DungeonScene);
        }
        public double GetEnterCost()
        {
            if (slider.value <= 1)
                return 0;

            var price = Math.Round(priceMultiplier * slider.value * (slider.value + 1));
            return price < 0 ? double.MaxValue : price;
        }
        public override void Refresh()
        {
            UpdateGui();
            slider.value = slider.maxValue;
        }

        private void UpdateGui()
        {
            slider.maxValue = (float)PrefabManager.Instance.activePlayer.Data.highestFloor;
            info.text = InfoPlaceholder;
            cost.text = CostPlaceholder.Replace("{0}", ((double)slider.value).Format()).Replace("{1}", GetEnterCost().Format());
            button.interactable = slider.value > 0 && PrefabManager.Instance.activePlayer.Data.money >= GetEnterCost();
        }

        private void FixedUpdate()
        {
            UpdateGui();
        }
    }
}
