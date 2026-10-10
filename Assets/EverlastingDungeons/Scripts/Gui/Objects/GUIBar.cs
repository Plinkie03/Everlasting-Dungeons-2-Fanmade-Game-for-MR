using Assets.EverlastingDungeons.Scripts.Extensions;
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
    public class GUIBar : MonoBehaviour
    {
        public TextMeshProUGUI counter;
        public bool hideCounter;
        public Image filler;
        public Slider slider;

        public void SetValue(double current, double max)
        {
            slider.value = (float)(current / max);
            if (hideCounter) counter.text = "";
            else counter.text = current.Format();
        }
    }
}
