using CommunityToolkit.Maui.Core;
using System.Windows.Input;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    public class WinGamePopupVM
    {
        public event Action? RequestClose;
        private readonly WinGamePopupML WinGamePopup;// = new();
        public string ResultMessage=> WinGamePopup.WinningGameResult;
        public ICommand MoveToHome { get; }
        public WinGamePopupVM(string winningText)
        {
            WinGamePopup = new WinGamePopupML(winningText);
            MoveToHome = new Command(MoveToHomeFunction);
        }

        private void MoveToHomeFunction(object obj)
        {
            RequestClose?.Invoke();
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PopAsync();
            });
         
        }
    }
}
