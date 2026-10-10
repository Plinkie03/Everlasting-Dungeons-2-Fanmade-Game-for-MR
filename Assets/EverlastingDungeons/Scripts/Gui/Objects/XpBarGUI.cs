using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.EverlastingDungeons.Scripts.Gui.Objects
{
    public class XpBarGUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GUIBar bar;
        public TextMeshProUGUI text;

        private void Start()
        {
            text.transform.parent.gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            text.text = $"{PrefabManager.Instance.activePlayer.GetXp().Format()} / {PrefabManager.Instance.activePlayer.GetReqXp().Format()} ({((float)PrefabManager.Instance.activePlayer.GetXp() / PrefabManager.Instance.activePlayer.GetReqXp() * 100f).Format()}%)";
            text.transform.parent.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            text.transform.parent.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (PrefabManager.Instance.activePlayer)
                bar.SetValue(PrefabManager.Instance.activePlayer.GetXp(), PrefabManager.Instance.activePlayer.GetReqXp());
        }
    }
}
