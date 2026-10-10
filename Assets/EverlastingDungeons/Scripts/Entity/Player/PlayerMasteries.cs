using Assets.EverlastingDungeons.Scripts.Extensions;
using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Entity.Player
{
    public enum MasteryType
    {
        Melee,
        Distance,
        Magic,
        Defense,
        Endurance
    }

    public class PlayerMastery : ILeveable
    {
        public static readonly float LevelMultiplier = 0.1f;
        public MasteryType type;
        public double level = 1;
        public double xp;

        public PlayerMastery(MasteryType type)
        {
            this.type = type;
        }

        public double GetMultiplier() => 1 + (level - 1) * LevelMultiplier;
        public double GetLevel() => level;
        public double GetReqXp() => level * 1;
        public double GetXp() => xp;
        public void SetLevel(double level) => this.level = level;
        public void SetXp(double xp) => this.xp = xp;
        public bool AddXp(double xp)
        {
            var levelUp = Leveable.AddXp(this, xp);
            if (levelUp)
                PrefabManager.Instance.activePlayer.CreateFloatingText($"{type} Up ({level.Format()})", Color.cyan);
            return levelUp;
        }
    }

    public class PlayerMasteries
    {
        public PlayerMastery melee = new(MasteryType.Melee);
        public PlayerMastery distance = new(MasteryType.Distance);
        public PlayerMastery magic = new(MasteryType.Magic);
        public PlayerMastery defense = new(MasteryType.Defense);
        public PlayerMastery endurance = new(MasteryType.Endurance);

        public PlayerMastery GetMastery(MasteryType type) => type switch
        {
            MasteryType.Melee => melee,
            MasteryType.Distance => distance,
            MasteryType.Magic => magic,
            MasteryType.Defense => defense,
            MasteryType.Endurance => endurance,
        };

        public PlayerMastery GetCombatMastery(EntityClassType classType) => classType switch
        {
            EntityClassType.Warrior => melee,
            EntityClassType.Mage => magic,
            EntityClassType.Archer => distance
        };
    }
}
