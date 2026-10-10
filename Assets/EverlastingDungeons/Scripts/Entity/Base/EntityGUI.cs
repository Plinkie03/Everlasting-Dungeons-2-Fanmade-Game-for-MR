using Assets.EverlastingDungeons.Scripts.Gui.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Entity.Base
{
    public class EntityGUI : MonoBehaviour
    {
        public EntityBehavior entity;
        public GUIBar bar;
        public new TextMeshProUGUI name;
    }
}
