using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Gui.Objects
{
    public class SpellHotkeySlotGUI : BaseHotkeySlotGUI<PlayerSpell>
    {
        protected override bool IsAssigned => GUIManager.Instance.selective.spellAssignment.selectedHotkeyIndex == index;
        public override PlayerSpell Element => PrefabManager.Instance.activePlayer ? PrefabManager.Instance.activePlayer.Data.GetSpellByIndex(index) : default;

        protected override void OnClick()
        {
            if (!selected)
            {
                Element?.Execute();
            }
            else
            {
                GUIManager.Instance.selective.spellAssignment.selectedHotkeyIndex = index;
                GUIManager.Instance.selective.spellAssignment.selectedUUID = Element?.spellUUID;
            }
        }

        protected override Sprite Sprite => Element.Spell.sprite;
        protected override long TimeLeft => PrefabManager.Instance.activePlayer.AttackHandler.GetCooldownTime(Element.spellUUID);
        protected override bool IsAvailable => TimeLeft == 0 && Element.Spell.CanExecute(PrefabManager.Instance.activePlayer) && Element.Spell.CanEquip(PrefabManager.Instance.activePlayer);
    }
}
