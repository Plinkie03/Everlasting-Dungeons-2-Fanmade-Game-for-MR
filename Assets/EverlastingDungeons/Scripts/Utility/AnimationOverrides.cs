using Assets.EverlastingDungeons.Scripts.Entity.Base;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Utility
{
    public static class AnimationOverrides
    {
        private static readonly Dictionary<string, AnimatorOverrideController> Overrides = new();

        public static void Apply(Animator animator, EntityAnimationsSO animations)
        {
            if (Overrides.TryGetValue(animations.name, out var controller))
            {
                animator.runtimeAnimatorController = controller;
                return;
            }


            controller = new AnimatorOverrideController(animator.runtimeAnimatorController);
            var clips = new List<KeyValuePair<AnimationClip, AnimationClip>>();

            foreach (var clip in animator.runtimeAnimatorController.animationClips)
                clips.Add(new(clip, CachedReflection.GetInstanceValue<AnimationClip>(animations, clip.name)));

            controller.ApplyOverrides(clips);
            animator.runtimeAnimatorController = controller;
            Overrides.Add(animations.name, controller);
        }
    }
}
