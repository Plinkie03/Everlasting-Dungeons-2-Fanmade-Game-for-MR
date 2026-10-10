using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.World
{
    public class Projectile : MonoBehaviour
    {
        public static readonly float Speed = 12f;

        public SpriteRenderer spriteRenderer;

        private Vector3 destination;

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }

        public void SetDestination(Vector3 pos)
        {
            destination = pos;
        }

        public void Rotate()
        {
            Vector2 direction = destination - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void FixedUpdate()
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Speed * Time.deltaTime);
            if (transform.position == destination)
                Destroy(gameObject);
        }
    }
}
