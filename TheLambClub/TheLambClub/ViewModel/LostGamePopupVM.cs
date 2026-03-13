using System.Windows.Input;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    public class LostGamePopupVM
    {
        #region fields

        private readonly LostGamePopupModel lostGamePopupModel;

        #endregion

        #region commands

        public ICommand MoveToHome { get; }

        #endregion

        #region properties

        public string ResultMessage => lostGamePopupModel.LosingGameResult;

        #endregion

        #region constructors

        public LostGamePopupVM(string winningText)
        {
            lostGamePopupModel = new LostGamePopupModel(winningText);
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
