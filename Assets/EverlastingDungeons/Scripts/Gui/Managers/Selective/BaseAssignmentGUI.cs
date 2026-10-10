using Assets.EverlastingDungeons.Scripts.Gui.Objects;
using Assets.EverlastingDungeons.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Gui.Managers.Selective
{
    public abstract class BaseAssignmentGUI<T> : BaseSelectiveGUI
    {
        public T[] hotkeys;
        public Transform content;
        public int selectedHotkeyIndex = -1;
        public string selectedUUID = null;

        public override void Refresh()
        {
            ClearContent();
            FillContent();
        }

        protected abstract void FillContent();

        private void ClearContent()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }
    }
}
