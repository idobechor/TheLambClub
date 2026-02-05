using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    public class LostGamePopupVM
    {
        private readonly LostGamePopupML lostGamePopupML;// = new();
        public string ResultMessage => lostGamePopupML.LosingGameResult;
        public LostGamePopupVM(string winningText)
        {
            lostGamePopupML = new LostGamePopupML(winningText);
        }
    }
}
