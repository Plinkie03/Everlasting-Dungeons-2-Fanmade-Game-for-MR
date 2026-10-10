using Assets.EverlastingDungeons.Scripts.Gui.Objects;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.EverlastingDungeons.Scripts.Gui.Managers.Selective
{
    public class ConsumableAssignmentGUI : BaseAssignmentGUI<ConsumableHotkeySlotGUI>
    {
        protected override void FillContent()
        {
            foreach (var item in PrefabManager.Instance.activePlayer.Data.items)
            {
                if (item.Item is not ConsumableItemSO consumable)
                    continue;
                var slot = Instantiate(PrefabManager.Instance.itemSlot, content);
                slot.SetItem(item);
            }
        }
    }
}
