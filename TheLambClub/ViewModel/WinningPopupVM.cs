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
        public bool IsPopupOpen=false;

        public WinningPopupVM(Player[] players,Dictionary<Player,HandRank>ranks)
        {
            IsPopupOpen=true;
            winningPopup = new WinningPopup(players,ranks);
        }
        public WinningPopupVM()
        {
        }

        public void Close()
        {
          IsPopupOpen = true;
        }
    }
}
