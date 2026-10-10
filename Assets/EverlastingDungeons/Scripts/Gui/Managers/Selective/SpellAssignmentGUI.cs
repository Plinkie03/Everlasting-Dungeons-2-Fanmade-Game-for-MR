using Assets.EverlastingDungeons.Scripts.Gui.Objects;
using Assets.EverlastingDungeons.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.EverlastingDungeons.Scripts.Gui.Managers.Selective
{
    public class SpellAssignmentGUI : BaseAssignmentGUI<SpellHotkeySlotGUI>
    {

        protected override void FillContent()
        {
            foreach (var spell in PrefabManager.Instance.activePlayer.Data.spells)
            {
                var slot = Instantiate(PrefabManager.Instance.spellInfoSlot, content);
                slot.SetSpell(spell);
            }
        }
    }
}
