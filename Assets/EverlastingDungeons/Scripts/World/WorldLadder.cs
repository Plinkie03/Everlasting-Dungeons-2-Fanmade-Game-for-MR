using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.World
{
    public class WorldLadder : MonoBehaviour
    {
        public RoomConfigurationOptions config = new();

        private void Start()
        {
            gameObject.SetActive(false);
            GetComponent<SpriteRenderer>().sprite = DungeonManager.Instance.sprites.ladder;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<PlayerBehavior>(out var p))
            {
                PrefabManager.Instance.activePlayer.Data.currentFloor += PrefabManager.Instance.activePlayer.Data.rebirthCount + 1;
                PrefabManager.Instance.activePlayer.Data.highestFloor = Math.Max(PrefabManager.Instance.activePlayer.Data.currentFloor, PrefabManager.Instance.activePlayer.Data.highestFloor);
                SceneManager.Instance.MoveToScene(SceneType.DungeonScene, config);
            }
        }
    }
}
