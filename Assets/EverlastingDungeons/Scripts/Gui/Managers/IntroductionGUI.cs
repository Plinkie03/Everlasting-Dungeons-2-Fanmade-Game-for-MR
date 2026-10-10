using Assets.EverlastingDungeons.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.EverlastingDungeons.Scripts.Gui.Managers
{
    public class IntroductionGUI : BaseGUI
    {
        public Button ok;

        public override void Init()
        {
            ok.onClick.AddListener(() =>
            {
                GUIManager.Instance.Open(GUIManager.Instance.main);
            });
        }

        private void OnDisable()
        {
            PrefabManager.Instance.activePlayer.Data.passedTutorial = true;
        }
    }
}
