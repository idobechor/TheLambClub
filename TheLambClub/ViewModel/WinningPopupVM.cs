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
        private readonly WinningPropmptML winningPropmptML;
        public string[] PlayersNames => winningPropmptML.PlayersNames;

        public WinningPopupVM(Player[] players,Dictionary<Player,HandRank>ranks)
        {
            winningPropmptML = new WinningPropmptML(players,ranks);
        }
    }
}
