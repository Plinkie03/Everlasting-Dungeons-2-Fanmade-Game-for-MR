using Assets.EverlastingDungeons.Scripts.Entity.Player;
using Assets.EverlastingDungeons.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Gui.Managers.Selective
{
    public enum LeaderboardType
    {
        Level,
        Melee,
        Distance,
        Magic,
        Endurance,
        Defense,
        HighestFloor,
    }

    public class LeaderboardGUI : BaseSelectiveGUI
    {
        public LeaderboardType type;
        public TextMeshProUGUI text;
        public Transform placeholder, content;

        public void SetType(LeaderboardType type)
        {
            this.type = type;
            Refresh();
        }

        public override void Refresh()
        {
            Clear();

            text.text = $"Leaderboard - Top {type}";

            var fn = GetFunction();
            var field = GetField();

            var players = DatabaseManager.Instance.GetTopPlayers(field);

            for (int i = 0; i < players.Count; i++)
            {
                var player = players[i];
                var slot = Instantiate(PrefabManager.Instance.leaderboardSlot, content);

                slot.SetData(player, i, fn);
            }
        }

        private string GetField() => type switch
        {
            LeaderboardType.Level => nameof(PlayerData.level),
            LeaderboardType.Melee => "masteries.melee.level",
            LeaderboardType.Distance => "masteries.distance.level",
            LeaderboardType.Magic => "masteries.magic.level",
            LeaderboardType.Endurance => "masteries.endurance.level",
            LeaderboardType.Defense => "masteries.defense.level",
            LeaderboardType.HighestFloor => nameof(PlayerData.highestFloor)
        };

        private Func<PlayerData, double> GetFunction() => type switch
        {
            LeaderboardType.Level => x => x.level,
            LeaderboardType.Melee => x => x.masteries.melee.level,
            LeaderboardType.Distance => x => x.masteries.distance.level,
            LeaderboardType.Magic => x => x.masteries.magic.level,
            LeaderboardType.Endurance => x => x.masteries.endurance.level,
            LeaderboardType.Defense => x => x.masteries.defense.level,
            LeaderboardType.HighestFloor => x => x.highestFloor
        };

        private void Clear()
        {
            for (int i = 0;i < content.childCount;i++)
            {
                var child = content.GetChild(i);
                if (child != placeholder)
                    Destroy(child.gameObject);
            }
        }
    }
}
