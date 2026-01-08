using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    public class WinningPropmptML:WinningPopupModel
    {
        public WinningPropmptML(Player[] players, Dictionary<Player, HandRank> ranks) : base(players, ranks)
        {
        }

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
                             if (Array.IndexOf(Players, player) == 0)
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
