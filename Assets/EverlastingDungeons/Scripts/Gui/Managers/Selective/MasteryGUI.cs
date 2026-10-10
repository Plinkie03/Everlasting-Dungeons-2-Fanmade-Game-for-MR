using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace Assets.EverlastingDungeons.Scripts.Gui.Managers.Selective
{
    public class MasteryGUI : BaseSelectiveGUI
    {
        public Transform statGrid;

        public override void Refresh()
        {
            ClearStats();
        }

        private void OnEnable()
        {
            StartCoroutine(AddStats());
        }

        private IEnumerator AddStats()
        {
            foreach (var raw in Enum.GetValues(typeof(StatType)))
            {
                var stat = (StatType)raw;
                var color = Stat.GetStatColor(stat);
                var value = PrefabManager.Instance.player.ModdedStats.GetStat(stat);

                AddStat(Stat.GetStatName(stat), Stat.IsAbsolute(stat) ? value.Format() : (value * 100f).Format() + "%", color);
                yield return new WaitForEndOfFrame();
            }
        }

        private void AddStat(string name, string value, Color color)
        {
            var slot = Instantiate(PrefabManager.Instance.itemStatSlot, statGrid).GetComponentInChildren<TextMeshProUGUI>();
            slot.color = color;
            slot.text = $"{value} {name}";
        }

        private void ClearStats()
        {
            for (int i = 0;i < statGrid.childCount;i++)
                Destroy(statGrid.GetChild(i).gameObject);
        }
    }
}
