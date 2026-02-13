using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    public class WinningPopupVM
    {
        private readonly WinningPopup winningPopup;
        public string[] PlayersNames => winningPopup.PlayersNames;
        public WinningPopupVM(Player[] players,Dictionary<Player,HandRank>ranks,int numUpWinners)
        {
            winningPopup = new WinningPopup(players,ranks, numUpWinners);
        }
        public WinningPopupVM()
        {
        }
    }
}
