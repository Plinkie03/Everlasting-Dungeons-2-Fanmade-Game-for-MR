using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.EverlastingDungeons.Scripts.Utility
{
    public static class Icons
    {
        public static readonly string Money = Icon("money");
        public static readonly string Gem = Icon("gem");
        public static readonly string Xp = Icon("xp");

        private static string Icon(string str) => $"<sprite name=\"{str}\">";
    }
}
