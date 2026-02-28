namespace TheLambClub.Services
{
    public class PokerSuggestionResult
    {
        #region properties

        public string? Suggestion { get; set; }
        public string? RawResponse { get; set; }
        public bool Success => !string.IsNullOrEmpty(Suggestion);

        #endregion

        #region public methods

        public static PokerSuggestionResult Failed(string rawResponse = null!)
        {
            PokerSuggestionResult res = new()
            {
                RawResponse = rawResponse
            };
            return res;
        }

        #endregion
    }
}
