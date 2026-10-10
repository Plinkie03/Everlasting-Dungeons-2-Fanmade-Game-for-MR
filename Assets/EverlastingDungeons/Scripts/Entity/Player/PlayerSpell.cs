using Assets.EverlastingDungeons.Scripts.Managers;
using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.EverlastingDungeons.Scripts.Entity.Player
{
    public class PlayerSpell
    {
        public string spellUUID;
        public double level;
        public int index = -1;

        [JsonIgnore]
        public SpellSO Spell => ResourceManager.Instance.spells.Get(spellUUID);
        
        public PlayerSpell(string spellUUID, long level)
        {
            this.spellUUID = spellUUID;
            this.level = level;
        }

        public void Execute() => Spell.Execute(PrefabManager.Instance.activePlayer);
    }
}
