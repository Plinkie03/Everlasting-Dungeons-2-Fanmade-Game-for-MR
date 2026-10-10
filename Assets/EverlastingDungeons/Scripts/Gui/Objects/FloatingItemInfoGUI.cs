using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.EverlastingDungeons.Scripts.Gui.Objects
{
    public class FloatingItemInfoGUI : BaseItemInfoGUI
    {
        public static FloatingItemInfoGUI Current;


        private void Start()
        {
            if (Current != null)
                Destroy(Current.gameObject);

            Current = this;

            Position();
        }

        private void OnDestroy()
        {
            if (Current == this)
                Current = null;
        }

        private void OnDisable()
        {
            Destroy(gameObject);
        }

        private void Position()
        {
            RectTransform rect = GetComponent<RectTransform>();

            Vector2 mousePos = Touchscreen.current?.primaryTouch?.position.ReadValue() ?? Mouse.current.position.ReadValue();

            bool rightSide = mousePos.x > Screen.width * 0.5f;
            bool bottomSide = mousePos.y < Screen.height * 0.5f;

            float offset = 20f;

            // Initial pivot based on screen quadrant
            rect.pivot = new Vector2(
                rightSide ? 1f : 0f,
                bottomSide ? 0f : 1f
            );

            // Initial position
            Vector2 pos = mousePos + new Vector2(
                rightSide ? -offset : offset,
                bottomSide ? offset : -offset
            );

            // Get tooltip size
            Vector2 size = rect.rect.size;

            // Calculate bounds
            float left = pos.x - rect.pivot.x * size.x;
            float right = left + size.x;

            float bottom = pos.y - rect.pivot.y * size.y;
            float top = bottom + size.y;

            // Clamp horizontally
            if (left < 0)
                pos.x -= left;

            if (right > Screen.width)
                pos.x -= right - Screen.width;

            // Clamp vertically
            if (bottom < 0)
                pos.y -= bottom;

            if (top > Screen.height)
                pos.y -= top - Screen.height;

            // Apply final position
            transform.position = pos;
        }

        private void Update()
        {
            Position();
        }
    }
}
