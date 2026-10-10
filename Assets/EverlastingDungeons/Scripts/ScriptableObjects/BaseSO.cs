using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriInspector;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.ScriptableObjects
{
    public abstract class BaseSO<T> : ScriptableObject where T : BaseSO<T>
    {
        [ReadOnly]
        public string uuid = Guid.NewGuid().ToString();

        public new string name;


        [Button("Regenerate UUID")]
        private void RegenerateUUID()
        {
            uuid = Guid.NewGuid().ToString();
        }
    }
}
