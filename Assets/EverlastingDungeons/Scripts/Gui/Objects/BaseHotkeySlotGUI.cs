using Assets.EverlastingDungeons.Scripts.Entity.Base;
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
using UnityEngine.UI;

namespace Assets.EverlastingDungeons.Scripts.Gui.Objects
{
    public abstract class BaseHotkeySlotGUI<T> : MonoBehaviour
    {
        public Image gray;
        public TextMeshProUGUI cooldown;
        public Button button;
        public Image selected;
        public Image icon;
        public int index;
        public abstract T Element { get; }

        private void Start()
        {
            if (button)
                button.onClick.AddListener(OnClick);
            UpdateData();
        }

        protected abstract void OnClick();
        protected abstract bool IsAssigned { get; }

        protected virtual void UpdateData()
        {
            if (selected)
                selected.gameObject.SetActive(IsAssigned);

            if (Element == null)
            {
                if (gray)
                {
                    gray.gameObject.SetActive(true);
                    button.interactable = false;
                    cooldown.gameObject.SetActive(false);
                }

                icon.gameObject.SetActive(false);
            }
            else
            {
                if (gray)
                {
                    button.interactable = IsAvailable;
                    gray.gameObject.SetActive(!IsAvailable);
                    cooldown.gameObject.SetActive(TimeLeft != 0);
                    cooldown.text = (TimeLeft / 1000 + 1).Format();
                }

                icon.gameObject.SetActive(true);
                icon.sprite = Sprite;
            }
        }

        protected abstract Sprite Sprite { get; }
        protected abstract long TimeLeft { get; }
        protected abstract bool IsAvailable { get; }

        public void FixedUpdate()
        {
            UpdateData();
        }
    }
}
