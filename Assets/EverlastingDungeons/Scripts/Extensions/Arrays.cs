using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.EverlastingDungeons.Scripts.Extensions
{
    public static class Arrays
    {
        public static T Random<T>(this T[] arr)
        {
            return arr[(int)UnityEngine.Random.Range(0, arr.Length)];
        }

        public static T Random<T>(this List<T> arr)
        {
            return arr[(int)UnityEngine.Random.Range(0, arr.Count)];
        }

        public static T Random<T>(this IEnumerable<T> arr) => arr.ToArray().Random();
    }
}
