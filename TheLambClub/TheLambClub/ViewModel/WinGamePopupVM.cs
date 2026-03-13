using CommunityToolkit.Maui.Core;
using System.Windows.Input;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    public class WinGamePopupVM
    {
        #region fields

        private readonly WinGamePopupModel? WinGamePopup;

        #endregion

        #region commands

        public ICommand? MoveToHome { get; }

        #endregion

        #region properties

        public string ResultMessage => WinGamePopup!.WinningGameResult;

        #endregion

        #region constructors

        public WinGamePopupVM(string winningText)
        {
            WinGamePopup = new WinGamePopupModel(winningText);
            MoveToHome = new Command(MoveToHomeFunction);
        }
        public WinGamePopupVM()
        {
        }

        #endregion

        #region private methods

        private void MoveToHomeFunction(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PopAsync();
            });
        }

        #endregion
    }
}
