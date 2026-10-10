using Assets.EverlastingDungeons.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Entity.Other
{
    public class NPCSellerBehavior : MonoBehaviour
    {
        public NPCBehavior npc;

        public void Start()
        {
            npc.gui = GUIManager.Instance.selective.sell;
        }
    }
}
