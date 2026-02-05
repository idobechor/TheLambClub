using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    public class WinGamePopupVM
    {
        private readonly WinGamePopupML WinGamePopup;// = new();
        public string ResultMessage=> WinGamePopup.WinningGameResult;
        public WinGamePopupVM(string winningText)
        {
            WinGamePopup = new WinGamePopupML(winningText);          
        }
    }
}
