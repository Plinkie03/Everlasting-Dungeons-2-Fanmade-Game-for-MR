using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Gui.Objects
{
    public class MasterySlotGUI : MonoBehaviour
    {
        public MasteryType mastery;
        public GUIBar bar;
        public Color color;
        public TextMeshProUGUI level;
        public TextMeshProUGUI percent;
        public new TextMeshProUGUI name;

        private void OnEnable()
        {
            bar.filler.color = color;
            level.color = color;
            percent.color = color;
            name.color = color;
            var mastery = PrefabManager.Instance.activePlayer.Data.masteries.GetMastery(this.mastery);
            bar.SetValue(mastery.GetXp(), mastery.GetReqXp());
            name.text = mastery.type.ToString();
            level.text = mastery.level.Format();
            percent.text = $"{((float)mastery.GetXp() / mastery.GetReqXp() * 100f).Format()}%";
        }
    }
}
