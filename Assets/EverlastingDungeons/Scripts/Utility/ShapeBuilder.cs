using Assets.EverlastingDungeons.Scripts.Entity.Base;
using Assets.EverlastingDungeons.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Utility
{
    public class ShapeBuilder
    {
        private readonly List<Vector2> positions = new();
        private Vector2 currentPos = Vector2.zero;

        public ShapeBuilder()
        {
            positions.Add(currentPos);
        }

        public ShapeBuilder Move(Vector2 direction, float offset)
        {
            currentPos += direction * offset;
            if (HasPosition(currentPos)) return this;
            positions.Add(currentPos);
            return this;
        }

        public ShapeBuilder Repeat(Func<ShapeBuilder, ShapeBuilder> action, int count)
        {
            for (int i = 0; i < count; i++)
            {
                action(this);
            }
            ResetPosition();
            return this;
        }

        public ShapeBuilder DrawAllNonDiagonal(float offset)
        {
            Right(offset);
            ResetPosition();
            Left(offset);
            ResetPosition();
            Up(offset);
            ResetPosition();
            Down(offset);
            ResetPosition();
            return this;
        }

        public ShapeBuilder DrawAllDiagonal(float offset)
        {
            DiagonalRightUp(offset);
            ResetPosition();
            DiagonalRightDown(offset);
            ResetPosition();
            DiagonalLeftUp(offset);
            ResetPosition();
            DiagonalLeftDown(offset);
            ResetPosition();
            return this;
        }

        public ShapeBuilder DrawAll(float offset)
        {
            DrawAllNonDiagonal(offset);
            DrawAllDiagonal(offset);
            return this;
        }

        public ShapeBuilder DiagonalRightUp(float offset = 1) => Move(new Vector2(1, 1), offset);
        public ShapeBuilder DiagonalRightDown(float offset = 1) => Move(new Vector2(1, -1), offset);
        public ShapeBuilder DiagonalLeftUp(float offset = 1) => Move(new Vector2(-1, 1), offset);
        public ShapeBuilder DiagonalLeftDown(float offset = 1) => Move(new Vector2(-1, -1), offset);

        public ShapeBuilder Up(float offset = 1) => Move(Vector2.up, offset);
        public ShapeBuilder Down(float offset = 1) => Move(Vector2.down, offset);
        public ShapeBuilder Left(float offset = 1) => Move(Vector2.left, offset);
        public ShapeBuilder Right(float offset = 1) => Move(Vector2.right, offset);

        public ShapeBuilder ResetPosition()
        {
            currentPos = Vector2.zero;
            return this;
        }

        public ShapeBuilder Line(Vector2 direction, float offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Move(direction, offset);
            }
            return this;
        }

        public bool HasPosition(Vector2 position) => positions.Contains(position);

        public Vector2[] Rotate(AnimationType type)
        {
            var newPositions = new Vector2[positions.Count];

            for (int i = 0; i < positions.Count; i++)
            {
                var pos = positions[i];

                if (type is AnimationType.IdleDown or AnimationType.MoveDown)
                    newPositions[i] = new Vector2(pos.y, -pos.x);     // ↓
                else if (type is AnimationType.IdleUp or AnimationType.MoveUp)
                    newPositions[i] = new Vector2(-pos.y, pos.x);     // ↑
                else if (type is AnimationType.IdleRight or AnimationType.MoveRight)
                    newPositions[i] = new Vector2(pos.x, pos.y);      // →
                else if (type is AnimationType.IdleLeft or AnimationType.MoveLeft)
                    newPositions[i] = new Vector2(-pos.x, -pos.y);    // ←
            }

            return newPositions;
        }

        public List<EntityBehavior> Draw(SpriteRenderer sprite, Vector2 startPos, AnimationType animation)
        {
            var entities = new List<EntityBehavior>();
            var positions = Rotate(animation);
            foreach (var pos in positions)
            {
                if (DungeonManager.Instance.floorPositions != null && !DungeonManager.Instance.floorPositions.Contains(Vector2Int.RoundToInt(startPos + pos)))
                    continue;

                var obj = GameObject.Instantiate(sprite, startPos + pos, Quaternion.identity);
                var hits = Physics2D.RaycastAll(startPos + pos, Vector2.zero, SettingsManager.Instance.entityLayer);
                foreach (var hit in hits)
                {
                    if (hit.collider != null && hit.collider.TryGetComponent(out EntityBehavior entity) && !entities.Contains(entity))
                        entities.Add(entity);
                }
            }
            return entities;
        }
    }
}
