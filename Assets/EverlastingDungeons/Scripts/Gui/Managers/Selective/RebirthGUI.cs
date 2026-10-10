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
    public class RebirthGUI : BaseSelectiveGUI
    {
        public static readonly string RebirthInfoPlaceholder = "Rebirthing gives you a stat boost of 25% to all primary stats (Currently <color=red>{0}%</color>)";
        public static readonly string RebirthCostPlaceholder = "You are currently at rebirth <color=red>{0}</color>, rebirthing will cost <sprite name=\"gem\"><color=red>{1}</color>. Are you sure you want to rebirth?";
        public TextMeshProUGUI info, cost;
        public Button button;

        public void Start()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            PrefabManager.Instance.activePlayer.Data.Rebirth();
            PrefabManager.Instance.activePlayer.rebirthSound.Play();
        }

        public override void Refresh()
        {
            info.text = RebirthInfoPlaceholder.Replace("{0}", ((PrefabManager.Instance.activePlayer.Data.GetRebirthMultiplier() - 1) * 100d).Format());
            cost.text = RebirthCostPlaceholder.Replace("{0}", PrefabManager.Instance.activePlayer.Data.rebirthCount.Format()).Replace("{1}", PrefabManager.Instance.activePlayer.Data.GetRebirthCost().Format());
            button.interactable = PrefabManager.Instance.activePlayer.Data.gems >= PrefabManager.Instance.activePlayer.Data.GetRebirthCost();
        }

        private void FixedUpdate()
        {
            Refresh();
        }
    }
}
