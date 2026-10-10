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
    [CreateAssetMenu(fileName = "New Collection of Item Drops", menuName = "Everlasting Dungeons/Drops/Collection")]
    public class CollectionItemDropSO : BaseSO<CollectionItemDropSO>
    {
        [TableList()]
        public List<ItemDrop> drops = new();
    }
}
