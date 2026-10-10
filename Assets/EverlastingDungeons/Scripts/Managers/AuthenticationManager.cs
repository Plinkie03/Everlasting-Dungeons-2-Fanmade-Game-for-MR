using Assets.EverlastingDungeons.Scripts.Entity.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.EverlastingDungeons.Scripts.Managers
{
    public class AuthenticationManager : BaseManager<AuthenticationManager>
    {
        public PlayerData data;

        public override void Init()
        {
        }

        public bool Authenticate(string username, string password)
        {
            var data = DatabaseManager.Instance.GetPlayerData(username) ?? new PlayerData(username, password);
            data.ForceDefaultData();
            if (data.password != password) return false;
            this.data = data;
            return true;
        }
    }
}
