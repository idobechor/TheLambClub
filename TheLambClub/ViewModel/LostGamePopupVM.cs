using System.Windows.Input;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    public class LostGamePopupVM
    {
        #region fields

        private readonly LostGamePopupML lostGamePopupML;

        #endregion

        #region commands

        public ICommand MoveToHome { get; }

        #endregion

        #region properties

        public string ResultMessage => lostGamePopupML.LosingGameResult;

        #endregion

        #region constructors

        public LostGamePopupVM(string winningText)
        {
            lostGamePopupML = new LostGamePopupML(winningText);
            MoveToHome = new Command(MoveToHomeFunction);
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
