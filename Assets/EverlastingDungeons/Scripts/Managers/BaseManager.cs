using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Managers
{
    public abstract class BaseManager<T> : MonoBehaviour where T : BaseManager<T>
    {
        public static T Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)this;
                transform.parent = null;
                DontDestroyOnLoad(gameObject);
                Init();
                Debug.Log($"Initialized manager {name}");
            } 
            else
            {
                Destroy(gameObject);
            }
        }

        public abstract void Init();
    }
}
