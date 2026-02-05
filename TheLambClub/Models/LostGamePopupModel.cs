namespace TheLambClub.Models
{
    public class LostGamePopupModel
    {
        public string LosingGameResult { get; set; } = string.Empty;
        public LostGamePopupModel(string losingGameResult)
        {
            LosingGameResult = losingGameResult;
        }
    }
}
