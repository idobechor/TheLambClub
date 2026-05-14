namespace TheLambClub.Services
{
    /// <summary>
    /// Represents the outcome of an AI-driven poker strategy request.
    /// Encapsulates the processed suggestion and the raw data received from the service.
    /// </summary>
    public class PokerSuggestionResult
    {
        #region properties

        /// <summary>
        /// Gets or sets the clean, human-readable strategic suggestion (e.g., "Raise 3x").
        /// </summary>
        public string? Suggestion { get; set; }

        /// <summary>
        /// Gets or sets the raw, unparsed response received from the AI provider.
        /// Useful for debugging or logging purposes.
        /// </summary>
        public string? RawResponse { get; set; }

        /// <summary>
        /// Gets a value indicating whether a valid suggestion was successfully retrieved.
        /// </summary>
        public bool Success => !string.IsNullOrEmpty(Suggestion);

        #endregion

        #region public methods

        /// <summary>
        /// Creates a failed result object, optionally containing the raw response 
        /// that led to the failure.
        /// </summary>
        /// <param name="rawResponse">The error details or raw output from the failed request.</param>
        /// <returns>A new PokerSuggestionResult instance indicating failure.</returns>
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