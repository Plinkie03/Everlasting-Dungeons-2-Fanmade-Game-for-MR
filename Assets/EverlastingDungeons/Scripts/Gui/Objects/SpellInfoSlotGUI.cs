using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.EverlastingDungeons.Scripts.Gui.Objects
{
    public class SpellInfoSlotGUI : MonoBehaviour
    {
        public Image icon;
        public Button button;
        public new TextMeshProUGUI name;
        public TextMeshProUGUI description;
        public TextMeshProUGUI level;
        public Image selected;
        public Image gray;
        public TextMeshProUGUI manaCost;
        private PlayerSpell spell;

        private void Start()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (GUIManager.Instance.selective.spellAssignment.selectedHotkeyIndex == -1)
                return;

            PrefabManager.Instance.activePlayer.Data.SetSpellIndex(spell.spellUUID, GUIManager.Instance.selective.spellAssignment.selectedHotkeyIndex);
            GUIManager.Instance.selective.spellAssignment.selectedUUID = spell.spellUUID;
        }

        public void SetSpell(PlayerSpell spell)
        {
            this.spell = spell;
            icon.sprite = spell.Spell.sprite;
            description.text = spell.Spell.description;
            name.text = spell.Spell.name;
            manaCost.text = $"{spell.Spell.manaCost.Format()} MP";
            level.text = $"Lv.{spell.level.Format()}";
        }

        private void UpdateData()
        {
            if (spell != null && PrefabManager.Instance.activePlayer != null)
            {
                selected.gameObject.SetActive(GUIManager.Instance.selective.spellAssignment.selectedUUID == spell.spellUUID);
                gray.gameObject.SetActive(!spell.Spell.CanEquip(PrefabManager.Instance.activePlayer));
                button.interactable = !gray.gameObject.activeSelf;
            }
        }

        private void FixedUpdate()
        {
            UpdateData();
        }
    }
}
