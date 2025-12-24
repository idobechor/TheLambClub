using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLambClub.ModelsLogic;

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
                     if (player==null)
                         return string.Empty;
                     if(Array.IndexOf(players, player)==0)
                          return "The Winner is:"+player.Name+" "+ranks[player].ToString();
                     return (Array.IndexOf(players, player)+1)+player.Name+" "+ranks[player].ToString();
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
