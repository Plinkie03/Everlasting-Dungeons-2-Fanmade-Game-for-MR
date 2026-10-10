using Assets.EverlastingDungeons.Scripts.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.EverlastingDungeons.Scripts.Entity.Player
{
    public class PlayerModdedStats : EntityModdedStats
    {
        public PlayerBehavior player;
        public override EntityBehavior Entity => player;
    }
}
