using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Entity.Monster
{
    public class MonsterAI : MonoBehaviour
    {
        public static readonly float DetectionRange = 8f;
        public MonsterBehavior monster;
        public Seeker seeker;
        private List<Vector3> waypoints;
        private int waypointIndex = -1;
        private Vector3? currentWaypoint;
        private bool forceTrack = false;

        public bool IsPlayerInDetectionRange => forceTrack || (PrefabManager.Instance.activePlayer != null && Vector3.Distance(monster.transform.position, PrefabManager.Instance.activePlayer.transform.position) <= DetectionRange);

        private void Start()
        {
            InvokeRepeating(nameof(Move), 0f, 1f);
        }
        private void Move()
        {
            if (IsPlayerInDetectionRange)
                forceTrack = true;

            if (IsPlayerInDetectionRange && monster.attackHandler.Target != null && !monster.attackHandler.IsTargetInRange)
            {
                seeker.StartPath(currentWaypoint.HasValue ? currentWaypoint.Value : seeker.transform.position.Snap(), PrefabManager.Instance.activePlayer.transform.position.Snap(), OnPathComplete);
            } else
            {
                Stop();
            }
        }

        private void OnPathComplete(Path path)
        {
            Stop();

            if (path.error)
            {
                Debug.Log(path.errorLog);
            } else
            {
                waypoints = path.vectorPath;
            }
        }

        private void Stop()
        {
            waypointIndex = -1;
            waypoints = default;
        }

        private void FixedUpdate()
        {
            if (waypoints != null && !currentWaypoint.HasValue)
            {
                if (monster.attackHandler.IsTargetInRange || waypointIndex + 1 >= waypoints.Count)
                {
                    Stop();
                    return;
                }

                currentWaypoint = waypoints[++waypointIndex];
            }

            if (currentWaypoint.HasValue)
            {
                var dir = (currentWaypoint.Value - monster.transform.position).normalized;
                monster.transform.position = Vector3.MoveTowards(monster.transform.position, currentWaypoint.Value, (float)monster.ModdedStats.GetStat(Utility.StatType.Speed) * Time.deltaTime);
                if (monster.transform.position == currentWaypoint.Value)
                {
                    currentWaypoint = null;
                }
            }
        }
    }
}
