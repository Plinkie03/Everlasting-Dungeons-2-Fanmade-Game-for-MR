using Assets.EverlastingDungeons.Scripts.Entity.Base;
using Assets.EverlastingDungeons.Scripts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Utility
{
    public class HitOptions
    {
        public bool forceCritical;
        public bool forceMiss;
        public bool forceHit;
        public bool forceBlock;
        public bool ignoreDefense;
        public float damageMultiplier = 1f;
        public bool disableAnimations;

        public HitOptions() { }
    }

    public class Hit
    {
        public EntityBehavior source;
        public EntityBehavior target;
        public HitOptions options;

        public bool isMiss;
        public bool isCritical;
        public bool isBlock;
        private double damage;

        public Hit(EntityBehavior source, EntityBehavior target, HitOptions options = null)
        {
            this.source = source;
            this.target = target;
            this.options = options ?? new HitOptions();
            CalculateDamage();
        }

        public void CalculateDamage()
        {
            try
            {
                damage = source.ModdedStats.GetRandomDamage() * options.damageMultiplier;

                isCritical = source.ModdedStats.GetStat(StatType.CriticalRate).IsChance() || options.forceCritical;
                isBlock = target.ModdedStats.GetStat(StatType.BlockRate).IsChance() || options.forceBlock;
                isMiss = false;

                if (isCritical)
                    damage *= source.ModdedStats.GetStat(StatType.CriticalMultiplier);

                damage *= 1f - target.ModdedStats.GetStat(StatType.DamageResistance);
                damage *= 1f - target.ModdedStats.GetResistanceAgainst(source);
                damage -= options.ignoreDefense ? 0 : target.ModdedStats.GetStat(StatType.Defense);

                if (isBlock)
                    damage *= 1f - target.ModdedStats.GetStat(StatType.BlockReduction);
            }
            catch
            {
                damage = 0f;
            }
        }

        public string Text => isMiss ? "Miss" : $"{(Damage > 0f ? $"-{Damage.Format()}" : "0")}";
        public Color Color => isMiss ? Color.gray : isCritical ? Color.orange : isBlock ? Color.cyan : Color.red;
        public float Scale => isCritical ? 1.75f : 1.35f;
        public double Damage
        {
            get
            {
                return Math.Round(damage);
            }

            set
            {
                damage = value;
            }
        }
    }
}
