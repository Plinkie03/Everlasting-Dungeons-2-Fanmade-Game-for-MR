using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Extensions
{
    public static class Vectors
    {
        public static Vector2 Snap(this Vector2 pos)
        {
            return new Vector2(
                Mathf.Round(pos.x),
                Mathf.Round(pos.y)
                );
        }
        public static Vector3 Snap(this Vector3 pos)
        {
            return new Vector3(
                Mathf.Round(pos.x),
                Mathf.Round(pos.y),
                pos.z
                );
        }
    }
}
