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
    public class SelectiveButtonGUI : MonoBehaviour
    {
        public Button button;
        public BaseSelectiveGUI gui;
        public Image selected;

        public void Start()
        {
            button.onClick.AddListener(() =>
            {
                if (gui != null)
                {
                    if (GUIManager.Instance.selective.Current == gui)
                        return;

                    if (!GUIManager.Instance.selective.gameObject.activeSelf)
                        GUIManager.Instance.Open(GUIManager.Instance.selective);
                    GUIManager.Instance.selective.Open(gui);
                }
                else
                {
                    GUIManager.Instance.Open(GUIManager.Instance.main);
                }
            });
        }

        private void FixedUpdate()
        {
            if (selected)
                selected.gameObject.SetActive(GUIManager.Instance.selective.Current == gui);
        }
    }
}
