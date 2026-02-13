using System.Windows.Input;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    public class WinningPopupVM
    {
        private readonly WinningPopup winningPopup;
        public event Action? RequestClose;
        public string[] PlayersNames => winningPopup.PlayersNames;
        public  ICommand ClosePopupCommand { get; }

        private  void ClosePopup(object obj)
        {
            RequestClose?.Invoke();
        }
        public WinningPopupVM(Player[] players,Dictionary<Player,HandRank>ranks,int numUpWinners)
        {
            winningPopup = new WinningPopup(players,ranks, numUpWinners);
            ClosePopupCommand = new Command(ClosePopup);
        }
        public WinningPopupVM()
        {
        }
    }
}
