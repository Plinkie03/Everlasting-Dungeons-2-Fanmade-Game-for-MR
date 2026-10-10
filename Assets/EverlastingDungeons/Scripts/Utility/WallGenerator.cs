using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Utility
{
    public static class WallGenerator
    {
        public static void CreateWalls(DungeonSpritesSO sprites, HashSet<Vector2Int> floorPositions)
        {
            var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionList);
            var cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionList);
            CreateBasicWalls(sprites, basicWallPositions, floorPositions);
            CreateCornerWalls(sprites, cornerWallPositions, floorPositions);
        }

        private static void CreateCornerWalls(DungeonSpritesSO sprites, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
        {
            foreach (var position in cornerWallPositions)
            {
                string neigboursBinaryType = "";
                foreach (var direction in Direction2D.eightDirectionList)
                {
                    var neighbourPosition = position + direction;
                    if (floorPositions.Contains(neighbourPosition))
                    {
                        neigboursBinaryType += "1";
                    }
                    else
                    {
                        neigboursBinaryType += "0";
                    }
                }

                PlaceCornerWall(sprites, position, neigboursBinaryType);
            }
        }

        private static void PlaceCornerWall(DungeonSpritesSO sprites, Vector2Int position, string binaryType)
        {
            int typeAsInt = Convert.ToInt32(binaryType, 2);
            Sprite sprite = default;

            if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
            {
                sprite = sprites.wallInnerCornerDownLeft;
            }
            else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
            {
                sprite = sprites.wallInnerCornerDownRight;
            }
            else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
            {
                sprite = sprites.wallDiagonalCornerDownLeft;
            }
            else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
            {
                sprite = sprites.wallDiagonalCornerDownRight;
            }
            else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
            {
                sprite = sprites.wallDiagonalCornerUpLeft;
            }
            else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
            {
                sprite = sprites.wallDiagonalCornerUpRight;
            }
            else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
            {
                sprite = sprites.wallFull;
            }
            else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt))
            {
                sprite = sprites.wallBottom;
            }

            if (!sprite)
                return;

            DungeonManager.Instance.PlaceTile(DungeonManager.Instance.floor, sprite, position, true);
        }

        private static void CreateBasicWalls(DungeonSpritesSO sprites, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions)
        {
            foreach (var wallPosition in basicWallPositions)
            {
                string neighboursBinaryType = "";

                foreach (var direction in Direction2D.cardinalDirectionList)
                {
                    var neighbourPosition = wallPosition + direction;
                    if (floorPositions.Contains(neighbourPosition))
                    {
                        neighboursBinaryType += "1";
                    }
                    else
                    {
                        neighboursBinaryType += "0";
                    }
                }

                PlaceWall(sprites, wallPosition, neighboursBinaryType);
            }
        }

        private static void PlaceWall(DungeonSpritesSO sprites, Vector2Int wallPosition, string binaryType)
        {
            int typeAsInt = Convert.ToInt32(binaryType, 2);
            Sprite sprite = default;
            if (WallTypesHelper.wallTop.Contains(typeAsInt))
            {
                sprite = sprites.wallTop;
            }
            else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
            {
                sprite = sprites.wallSideRight;
            }
            else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
            {
                sprite = sprites.wallSideLeft;
            }
            else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
            {
                sprite = sprites.wallBottom;
            }
            else if (WallTypesHelper.wallFull.Contains(typeAsInt))
            {
                sprite = sprites.wallFull;
            }

            if (!sprite)
                return;

            DungeonManager.Instance.PlaceTile(DungeonManager.Instance.floor, sprite, wallPosition, true);
        }

        private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directions)
        {
            HashSet<Vector2Int> wallPositions = new();
            foreach (var position in floorPositions)
            {
                foreach (var direction in directions)
                {
                    var neighbourPosition = position + direction;
                    if (!floorPositions.Contains(neighbourPosition))
                        wallPositions.Add(neighbourPosition);
                }
            }
            return wallPositions;
        }
    }
}
