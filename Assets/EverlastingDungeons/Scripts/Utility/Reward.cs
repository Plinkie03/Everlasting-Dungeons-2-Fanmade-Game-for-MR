using Assets.EverlastingDungeons.Scripts.Entity.Base;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Utility
{
    [Serializable]
    public class Reward
    {
        public double xp, money, gems;

        public void Add(EntityBehavior from, double multiplier)
        {
            var gemsAcquired = Math.Round(gems * multiplier);
            var xpAcquired = Math.Round(xp * multiplier);
            var moneyAcquired = Math.Round(money * multiplier);

            if (from)
            {
                float mult = 1f;

                if (xpAcquired != 0)
                    from.CreateFloatingText($"{Icons.Xp} {xpAcquired.Format()}", Color.white, 1.5f, 0f);

                if (moneyAcquired != 0)
                    from.CreateFloatingText($"{Icons.Money} {moneyAcquired.Format()}", Color.yellow, 1.5f, PrefabManager.Instance.floatingText.GetComponent<RectTransform>().rect.height * mult++);

                if (gemsAcquired != 0)
                    from.CreateFloatingText($"{Icons.Gem} {gemsAcquired.Format()}", Color.red, 1.5f, PrefabManager.Instance.floatingText.GetComponent<RectTransform>().rect.height * mult);
            }

            PrefabManager.Instance.activePlayer.Data.gems += gemsAcquired;
            PrefabManager.Instance.activePlayer.Data.money += moneyAcquired;
            PrefabManager.Instance.activePlayer.AddXp(xpAcquired);
        }
    }
}
