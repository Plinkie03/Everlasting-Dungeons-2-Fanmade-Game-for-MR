using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Gui.Objects;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriInspector;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.ScriptableObjects
{
    public abstract class BaseItemSO : BaseSO<BaseItemSO>
    {
        public Sprite sprite;
        public string description;
        public double price;
        public abstract bool IsStackable { get; }
        public abstract bool IsSellable { get; }

        public abstract void FillInfo(BaseItemInfoGUI info, PlayerInventoryItem item);
    }
}
