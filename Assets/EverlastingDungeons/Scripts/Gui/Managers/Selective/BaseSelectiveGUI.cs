using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Gui.Managers.Selective
{
    public abstract class BaseSelectiveGUI : MonoBehaviour
    {
        public abstract void Refresh();
        public void Open() => gameObject.SetActive(true);
        public void Close() => gameObject.SetActive(false);
    }
}
