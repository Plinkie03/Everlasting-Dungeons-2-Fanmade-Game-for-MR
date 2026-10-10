using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Entity.Base
{
    public abstract class EntityBehavior : MonoBehaviour
    {
        public abstract EntityBaseStats BaseStats { get; }
        public abstract EntityModdedStats ModdedStats { get; }
        public abstract EntityAttackHandler AttackHandler { get; }
        public EntityAnimator animator;
        public EntityGUI gui;
        public double health;
        public double mana;
        public AudioSource walkSound;
        public Transform basicHitSounds, damagedSounds, specialAttackSounds;

        public abstract EntityAnimationsSO GetAnimations();

        public virtual void Start()
        {
            mana = ModdedStats.GetStat(Utility.StatType.MaxMana);
            health = ModdedStats.GetStat(Utility.StatType.MaxHealth);
            UpdateGui();
        }

        public void PlayRandomFrom(Transform from) => from.GetComponents<AudioSource>().Random().Play();

        private void UpdateGui()
        {
            var maxHp = ModdedStats.GetStat(Utility.StatType.MaxHealth);
            var maxMp = ModdedStats.GetStat(StatType.MaxMana);
            
            if (health > maxHp)
                health = maxHp;
            if (mana > maxMp) 
                mana = maxMp;
            
            gui.name.text = GetName();
            gui.bar.SetValue(health, maxHp);
        }

        public void CreateFloatingText(string text, Color color, float scale = 1.2f, float? useOwnOffset = null) => PrefabManager.Instance.CreateFloatingText(transform.position + Vector3.up * (useOwnOffset.HasValue ? useOwnOffset.Value : GetTextOffset()), text, color, 1.2f);

        public void HealMana(double amount)
        {
            amount = Math.Round(amount);
            if (amount == 0)
                return;
            CreateFloatingText($"+{amount.Format()}", Color.blue);
            mana += Math.Min(amount, ModdedStats.GetStat(StatType.MaxMana) - mana);
        }

        public void Heal(double amount)
        {
            amount = Math.Round(amount);
            if (amount == 0)
                return;
            CreateFloatingText($"+{amount.Format()}", Color.green);
            health += Math.Min(amount, ModdedStats.GetStat(StatType.MaxHealth) - health);
        }

        public bool IsDead => health == 0;
        public abstract string GetName();
        public float GetTextOffset() => gui.transform.localPosition.y + 0.2f;
        public abstract EntityClass GetClass();

        public virtual void TakeDamage(Hit hit)
        {
            if (IsDead)
                return;
            var damage = Math.Max(Math.Min(health, hit.Damage), 0);
            health -= damage;
            hit.source.Heal(damage * hit.source.ModdedStats.GetStat(StatType.Lifesteal));
            PrefabManager.Instance.CreateHitAnimation(hit);
            UpdateGui();
            if (IsDead)
                Die();
        }

        public abstract void Die();

        private void FixedUpdate()
        {
            UpdateGui();
        }
    }
}
