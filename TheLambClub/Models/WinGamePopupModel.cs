namespace TheLambClub.Models
{
    public class WinGamePopupModel
    {
        #region properties

        public string WinningGameResult { get; set; } = string.Empty;

        #endregion

        #region constructors

        public WinGamePopupModel(string winningGameResult)
        {
            WinningGameResult = winningGameResult;
        }

        #endregion
    }
}
