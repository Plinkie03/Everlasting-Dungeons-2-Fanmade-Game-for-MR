using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.EverlastingDungeons.Scripts.Utility
{
    public interface ILeveable
    {
        double GetLevel();
        double GetXp();
        double GetReqXp();
        void SetLevel(double level);
        void SetXp(double xp);
    }

    public static class Leveable
    {
        public static bool AddXp(ILeveable leveable, double xpToAdd)
        {
            var currentXp = leveable.GetXp();
            var leveled = false;

            while (xpToAdd != 0)
            {
                var reqXp = leveable.GetReqXp();
                var toAdd = currentXp + xpToAdd > reqXp ? reqXp - currentXp : xpToAdd;
                xpToAdd -= toAdd;
                currentXp += toAdd;
                if (currentXp == reqXp)
                {
                    leveled = true;
                    currentXp = 0;
                    leveable.SetLevel(leveable.GetLevel() + 1);
                }
            }

            leveable.SetXp(currentXp);

            return leveled;
        }
    }
}
