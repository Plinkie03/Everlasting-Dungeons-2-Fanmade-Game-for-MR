using Assets.EverlastingDungeons.Scripts.Animations;
using Assets.EverlastingDungeons.Scripts.Entity.Monster;
using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Gui.Objects;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using Assets.EverlastingDungeons.Scripts.Utility;
using Assets.EverlastingDungeons.Scripts.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.EverlastingDungeons.Scripts.Managers
{
    public class PrefabManager : BaseManager<PrefabManager>
    {
        public PlayerBehavior activePlayer;

        public new Camera camera;
        public PlayerBehavior player;
        public ItemSlotGUI itemSlot;
        public MonsterBehavior monster;
        public Transform itemStatSlot;
        public FloatingItemInfoGUI floatingItemInfo;
        public GUIFloatingText floatingText;
        public LeaderboardSlotGUI leaderboardSlot;
        public Projectile arrow, magic;
        public TextMeshProUGUI notification;
        public Transform itemLevelSlot;
        public SellItemSlotGUI sellItemSlot;
        public SpellInfoSlotGUI spellInfoSlot;
        public Transform itemPriceSlot;
        public WorldItem worldItem;
        public SpriteRenderer corpse;
        public WorldLadder worldLadder;
        public SpriteRenderer hitMarker, physicalMarker, rangedMarker, magicalMarker, hitReg, targetSelector;
        public ItemDropGUI itemDrop;
        public Toggle statToggle;

        public override void Init()
        {
            
        }

        public FloatingItemInfoGUI CreateFloatingItemInfo(Transform parent, PlayerInventoryItem item)
        {
            var info = Instantiate(floatingItemInfo, parent);
            info.SetItem(item);
            return info;
        }

        public WorldItem CreateWorldItem(Vector3 pos, BaseItemSO item, double level, double amount, double rarityMultiplier)
        {
            var worldItem = Instantiate(this.worldItem, pos.Snap(), Quaternion.identity);
            worldItem.SetItem(item, level, amount, rarityMultiplier);
            return worldItem;
        }

        public GUIFloatingText CreateFloatingText(Vector2 pos, string text, Color color, float scale)
        {
            var txt = Instantiate(floatingText, pos, Quaternion.identity);
            txt.SetScale(scale, 0.8f, true);
            txt.SetFloat(1f, 1.2f);
            txt.SetText(text);
            txt.SetColor(color);
            return txt;
        }

        public void CreateHitAnimation(Hit hit)
        {
            CreateFloatingText(hit.target.transform.position + Vector3.up * hit.target.GetTextOffset(), hit.Text, hit.Color, hit.Scale);
            
            var marker = Instantiate(hitMarker, hit.target.transform.position, Quaternion.identity);
            marker.color = hit.Color;

            if (!hit.options.disableAnimations)
            {
                Instantiate(EntityClass.GetMarkerOf(hit.source.GetClass().classType), hit.target.transform.position, Quaternion.identity);

                var projectilePrefab = EntityClass.GetProjectileOf(hit.source.GetClass().classType);
                if (projectilePrefab == null) return;
                var projectile = Instantiate(projectilePrefab, hit.source.transform.position, Quaternion.identity);
                projectile.SetDestination(hit.target.transform.position);
                projectile.Rotate();
            }
        }

        public MonsterBehavior SpawnMonster(Vector2 position, double? level = null, MonsterSO data = default, Mutation mutation = default)
        {
            data ??= ResourceManager.Instance.monsters.Raw.Where(x => !x.boss && Aura.GetFloorLevel(PrefabManager.Instance.activePlayer.Data.currentFloor) >= x.floorLevel).OrderByDescending(x => x.floorLevel).Take(3).Random();
            var mob = Instantiate(monster, position.Snap(), Quaternion.identity);
            if (mutation != null)
                mob.mutation = mutation;
            mob.level = level.GetValueOrDefault(PrefabManager.Instance.activePlayer.Data.GetFloorLevel());
            mob.data = data;
            return mob;
        }

        public void AttachCamera(GameObject to)
        {
            Instantiate(camera, to.transform);
        }

        public PlayerBehavior SpawnPlayer(Vector2 position)
        {
            if (activePlayer)
            {
                Destroy(activePlayer.gameObject);
                activePlayer = null;
            }
            activePlayer = Instantiate(player, position.Snap(), Quaternion.identity);
            return activePlayer;
        }
    }
}
