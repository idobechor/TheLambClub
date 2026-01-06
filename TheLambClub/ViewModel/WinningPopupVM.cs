using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;

using TheLambClub.ModelsLogic;
using TheLambClub.Models;

namespace TheLambClub.ViewModel
{
    public class WinningPopupVM
    {
        private Player[] players{get;set;}
        private Dictionary<Player, HandRank> ranks { get;set;}
        public string[] PlayersNames
        {
            get {
                return [..players.Select(player =>
                 {
                     string WinnerText = string.Empty;
                     if(ranks==null)
                         WinnerText= Models.Strings.IntoruceTheWinner+player.Name;
                     else
                     {
                         if (player!=null)
                             if (Array.IndexOf(players, player) == 0)
                                WinnerText= Models.Strings.IntoruceTheWinner+player.Name+" "+ranks[player].ToString();
                             else
                                WinnerText= (Array.IndexOf(players, player)+1)+player.Name+" "+ranks[player].ToString();
                     } 
                     return WinnerText;
                 })];
            }
        }
          
        public WinningPopupVM(Player[] players,Dictionary<Player,HandRank>ranks)
        {
            this.players = players;
            this.ranks = ranks;
        }
    }
}
