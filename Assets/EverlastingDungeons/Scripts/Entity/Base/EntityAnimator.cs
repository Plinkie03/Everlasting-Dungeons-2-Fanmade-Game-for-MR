using Assets.EverlastingDungeons.Scripts.Entity.Monster;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Entity.Base
{
    public enum AnimationType
    {
        IdleDown,
        IdleLeft,
        IdleRight,
        IdleUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        MoveUp
    }

    public class EntityAnimator : MonoBehaviour
    {
        public static readonly float WalkSoundSpeedMultiplier = 0.8f;
        public EntityBehavior entity;
        public Animator animator;
        public SpriteRenderer spriteRenderer;
        private Vector2 lastPosition;
        private AnimationType currentAnimation;

        private void Start()
        {
            AnimationOverrides.Apply(animator, entity.GetAnimations());
        }

        public AnimationType Animation => currentAnimation;

        private void Play(AnimationType type)
        {
            if (currentAnimation == type)
                return;

            animator.SetFloat("Action", (int)type);
            animator.SetFloat("Speed", (float)(entity.ModdedStats.GetStat(StatType.Speed) / EntityBaseStats.DefaultSpeed));

            currentAnimation = type;
        }

        private Vector2 GetDifference()
        {
            return (Vector2)transform.position - lastPosition;
        }

        private void Update()
        {
            var diff = GetDifference();
            if (diff == Vector2.zero && entity.walkSound)
                entity.walkSound.Pause();
            else if (entity.walkSound)
            {
                entity.walkSound.pitch = (float)(entity.ModdedStats.GetStat(StatType.Speed) / (EntityBaseStats.DefaultSpeed * WalkSoundSpeedMultiplier));
                entity.walkSound.UnPause();
            }

            const float threshold = 0.01f;

            if (diff.x > threshold)
                Play(AnimationType.MoveRight);
            else if (diff.x < -threshold)
                Play(AnimationType.MoveLeft);
            else if (diff.y > threshold)
                Play(AnimationType.MoveUp);
            else if (diff.y < -threshold)
                Play(AnimationType.MoveDown);
            else
            {
                if (entity.AttackHandler.Target && entity is MonsterBehavior)
                {
                    Vector2 dir =
                        entity.AttackHandler.Target.transform.position - entity.transform.position;

                    if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                    {
                        if (dir.x > 0)
                            Play(AnimationType.IdleRight);
                        else
                            Play(AnimationType.IdleLeft);
                    }
                    else
                    {
                        if (dir.y > 0)
                            Play(AnimationType.IdleUp);
                        else
                            Play(AnimationType.IdleDown);
                    }
                }
                else
                {
                    switch (currentAnimation)
                    {
                        case AnimationType.MoveDown: Play(AnimationType.IdleDown); break;
                        case AnimationType.MoveLeft: Play(AnimationType.IdleLeft); break;
                        case AnimationType.MoveRight: Play(AnimationType.IdleRight); break;
                        case AnimationType.MoveUp: Play(AnimationType.IdleUp); break;
                    }
                }
            }

            lastPosition = transform.position;
        }
    }
}
