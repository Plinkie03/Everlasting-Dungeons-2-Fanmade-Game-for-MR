using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Entity Animation", menuName = "Everlasting Dungeons/Entity/Animation", order = 1)]
    public class EntityAnimationsSO : BaseSO<EntityAnimationsSO>
    {
        public AnimationClip idleUp;
        public AnimationClip idleDown;
        public AnimationClip idleLeft;
        public AnimationClip idleRight;
        public AnimationClip moveUp;
        public AnimationClip moveDown;
        public AnimationClip moveLeft;
        public AnimationClip moveRight;
        public Sprite dead;
    }
}
