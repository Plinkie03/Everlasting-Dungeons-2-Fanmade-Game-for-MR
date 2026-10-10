using Assets.EverlastingDungeons.Scripts.Entity.Base;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using Assets.EverlastingDungeons.Scripts.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Utility
{
    public enum EntityClassType
    {
        Warrior,
        Mage,
        Archer
    }

    public class EntityClass
    {
        public static readonly Dictionary<EntityClassType, EntityClass> Classes = new()
        {
            { EntityClassType.Warrior, new EntityClass(EntityClassType.Warrior, attackRange: 1.5f, defenseMultiplier: 1.5f, damageMultiplier: 1.0f, healthMultiplier: 1.5f) },
            { EntityClassType.Mage, new EntityClass(EntityClassType.Mage, attackRange: 4.5f, defenseMultiplier: 0.75f, damageMultiplier: 1.5f, healthMultiplier: 0.75f) },
            { EntityClassType.Archer, new EntityClass(EntityClassType.Archer, attackRange: 4.5f, defenseMultiplier: 1.0f, damageMultiplier: 1.25f, healthMultiplier: 1.0f) }
        };

        public float defenseMultiplier;
        public float damageMultiplier;
        public float attackRange;
        public float healthMultiplier;
        public EntityClassType classType;

        private EntityClass(EntityClassType classType, float attackRange, float defenseMultiplier, float damageMultiplier, float healthMultiplier)
        {
            this.classType = classType;
            this.attackRange = attackRange;
            this.defenseMultiplier = defenseMultiplier;
            this.damageMultiplier = damageMultiplier;
            this.healthMultiplier = healthMultiplier;
        }

        public StatType CombatStat => classType switch
        {
            EntityClassType.Warrior => StatType.Melee,
            EntityClassType.Mage => StatType.Magic,
            EntityClassType.Archer => StatType.Distance,
        };

        public static SpriteRenderer GetMarkerOf(EntityClassType classType) => classType switch
        {
            EntityClassType.Warrior => PrefabManager.Instance.physicalMarker,
            EntityClassType.Mage => PrefabManager.Instance.magicalMarker,
            EntityClassType.Archer => PrefabManager.Instance.rangedMarker,
            _ => default
        };

        public static Projectile GetProjectileOf(EntityClassType classType) => classType switch
        {
            EntityClassType.Warrior => default,
            EntityClassType.Mage => PrefabManager.Instance.magic,
            EntityClassType.Archer => PrefabManager.Instance.arrow,
            _ => default
        };

        public static EntityClass GetClass(WeaponType weaponType) => weaponType switch
        {
            WeaponType.Sword => Classes[EntityClassType.Warrior],
            WeaponType.Staff => Classes[EntityClassType.Mage],
            WeaponType.Bow => Classes[EntityClassType.Archer],
            _ => default
        };
    }
}
