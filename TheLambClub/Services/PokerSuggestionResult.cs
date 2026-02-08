namespace TheLambClub.Services
{
    /// <summary>
    /// Result of an AI poker suggestion (raise, stay, or fold).
    /// </summary>
    public class PokerSuggestionResult
    {
        /// <summary>Normalized suggestion: "raise", "stay", or "fold". Null if no suggestion could be obtained.</summary>
        public string? Suggestion { get; set; }

        /// <summary>Raw model response for debugging or display.</summary>
        public string? RawResponse { get; set; }

        /// <summary>True if the suggestion was obtained successfully.</summary>
        public bool Success => !string.IsNullOrEmpty(Suggestion);

        public static PokerSuggestionResult Failed(string rawResponse = null!)
        {
            PokerSuggestionResult res = new PokerSuggestionResult();
            res.RawResponse = rawResponse;
            return res;
        }
    }
}
