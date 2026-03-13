namespace TheLambClub.Models
{
    public class LostGamePopupModel
    {
        #region properties

        public string LosingGameResult { get; set; } = string.Empty;

        #endregion

        #region constructors

        public LostGamePopupModel(string losingGameResult)
        {
            LosingGameResult = losingGameResult;
        }

        #endregion
    }
}
