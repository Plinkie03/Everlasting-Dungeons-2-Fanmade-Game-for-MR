using Assets.EverlastingDungeons.Scripts.Entity.Base;
using Assets.EverlastingDungeons.Scripts.Entity.Monster;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Entity.Player
{
    public class PlayerBehavior : EntityBehavior, ILeveable
    {
        public static readonly float DefenseBreakMultiplier = 0.15f;
        public static readonly float TextOffset = 1f;
        public EntityAnimationsSO animations;
        public PlayerBaseStats baseStats;
        public PlayerModdedStats moddedStats;
        public PlayerAttackHandler attackHandler;
        public PlayerGear gear;

        public override EntityAttackHandler AttackHandler => attackHandler;
        public override EntityBaseStats BaseStats => baseStats;
        public override EntityModdedStats ModdedStats => moddedStats;

        public PlayerData Data => AuthenticationManager.Instance.data;
        public override EntityAnimationsSO GetAnimations() => animations;
        public PlayerInput input;
        public AudioSource townMusic, dungeonMusic, wishStatsSound, buySound, sellSound, rebirthSound;
        public Transform potSounds, lootSounds;
        private readonly HashSet<EntityBehavior> mobsThatAttached = new();
        
        public override string GetName() => Data.username;
        public override EntityClass GetClass()
        {
            var weapon = gear.GetEquippedPiece(GearType.Weapon);
            if (weapon == null || weapon.Item is not GearItemSO weap)
            {
                return EntityClass.GetClass(default);
            }
            else
            {
                return EntityClass.GetClass(weap.weaponType);
            }
        }
        
        private void Awake()
        {
            PrefabManager.Instance.AttachCamera(gameObject);
            if (SceneManager.Instance.Current is SceneType.DungeonScene)
                Camera.main.backgroundColor = DungeonManager.Instance.sprites.color;
            GUIManager.Instance.Open(Data.passedTutorial ? GUIManager.Instance.main : GUIManager.Instance.introduction);
        }

        public override void Start()
        {
            base.Start();

            GUIManager.Instance.main.SetBossbarData(null);

            InvokeRepeating(nameof(RefreshMobsThatAttacked), 0f, 3f);
            
            // Scan
            AstarPath.active.Scan();
        }

        private void RefreshMobsThatAttacked() => mobsThatAttached.Clear();

        public override void Die() => ProcessDeath();

        private async void ProcessDeath()
        {
            await SceneManager.Instance.MoveToScene(SceneType.TownScene);
        }

        public void Notify(string text, Color color)
        {
            GUIManager.Instance.main.Notify(text, color);
        }

        public override void TakeDamage(Hit hit)
        {
            // Only if another mob attacks
            if (mobsThatAttached.Count > 1)
            {
                hit.options.damageMultiplier += (mobsThatAttached.Count - 1) * DefenseBreakMultiplier;
                hit.CalculateDamage();
            }
            mobsThatAttached.Add(hit.source);
            
            base.TakeDamage(hit);
            
            PlayRandomFrom(damagedSounds);
            Data.masteries.endurance.AddXp(2);
            Data.masteries.defense.AddXp(1);
            Instantiate(PrefabManager.Instance.hitReg, hit.source.transform);
        }

        public bool AddXp(double xp)
        {
            var levelUp = Leveable.AddXp(this, xp);
            if (levelUp)
            {
                Heal(ModdedStats.GetStat(StatType.MaxHealth));
                HealMana(ModdedStats.GetStat(StatType.MaxMana));
                CreateFloatingText($"Level Up ({Data.level.Format()})", Color.yellow);
            }

            return levelUp;
        }
        public double GetLevel() => Data.level;
        public double GetXp() => Data.xp;
        public double GetReqXp() => 50L * Data.level * (Data.level + 1);
        public void SetLevel(double level) => Data.level = level;
        public void SetXp(double xp) => Data.xp = xp;
    }
}
