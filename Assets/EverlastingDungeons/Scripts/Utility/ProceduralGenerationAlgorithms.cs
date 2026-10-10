using Assets.EverlastingDungeons.Scripts.Entity.Monster;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.EverlastingDungeons.Scripts.Utility
{
    public enum RoomType
    {
        Unknown,
        Boss,
        Spawn
    }

    public class RoomConfigurationOptions
    {
        public bool onlyBoss;
        public Mutation onlyMutation;

        public MonsterBehavior SpawnBoss(Vector3 pos)
        {
            var mob = PrefabManager.Instance.SpawnMonster(pos, data: Boss, mutation: onlyMutation);
            mob.allowedLadderSpawn = true;
            return mob;
        }

        public MonsterBehavior SpawnMob(Vector2Int pos)
        {
            var mob = PrefabManager.Instance.SpawnMonster(pos, data: Mob, mutation: onlyMutation);
            mob.allowedLadderSpawn = false;
            return mob;
        }

        private MonsterSO Boss => ResourceManager.Instance.monsters.Raw.Where(x => x.boss && Aura.GetFloorLevel(PrefabManager.Instance.activePlayer.Data.currentFloor) >= x.floorLevel).OrderByDescending(x => Aura.GetFloorLevel(PrefabManager.Instance.activePlayer.Data.currentFloor)).First();
        private MonsterSO Mob => onlyBoss ? Boss : default;
    }

    public class RoomData
    {
        public RoomType type;
        public BoundsInt bounds;
        public RoomConfigurationOptions config;
        public HashSet<Vector2Int> roomFloor;

        public RoomData(BoundsInt bounds, RoomConfigurationOptions config)
        {
            this.config = config;
            this.bounds = bounds;
        }

        private void FilterInvalidFloor(HashSet<Vector2Int> dungeonFloor)
        {
            foreach (var floorPos in roomFloor.ToList())
            {
                if (!dungeonFloor.Contains(floorPos))
                    roomFloor.Remove(floorPos);
            }
        }

        public void Prepare(HashSet<Vector2Int> dungeonFloor, DungeonGenerationOptionsSO options)
        {
            FilterInvalidFloor(dungeonFloor);

            switch (type)
            {
                case RoomType.Unknown:
                    var mobsToAddRemaining = options.mobCountPerRoom;
                    while (roomFloor.Count != 0 && mobsToAddRemaining != 0)
                    {
                        var randomPos = roomFloor.Random();
                        roomFloor.Remove(randomPos);
                        config.SpawnMob(randomPos);
                        mobsToAddRemaining--;
                    }
                    break;

                case RoomType.Boss:
                    config.SpawnBoss(bounds.center);
                    break;

                case RoomType.Spawn:
                    PrefabManager.Instance.SpawnPlayer(bounds.center);
                    break;
            }
        }
    }

    public static class ProceduralGenerationAlgorithms
    {
        public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int walkLength)
        {
            var path = new HashSet<Vector2Int>();
            path.Add(startPos);
            var previousPos = startPos;

            for (int i = 0; i < walkLength; i++)
            {
                var newPos = previousPos + Direction2D.GetRandomCardinalDirection();
                path.Add(newPos);
                previousPos = newPos;
            }

            return path;
        }

        public static List<RoomData> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight, RoomConfigurationOptions config)
        {
            Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
            List<RoomData> roomsList = new();
            roomsQueue.Enqueue(spaceToSplit);

            while (roomsQueue.Count > 0)
            {
                var room = roomsQueue.Dequeue();
                if (room.size.y >= minHeight && room.size.x >= minWidth)
                {
                    if (UnityEngine.Random.value < 0.5f)
                    {
                        if (room.size.y >= minHeight * 2)
                        {
                            SplitHorizontally(minHeight, roomsQueue, room);
                        } 
                        else if (room.size.x >= minWidth * 2)
                        {
                            SplitVertically(minWidth, roomsQueue, room);
                        }
                        else
                        {
                            roomsList.Add(new(room, config));
                        }
                    }
                    else
                    {
                        if (room.size.x >= minWidth * 2)
                        {
                            SplitVertically(minWidth, roomsQueue, room);
                        }
                        else if (room.size.y >= minHeight * 2)
                        {
                            SplitHorizontally(minHeight, roomsQueue, room);
                        }
                        else
                        {
                            roomsList.Add(new(room, config));
                        }
                    }
                } 
            }

            return roomsList;
        }

        private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
        {
            var xSplit = UnityEngine.Random.Range(1, room.size.x);
            BoundsInt room1 = new(room.min, new(xSplit, room.size.y, room.size.z));
            BoundsInt room2 = new(new(room.min.x + xSplit, room.min.y, room.min.z), new(room.size.x - xSplit, room.size.y, room.size.z));
            roomsQueue.Enqueue(room1);
            roomsQueue.Enqueue(room2);
        }

        private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
        {
            var ySplit = UnityEngine.Random.Range(1, room.size.y);
            BoundsInt room1 = new(room.min, new(room.size.x, ySplit, room.size.z));
            BoundsInt room2 = new(new(room.min.x, room.min.y + ySplit, room.min.z), new(room.size.x, room.size.y - ySplit, room.size.z));
            roomsQueue.Enqueue(room1);
            roomsQueue.Enqueue(room2);
        }

        public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int corridorLength)
        {
            List<Vector2Int> corridor = new();
            var direction = Direction2D.GetRandomCardinalDirection();
            var currentPos = startPos;
            corridor.Add(currentPos);

            for (int i = 0; i < corridorLength;i++)
            {
                currentPos += direction;
                corridor.Add(currentPos);
            }

            return corridor;
        }
    }

    public static class Direction2D
    {
        public static readonly List<Vector2Int> cardinalDirectionList = new()
        {
            Vector2Int.up, // UP
            Vector2Int.right, // RIGHT
            Vector2Int.down, // DOWN
            Vector2Int.left, // LEFT
        };

        public static readonly List<Vector2Int> diagonalDirectionList = new()
        {
            new Vector2Int(1, 1), // UP RIGHT
            new Vector2Int(1, -1), // RIGHT DOWN
            new Vector2Int(-1, -1), // DOWN LEFT
            new Vector2Int(-1, 1), // LEFT -UP
        };

        public static List<Vector2Int> eightDirectionList = new List<Vector2Int>
        {
            Vector2Int.up, // UP
            new Vector2Int(1, 1), // UP RIGHT
            Vector2Int.right, // RIGHT
            new Vector2Int(1, -1), // RIGHT DOWN
            Vector2Int.down, // DOWN
            new Vector2Int(-1, -1), // DOWN LEFT
            Vector2Int.left, // LEFT
            new Vector2Int(-1, 1), // LEFT -UP
        };

        public static Vector2Int GetRandomCardinalDirection() => cardinalDirectionList.Random();
    }
}
