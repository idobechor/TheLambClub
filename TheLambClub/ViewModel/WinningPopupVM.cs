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
        private readonly WinningPopup winningPopup;
        public string[] PlayersNames => winningPopup.PlayersNames;

        public WinningPopupVM(Player[] players,Dictionary<Player,HandRank>ranks)
        {
            winningPopup = new WinningPopup(players,ranks);
        }
    }
}
