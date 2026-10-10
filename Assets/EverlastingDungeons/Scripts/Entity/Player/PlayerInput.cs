using Assets.EverlastingDungeons.Scripts.Entity.Base;
using Assets.EverlastingDungeons.Scripts.Entity.Monster;
using Assets.EverlastingDungeons.Scripts.Entity.Other;
using Assets.EverlastingDungeons.Scripts.Generated;
using Assets.EverlastingDungeons.Scripts.Gui.Objects;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Assets.EverlastingDungeons.Scripts.Entity.Player
{
    public class PlayerInput : MonoBehaviour
    {
        private PlayerInputActions input;
        public LayerMask collisionLayer;
        public PlayerBehavior player;
        public float tileSize = 1f;

        public Vector2 moveInput;
        private bool isMoving;
        private Vector3 targetPosition;

        private bool mouseDown;

        private void Awake()
        {
            input = new PlayerInputActions();

            input.Player.Move.performed += ctx =>
                moveInput = ctx.ReadValue<Vector2>();

            input.Player.Move.canceled += ctx =>
                moveInput = Vector2.zero;

            input.UI.Inventory.performed += Inventory_performed;
            input.UI.Escape.performed += Escape_performed;
            input.UI.Gear.performed += Gear_performed;
            input.UI.Mastery.performed += Mastery_performed;
            input.UI.Spell.performed += Spell_performed;
            input.Developer.SpawnMob.performed += SpawnMob_performed;
            input.UI.Space.performed += Space_performed;
            input.UI.SpellAssignment.performed += SpellAssignment_performed;
            input.UI.ConsumableAssignment.performed += ConsumableAssignment_performed;
            input.UI.Consumables.performed += Consumables_performed;
            input.Player.AttackNearest.performed += AttackNearest_performed;
        }

        private void AttackNearest_performed(InputAction.CallbackContext obj)
        {
            var nearest = GameObject.FindObjectsByType<MonsterBehavior>().OrderBy(x => Vector2.Distance(player.transform.position, x.transform.position)).FirstOrDefault();
            if (nearest && player.AttackHandler.Target != nearest) 
                player.AttackHandler.SetTarget(nearest);
        }

        private void Consumables_performed(InputAction.CallbackContext obj)
        {
            var item = player.Data.GetItemByIndex(obj.action.GetBindingIndexForControl(obj.control));
            if (item?.Item is ConsumableItemSO consumable)
                consumable.Use(player, item);
        }

        private void ConsumableAssignment_performed(InputAction.CallbackContext obj)
        {
            GUIManager.Instance.Open(GUIManager.Instance.selective);
            GUIManager.Instance.selective.Open(GUIManager.Instance.selective.consumableAssignment);
        }

        private void Space_performed(InputAction.CallbackContext obj)
        {
            if (GUIManager.Instance.Current == GUIManager.Instance.main)
            {
                var dropItem = GUIManager.Instance.main.itemDropContent.GetComponentInChildren<ItemDropGUI>();
                if (dropItem)
                    dropItem.PickUp();
            }
            else if (GUIManager.Instance.selective.Current == GUIManager.Instance.selective.gear)
            {
                GUIManager.Instance.selective.gear.otherItemInfoGUI.OnClickEquip();
            }
        }

        private void SpellAssignment_performed(InputAction.CallbackContext obj)
        {
            GUIManager.Instance.Open(GUIManager.Instance.selective);
            GUIManager.Instance.selective.Open(GUIManager.Instance.selective.spellAssignment);
        }

        private void Spell_performed(InputAction.CallbackContext obj)
        {
            player.Data.GetSpellByIndex(obj.action.GetBindingIndexForControl(obj.control))?.Execute();
        }

        private void Mastery_performed(InputAction.CallbackContext obj)
        {
            GUIManager.Instance.Open(GUIManager.Instance.selective);
            GUIManager.Instance.selective.Open(GUIManager.Instance.selective.mastery);
        }

        private void Gear_performed(InputAction.CallbackContext obj)
        {
            GUIManager.Instance.Open(GUIManager.Instance.selective);
            GUIManager.Instance.selective.Open(GUIManager.Instance.selective.gear);
        }

        private void SpawnMob_performed(InputAction.CallbackContext obj)
        {
            PrefabManager.Instance.SpawnMonster(PrefabManager.Instance.activePlayer.transform.position + Vector3.up);
        }

        private void Escape_performed(InputAction.CallbackContext obj)
        {
            if (GUIManager.Instance.Current != GUIManager.Instance.main)
                GUIManager.Instance.Open(GUIManager.Instance.main);
            else if (GUIManager.Instance.Current == GUIManager.Instance.main)
            {
                GUIManager.Instance.Open(GUIManager.Instance.selective);
                GUIManager.Instance.selective.Open(GUIManager.Instance.selective.settings);
            }
        }

        private void Inventory_performed(InputAction.CallbackContext obj)
        {
            GUIManager.Instance.Open(GUIManager.Instance.selective);
            GUIManager.Instance.selective.Open(GUIManager.Instance.selective.inventory);
        }

        private bool IsBlocked(Vector3 targetPos)
        {
            float radius = 0.2f; // adjust to your player size

            return Physics2D.OverlapCircle(
                targetPos,
                radius,
                collisionLayer
            );
        }

        private void OnEnable() => input.Enable();
        private void OnDisable() => input.Disable();

        private void ProcessMove()
        {
            // Start new movement if idle and input exists
            if (!isMoving)
            {
                Vector2 dir = GetCardinalDirection(moveInput);

                if (dir != Vector2.zero)
                {
                    Vector3 potentialTarget = player.transform.position + new Vector3(dir.x, dir.y, 0) * tileSize;

                    if (!IsBlocked(potentialTarget))
                    {
                        targetPosition = potentialTarget;
                        isMoving = true;
                    }
                }
            }

            // Move toward target
            if (isMoving)
            {
                player.transform.position = Vector3.MoveTowards(
                    player.transform.position,
                    targetPosition,
                    (float)player.moddedStats.GetStat(StatType.Speed) * Time.deltaTime
                );

                // Snap & finish movement
                if (Vector3.Distance(player.transform.position, targetPosition) < 0.001f)
                {
                    player.transform.position = targetPosition;
                    isMoving = false;
                }
            }
        }

        private void ProcessClick()
        {
            var mouse = Mouse.current;
            var touchscreen = Touchscreen.current;

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            bool pressed = false;
            Vector2 screenPos = default;

            if (mouse != null && mouse.leftButton.isPressed)
            {
                pressed = true;
                screenPos = mouse.position.ReadValue();
            }
            else if (touchscreen != null && touchscreen.primaryTouch.press.isPressed)
            {
                pressed = true;
                screenPos = touchscreen.primaryTouch.position.ReadValue();
            }

            if (!mouseDown && pressed)
            {
                mouseDown = true;

                // Ignore taps on UI.
                if (touchscreen != null &&
                    touchscreen.primaryTouch.press.isPressed &&
                    EventSystem.current.IsPointerOverGameObject(
                        touchscreen.primaryTouch.touchId.ReadValue()))
                {
                    return;
                }

                Vector3 clickPos = Camera.main.ScreenToWorldPoint(screenPos);

                var raycasts = Physics2D.RaycastAll(
                    clickPos,
                    Vector2.zero,
                    SettingsManager.Instance.entityLayer
                );

                foreach (var raycast in raycasts)
                {
                    if (raycast.collider == null)
                        continue;

                    if (raycast.collider.TryGetComponent<EntityBehavior>(out var cc)
                        && cc is MonsterBehavior mob)
                    {
                        if (mob == player.attackHandler.Target)
                            player.attackHandler.SetTarget(default);
                        else
                            player.attackHandler.SetTarget(mob);

                        break;
                    }
                    else if (raycast.collider.TryGetComponent<NPCBehavior>(out var npc))
                    {
                        npc.Interact();
                        break;
                    }
                }
            }
            else if (mouseDown && !pressed)
            {
                mouseDown = false;
            }
        }

        private void Update()
        {
            ProcessMove();
            ProcessClick();
        }

        private Vector2 GetCardinalDirection(Vector2 input)
        {
            if (input == Vector2.zero)
                return Vector2.zero;

            // Prevent diagonal movement
            if (Mathf.Abs(input.x) >= Mathf.Abs(input.y))
                return new Vector2(Mathf.Sign(input.x), 0);
            else
                return new Vector2(0, Mathf.Sign(input.y));
        }
    }
}
