using Assets.EverlastingDungeons.Scripts.Gui.Managers.Selective;
using Assets.EverlastingDungeons.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.EverlastingDungeons.Scripts.Gui.Objects
{
    public class LeaderboardButtonGUI : MonoBehaviour
    {
        public LeaderboardType type;
        public Button button;
        public Image selected;

        private void Start()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            GUIManager.Instance.selective.leaderboard.SetType(type);
        }

        private void FixedUpdate()
        {
            selected.gameObject.SetActive(GUIManager.Instance.selective.leaderboard.type == type);
        }
    }
}
