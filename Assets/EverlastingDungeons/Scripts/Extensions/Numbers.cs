using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.EverlastingDungeons.Scripts.Extensions
{
    public static class Numbers
    {
        private static readonly string[] Suffixes =
        {
            "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc"
        };

        public static bool IsChance(this double c)
        {
            return UnityEngine.Random.value <= c;
        }

        public static string Format(this float c)
        {
            return c.ToString("#,0.##", CultureInfo.InvariantCulture);
        }

        public static string Format(this double c)
        {
            bool negative = c < 0;
            c = Math.Abs(c);

            int suffix = 0;
            while (c >= 1000 && suffix < Suffixes.Length - 1)
            {
                c /= 1000;
                suffix++;
            }

            string result = suffix == 0
                ? c.ToString("#,0.##", CultureInfo.InvariantCulture)
                : c.ToString(c % 1 == 0 ? "0" : "0.##", CultureInfo.InvariantCulture) + Suffixes[suffix];

            return negative ? "-" + result : result;
        }

        public static string Format(this long c)
        {
            return c.ToString("#,0", CultureInfo.InvariantCulture);
        }
    }
}
