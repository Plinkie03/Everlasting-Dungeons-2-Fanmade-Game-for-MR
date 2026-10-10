using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.World
{
    public class HitReg : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(ProcessDestroy());
        }

        private IEnumerator ProcessDestroy()
        {
            yield return new WaitForSeconds(0.75f);
            Destroy(gameObject);
        }
    }
}
