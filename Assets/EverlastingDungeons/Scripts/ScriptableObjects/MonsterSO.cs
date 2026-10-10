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
    [CreateAssetMenu(fileName = "New Monster", menuName = "Everlasting Dungeons/Entity/Monster", order = 1)]
    public class MonsterSO : BaseSO<MonsterSO>
    {
        public EntityClassType classType;
        public EntityAnimationsSO animations;
        public float textOffset = 1f;
        public long floorLevel;
        public List<SpellSO> spells = new();
        public float scale = 1f;
        public bool boss;
        public Color color = Color.white;

        public Reward rewards = new();

        [TableList(Draggable = true)]
        public List<ItemDrop> drops = new();

        public List<CollectionItemDropSO> dropCollection = new();

        [TableList()]
        public List<Stat> stats = new()
        {
            Stat.Absolute(StatType.MaxHealth, 100f),
            Stat.Absolute(StatType.Defense, 0f),
            Stat.Absolute(StatType.Melee, 3f),
            Stat.Absolute(StatType.Distance, 3f),
            Stat.Absolute(StatType.Magic, 3f),
            Stat.Absolute(StatType.AttackRate, 1f),
            Stat.Absolute(StatType.Speed, 1f),
        };
    }
}
