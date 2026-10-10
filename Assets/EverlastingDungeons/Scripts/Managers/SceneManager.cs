using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.STP;

namespace Assets.EverlastingDungeons.Scripts.Managers
{
    public enum SceneType
    {
        LoginScene,
        TownScene,
        DungeonScene
    }

    public class SceneManager : BaseManager<SceneManager>
    {
        private SceneType currentScene;

        public override void Init()
        {

        }

        public SceneType Current => currentScene;

        public async Task MoveToScene(SceneType type, RoomConfigurationOptions config = default)
        {
            config ??= new();

            GUIManager.Instance.CloseAll();
            GUIManager.Instance.loadScreen.gameObject.SetActive(true);

            var operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(type.ToString());

            while (!operation.isDone)
                await Task.Yield();

            currentScene = type;

            if (AuthenticationManager.Instance.data != null)
                DatabaseManager.Instance.UpdatePlayerData(AuthenticationManager.Instance.data);

            if (type is SceneType.TownScene)
            {
                DungeonManager.Instance.Clear();
                PrefabManager.Instance.SpawnPlayer(Vector3.zero);
                PrefabManager.Instance.activePlayer.townMusic.Play();
            } else if (type is SceneType.DungeonScene)
            {
                DungeonManager.Instance.Generate(config);
                PrefabManager.Instance.activePlayer.dungeonMusic.Play();
            } else if (type is SceneType.LoginScene)
            {
                GUIManager.Instance.Open(GUIManager.Instance.login);
            }
            GUIManager.Instance.loadScreen.gameObject.SetActive(false);
        }
    }
}
