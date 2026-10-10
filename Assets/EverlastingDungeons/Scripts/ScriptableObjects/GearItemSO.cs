using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Gui.Objects;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriInspector;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.ScriptableObjects
{
    public enum GearType
    {
        Helmet,
        Chestplate,
        Boots,
        Gloves,
        Weapon,
        Shield
    }

    public enum WeaponType
    {
        Sword,
        Staff,
        Bow
    }

    [CreateAssetMenu(menuName = "Everlasting Dungeons/Items/Gear Item", fileName = "New Gear Item")]
    public class GearItemSO : BaseItemSO
    {
        public override bool IsSellable => true;
        public override bool IsStackable => false;

        [EnumToggleButtons]
        public GearType gearType;

        [ShowIf(nameof(gearType), GearType.Weapon), EnumToggleButtons]
        public WeaponType weaponType;

        public StatVariance statVariance = new StatVariance();

        [TableList()]
        public List<Stat> defaultStats = new();

        public override void FillInfo(BaseItemInfoGUI info, PlayerInventoryItem item)
        {
            foreach (var stat in Enum.GetValues(typeof(StatType)))
            {
                var statType = (StatType)stat;
                var absolute = Math.Round(item.GetStatAbsolute(statType));
                var multiplier = item.GetStatMultiplier(statType);
                var color = Stat.GetStatColor(statType);

                if (absolute != 0)
                    info.AddText($"{absolute.Format()} {Stat.GetStatName(statType)}", color);
                if (multiplier != 0)
                    info.AddText($"{(multiplier * 100d).Format()}% {Stat.GetStatName(statType)}", color);
            }
        }
    }
}
