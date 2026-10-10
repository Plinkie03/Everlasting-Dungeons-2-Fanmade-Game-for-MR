using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Dungeon Generation Options", menuName = "Everlasting Dungeons/Dungeon/Options")]
    public class DungeonGenerationOptionsSO : BaseSO<DungeonGenerationOptionsSO>
    {
        public int minRoomWidth = 20, minRoomHeight = 20;
        public int dungeonWidth = 100, dungeonHeight = 100;
        public int offset = 0;
        public bool startRandomlyEachIteration = true;
        public int iterations = 100;
        public int walkLength = 30;
        public int mobCountPerRoom = 10;
        public Vector2Int startPosition = Vector2Int.zero;
    }
}
