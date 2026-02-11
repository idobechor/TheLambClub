using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    public class WinningPopup(Player[] players, Dictionary<Player, HandRank> ranks, int numUpWinners) : WinningPopupModel(players, ranks, numUpWinners)
    {
        public override string[] PlayersNames
        {
            get
            {
                return [..Players.Select(player =>
                 {
                     string WinnerText = string.Empty;
                     if(Ranks==null)
                         WinnerText= Models.Strings.IntoruceTheWinner+player.Name;
                     else
                     {
                         if (player!=null)
                             if (Array.IndexOf(Players, player) <=numUpWinners)
                                WinnerText= Models.Strings.IntoruceTheWinner+player.Name+" "+Ranks[player].ToString();
                             else
                                WinnerText= (Array.IndexOf(Players, player)+1)+player.Name+" "+Ranks[player].ToString();
                     }
                     return WinnerText;
                 })];
            }
        }
    }
}
