using System.Windows.Input;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    public class WinningPopupVM
    {
        #region fields

        private readonly WinningPopup ?winningPopup;

        #endregion

        #region events

        public event Action? RequestClose;

        #endregion

        #region commands

        public ICommand ?ClosePopupCommand { get; }

        #endregion

        #region properties

        public string[] PlayersNames => winningPopup!.PlayersNames;

        #endregion

        #region constructors

        public WinningPopupVM(Player[] players, Dictionary<Player, HandRank> ranks, int numUpWinners)
        {
            winningPopup = new WinningPopup(players, ranks, numUpWinners);
            ClosePopupCommand = new Command(ClosePopup);
        }
        public WinningPopupVM()
        {
        }

        #endregion

        #region private methods

        private void ClosePopup(object obj)
        {
            RequestClose?.Invoke();
        }

        #endregion
    }
}
