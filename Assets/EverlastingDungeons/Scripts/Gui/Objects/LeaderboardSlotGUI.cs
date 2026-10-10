using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Gui.Objects
{
    public class LeaderboardSlotGUI : MonoBehaviour
    {
        public TextMeshProUGUI position, level, username;

        public void SetData(PlayerData player, int index, Func<PlayerData, double> fn)
        {
            level.text = fn(player).Format();
            position.text = (index + 1d).Format();
            username.text = player.username;
        }
    }
}
