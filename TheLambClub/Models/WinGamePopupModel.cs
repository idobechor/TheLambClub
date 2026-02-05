namespace TheLambClub.Models
{
    public class WinGamePopupModel
    {
        public string WinningGameResult {  get; set; }=string.Empty;
        public WinGamePopupModel(string winningGameResult)
        {
            WinningGameResult = winningGameResult;
        }
    }
}
