using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using Assets.EverlastingDungeons.Scripts.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Rendering.STP;

namespace Assets.EverlastingDungeons.Scripts.Managers
{
    public class DungeonManager : BaseManager<DungeonManager>
    {
        public static readonly double DecorationChance = 0.1f;
        public static readonly double CandelChance = 0.1f;
        public static readonly double CorridorMonsterChance = 0.1f;
        public Grid grid;
        public Tilemap floor, deco, collision;
        public HashSet<Vector2Int> floorPositions;
        public DungeonSpritesSO sprites;

        public override void Init()
        {
        }

        public void Generate(RoomConfigurationOptions config)
        {
            // Clear
            Clear();

            RunProceduralGeneration(config);
        }

        protected void RunProceduralGeneration(RoomConfigurationOptions config)
        {
            var options = ResourceManager.Instance.dungeonGenerationOptions.Random;
            sprites = ResourceManager.Instance.dungeonSprites.Random;

            var roomList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new((Vector3Int)options.startPosition, new(options.dungeonWidth, options.dungeonHeight, 0)), options.minRoomWidth, options.minRoomHeight, config);
            var floor = CreateRoomsRandomly(roomList, options);

            List<Vector2Int> roomCenters = new();
            foreach (var room in roomList)
                roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.bounds.center));
            
            HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
            floor.UnionWith(corridors);

            foreach (var floorPos in floor)
                PlaceTile(this.floor, sprites.floorSprites.Random(), floorPos);
            WallGenerator.CreateWalls(sprites, floor);

            // First room is spawn
            var spawnRoom = roomList.First();
            spawnRoom.type = RoomType.Spawn;

            // Last room is boss
            roomList.OrderBy(x => Vector2.Distance(spawnRoom.bounds.center, x.bounds.center)).Last().type = RoomType.Boss;

            foreach (var floorPos in floor.ToList())
            {
                // Is wall 
                if (!Direction2D.cardinalDirectionList.All(x => floor.Contains(floorPos + x)))
                {
                    if (CandelChance.IsChance())
                    {
                        DungeonManager.Instance.PlaceTile(DungeonManager.Instance.deco, sprites.candelSprites.Random(), floorPos, true);
                        floor.Remove(floorPos);
                    }
                }
                else
                {
                    if (DecorationChance.IsChance())
                        DungeonManager.Instance.PlaceTile(DungeonManager.Instance.deco, sprites.decorationSprites.Random(), floorPos, false);
                }
            }

            foreach (var room in roomList)
                room.Prepare(floor, options);

            foreach (var corridorPos in corridors)
            {
                if (!floor.Contains(corridorPos) || roomList.Any(x => x.roomFloor.Contains(corridorPos)))
                    continue;

                if (CorridorMonsterChance.IsChance())
                {
                    config.SpawnMob(corridorPos);
                }
            }

            floorPositions = floor;
        }

        private HashSet<Vector2Int> CreateRoomsRandomly(List<RoomData> roomList, DungeonGenerationOptionsSO options)
        {
            HashSet<Vector2Int> floor = new();

            for (int i = 0;i < roomList.Count;i++)
            {
                var room = roomList[i];
                var roomBounds = room.bounds;
                var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
                var roomFloor = RunRandomWalk(options, roomCenter);
                room.roomFloor = roomFloor;
                foreach (var position in roomFloor)
                {
                    if (position.x >= (roomBounds.xMin + options.offset) && position.x <= (roomBounds.xMax - options.offset) && position.y >= (roomBounds.yMin - options.offset) && position.y <= (roomBounds.yMax - options.offset)) {
                        floor.Add(position);
                    }
                }
            }

            return floor;
        }

        private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
        {
            HashSet<Vector2Int> corridors = new();
            var currentRoomCenter = roomCenters[UnityEngine.Random.Range(0, roomCenters.Count)];
            roomCenters.Remove(currentRoomCenter);

            while (roomCenters.Count > 0)
            {
                Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
                roomCenters.Remove(closest);
                HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
                currentRoomCenter = closest;
                corridors.UnionWith(newCorridor);
            }

            return corridors;
        }

        private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
        {
            HashSet<Vector2Int> corridor = new();
            var position = currentRoomCenter;
            corridor.Add(position);
            while (position.y != destination.y)
            {
                if (destination.y > position.y)
                {
                    position += Vector2Int.up;
                }
                else if (destination.y < position.y)
                {
                    position += Vector2Int.down;
                }
                corridor.Add((Vector2Int)position);
            }

            while (position.x != destination.x)
            {
                if (destination.x > position.x)
                {
                    position += Vector2Int.right;
                }
                else if (destination.x < position.x)
                {
                    position += Vector2Int.left;
                }
                corridor.Add(position);
            }

            var count = corridor.Count;
            for (int i = 0;i < count;i++)
            {
                foreach (var direction in Direction2D.cardinalDirectionList)
                    corridor.Add(corridor.ElementAt(i) + direction);
            }

            return corridor;
        }

        private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
        {
            Vector2Int closest = Vector2Int.zero;
            float length = float.MaxValue;
            foreach (var position in roomCenters)
            {
                float currentDistance = Vector2.Distance(position, currentRoomCenter);
                if (currentDistance < length)
                {
                    length = currentDistance;
                    closest = position;
                }
            }
            return closest;
        }

        private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
        {
            List<Vector2Int> deadEnds = new List<Vector2Int>();

            foreach (var position in floorPositions)
            {
                int neighbourCount = 0;
                foreach (var direction in Direction2D.cardinalDirectionList)
                {
                    if (floorPositions.Contains(position + direction))
                        neighbourCount++;
                }
                if (neighbourCount == 1)
                    deadEnds.Add(position);
            }

            return deadEnds;
        }

        protected HashSet<Vector2Int> RunRandomWalk(DungeonGenerationOptionsSO options, Vector2Int startPos)
        {
            var currentPosition = startPos;
            HashSet<Vector2Int> floorPositions = new();
            for (int i = 0; i < options.iterations; i++)
            {
                var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, options.walkLength);
                floorPositions.UnionWith(path);
                if (options.startRandomlyEachIteration)
                    currentPosition = floorPositions.Random();
            }
            return floorPositions;
        }

        public void Clear()
        {
            foreach (var comp in grid.GetComponentsInChildren<Tilemap>())
                comp.ClearAllTiles();
        }

        public void PlaceTile(Tilemap map, Sprite sprite, Vector2Int pos, bool collide = false)
        {
            var tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprite;
            map.SetTile(map.WorldToCell((Vector3Int)pos), tile);
            if (collide)
                PlaceTile(collision, sprite, pos, false);
        }
    }
}
