using System.Windows.Input;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    public class LostGamePopupVM
    {
        private readonly LostGamePopupML lostGamePopupML;// = new();
        public string ResultMessage => lostGamePopupML.LosingGameResult;
        public ICommand MoveToHome { get; }
        public LostGamePopupVM(string winningText)
        {
            lostGamePopupML = new LostGamePopupML(winningText);
            MoveToHome = new Command(MoveToHomeFunction);
        }
        private void MoveToHomeFunction(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PopAsync();
            });
        }
    }
}
