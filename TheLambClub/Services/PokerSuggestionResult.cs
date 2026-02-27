namespace TheLambClub.Services
{
    public class PokerSuggestionResult
    {
        public string? Suggestion { get; set; }
        public string? RawResponse { get; set; }
        public bool Success => !string.IsNullOrEmpty(Suggestion);

        public static PokerSuggestionResult Failed(string rawResponse = null!)
        {
            PokerSuggestionResult res = new()
            {
                RawResponse = rawResponse
            };
            return res;
        }
    }
}
