using Assets.EverlastingDungeons.Scripts.Entity.Base;
using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using Assets.EverlastingDungeons.Scripts.Utility;
using Assets.EverlastingDungeons.Scripts.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Entity.Monster
{
    public class MonsterBehavior : EntityBehavior
    {
        public MonsterSO data;
        public MonsterBaseStats baseStats;
        public MonsterModdedStats moddedStats;
        public MonsterAI ai;
        public Mutation mutation;
        public EntityAttackHandler attackHandler;
        public double level = 1;
        private List<WorldLadder> ladders = new();
        public bool allowedLadderSpawn = false;

        public override EntityAttackHandler AttackHandler => attackHandler;
        public override EntityBaseStats BaseStats => baseStats;
        public override EntityModdedStats ModdedStats => moddedStats;
        public override string GetName() => $"<color={Aura.color.ToHex(false)}>{(mutation.type == MutationType.None ? string.Empty : $"{mutation.type} ")}{(Aura.type is AuraType.None ? string.Empty : $"{Aura.type} ")}{data.name} Lv.{level.Format()}</color>";
        public override EntityClass GetClass() => EntityClass.Classes[mutation.classType.GetValueOrDefault(data.classType)];
        public override EntityAnimationsSO GetAnimations() => data.animations;
        public Aura Aura => Aura.GetAuraForLevel(level);

        private void Awake()
        {
            mutation ??= Mutation.RandomMutation;
        }

        public override void Start()
        {
            animator.transform.localScale = Vector3.one * data.scale;
            animator.transform.localScale *= (float)mutation.scaleMultiplier;
            animator.transform.localPosition = Vector3.up * 0.5f * (data.scale * (float)mutation.scaleMultiplier - 1);
            gui.transform.localPosition = Vector3.up * gui.transform.localPosition.y * animator.transform.localScale.y;
            Color final = data.color;
            if (Aura.color != Color.white)
                final = Aura.color;
            if (mutation.color != Color.white)
                final = mutation.color;
            animator.spriteRenderer.color = final;

            if (data.boss && allowedLadderSpawn)
            {
                ladders.Add(Instantiate(PrefabManager.Instance.worldLadder, transform.position, Quaternion.identity));

                var hellLadder = Instantiate(PrefabManager.Instance.worldLadder, transform.position + Vector3.up * 2, Quaternion.identity);
                hellLadder.GetComponent<SpriteRenderer>().color = Color.red;
                hellLadder.config = new RoomConfigurationOptions { onlyBoss = true };

                ladders.Add(hellLadder);

                var champLadder = Instantiate(PrefabManager.Instance.worldLadder, transform.position + Vector3.right * 2, Quaternion.identity);
                champLadder.GetComponent<SpriteRenderer>().color = Color.orange;
                champLadder.config = new RoomConfigurationOptions { onlyMutation = Mutation.Get(MutationType.Champion) };

                ladders.Add(champLadder);

                var bossChampLadder = Instantiate(PrefabManager.Instance.worldLadder, transform.position + Vector3.right * 2 + Vector3.up * 2, Quaternion.identity);
                bossChampLadder.GetComponent<SpriteRenderer>().color = Color.black;
                bossChampLadder.config = new RoomConfigurationOptions { onlyBoss = true, onlyMutation = Mutation.Get(MutationType.Champion) };

                ladders.Add(bossChampLadder);

                gui.gameObject.SetActive(false);
            }

            base.Start();
        }

        private void FixedUpdate()
        {
            if (data.boss && allowedLadderSpawn)
                GUIManager.Instance.main.SetBossbarData(this);
            attackHandler.SetTarget(PrefabManager.Instance.activePlayer);
        }

        public override void Die()
        {
            // Corpse 
            var corpse = Instantiate(PrefabManager.Instance.corpse, transform.position.Snap(), Quaternion.identity);
            corpse.sprite = data.animations.dead;
            corpse.transform.localScale = animator.transform.localScale;
            corpse.color = animator.spriteRenderer.color;

            if (data.boss && allowedLadderSpawn)
                GUIManager.Instance.main.SetBossbarData(this);

            data.rewards.Add(this, Math.Pow(level * mutation.rewardMultiplier, Aura.exponent));

            var drops = new List<ItemDrop>();
            data.drops.ForEach(drops.Add);
            data.dropCollection.ForEach(x => x.drops.ForEach(drops.Add));

            foreach (var item in drops)
            {
                if ((item.chance * mutation.dropMultiplier * (data.boss ? PrefabManager.Instance.activePlayer.Data.currentFloor : 1f) * Aura.dropChance).IsChance())
                {
                    PrefabManager.Instance.CreateWorldItem(transform.position, item.item, level, item.RandomAmount, mutation.rarityMultiplier);
                }
            }

            if (ladders.Count != 0)
                ladders.ForEach(x => x.gameObject.SetActive(true));

            Destroy(gameObject);
        }

        public override void TakeDamage(Hit hit)
        {
            base.TakeDamage(hit);
            PrefabManager.Instance.activePlayer.Data.masteries.GetCombatMastery(PrefabManager.Instance.activePlayer.GetClass().classType).AddXp(1);
        }
    }
}
