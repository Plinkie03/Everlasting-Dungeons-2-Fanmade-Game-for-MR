using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriInspector;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Utility
{
    [Serializable]
    public class ItemDrop
    {
        public BaseItemSO item;
        public float chance;

        [LabelText("Min"), Group("Amount")]
        public long minAmount = 1;

        [LabelText("Max"), Group("Amount")]
        public long maxAmount = 1;

        public long RandomAmount => UnityEngine.Random.Range((int)minAmount, (int)maxAmount + 1);
    }
}
