using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Entity.Player
{
    public class PlayerGear : MonoBehaviour
    {
        public PlayerBehavior player;

        public PlayerInventoryItem[] GetEquippedPieces() => player.Data.items.Where(x => x.equipped).ToArray();
        public PlayerInventoryItem GetEquippedPiece(GearType type) => GetEquippedPieces().FirstOrDefault(x => x.Item is GearItemSO gear && gear.gearType == type);
        public double GetStatAbsolute(StatType stat) => GetEquippedPieces().Sum(x => x.GetStatAbsolute(stat));
        public double GetStatMultiplier(StatType stat) => GetEquippedPieces().Sum(x => x.GetStatMultiplier(stat));
    }
}
