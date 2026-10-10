using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Dungeon Sprites", menuName = "Everlasting Dungeons/Dungeon/Sprites")]
    public class DungeonSpritesSO : BaseSO<DungeonSpritesSO>
    {
        public Color color;
        public Sprite ladder;
        public Sprite[] floorSprites;
        public Sprite[] decorationSprites;
        public Sprite[] candelSprites;
        public Sprite wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull,
            wallInnerCornerDownLeft, wallInnerCornerDownRight,
            wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpLeft, wallDiagonalCornerUpRight;
    }
}
