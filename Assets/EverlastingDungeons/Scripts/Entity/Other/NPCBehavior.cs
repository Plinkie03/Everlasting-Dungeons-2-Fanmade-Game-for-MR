using Assets.EverlastingDungeons.Scripts.Gui.Managers;
using Assets.EverlastingDungeons.Scripts.Gui.Managers.Selective;
using Assets.EverlastingDungeons.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Entity.Other
{
    public class NPCBehavior : MonoBehaviour
    {
        public string npcName;
        public float npcDistance;
        public string npcDescription;
        public Sprite npcSprite;

        public BaseSelectiveGUI gui;
        public SceneType tp;

        public SpriteRenderer spriteRenderer;
        public new TextMeshProUGUI name;
        public TextMeshProUGUI description;
        [SerializeField]
        private bool isMobileFriendly = true;

        private void Start()
        {
            name.text = npcName;
            description.text = npcDescription;
            spriteRenderer.sprite = npcSprite;
        }

        public void Interact()
        {
            if (!isMobileFriendly && Application.isMobilePlatform)
            {
                GUIManager.Instance.main.Notify($"This NPC is not available on mobile devices.", Color.white);
                return;
            }

            if (Vector2.Distance(PrefabManager.Instance.activePlayer.transform.position, transform.position) > npcDistance)
            {
                GUIManager.Instance.main.Notify($"You're too far away from {npcName}", Color.white);
                return;
            }

            if (gui)
            {
                GUIManager.Instance.Open(GUIManager.Instance.selective);
                GUIManager.Instance.selective.Open(gui);
            }
            else
            {
                _ = SceneManager.Instance.MoveToScene(tp);
            }
        }
    }
}
