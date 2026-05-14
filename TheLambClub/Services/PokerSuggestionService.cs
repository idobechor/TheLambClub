using OpenAI.Chat;
using TheLambClub.Models;

namespace TheLambClub.Services
{
    /// <summary>
    /// Implements the poker suggestion service using OpenAI's API to provide 
    /// strategic advice based on real-time game data.
    /// </summary>
    public class PokerSuggestionService(string apiKey) : IPokerSuggestionService
    {
        #region fields

        private readonly string _apiKey = apiKey;

        #endregion

        #region public methods        

        /// <summary>
        /// Asynchronously fetches a strategic suggestion from OpenAI by formatting the 
        /// current hand, board, and game stage into a detailed prompt.
        /// </summary>
        /// <param name="playerCard1">The first card in the player's hand.</param>
        /// <param name="playerCard2">The second card in the player's hand.</param>
        /// <param name="boardCards">The list of community cards on the board.</param>
        /// <returns>A result object containing the AI's suggestion or failure details.</returns>
        public async Task<PokerSuggestionResult> GetSuggestionAsync(
        FBCard playerCard1,
        FBCard playerCard2,
        List<FBCard> boardCards)
        {
            PokerSuggestionResult finalResult;

            // Check if the API key is configured before making the request
            if (string.IsNullOrWhiteSpace(Keys.OpenAIApiKey))
                finalResult = PokerSuggestionResult.Failed(Keys.ApiKeyDosentSet);
            else
            {
                try
                {
                    // Format the raw card data into human-readable strings for the AI prompt
                    string playerHand = CardFormattingHelper.FormatPlayerHand(playerCard1, playerCard2);
                    int boardCount = 0;

                    if (boardCards != null)
                        foreach (FBCard c in boardCards)
                            if (c != null && c.Value > 0) boardCount++;

                    string boardText = CardFormattingHelper.FormatBoard(boardCards!);
                    string stage = CardFormattingHelper.GetStageLabel(boardCount);

                    // Construct the full prompt using predefined strings and game context
                    string userPrompt = $"""
                {Strings.userPromptExp}
                Context Data:
                - Hand: {playerHand}
                - Board: {boardText}
                - Stage: {stage}
                
                {Strings.Rulls}
                """;

                    // Initialize the OpenAI client and request a completion
                    ChatClient client = new(Keys.DefaultModel, Keys.OpenAIApiKey);
                    ChatCompletion completion = await client.CompleteChatAsync(userPrompt).ConfigureAwait(false);

                    // Extract the text content from the completion
                    string raw = (completion.Content != null && completion.Content.Count > 0)
                                 ? completion.Content[0].Text
                                 : null!;

                    finalResult = new PokerSuggestionResult
                    {
                        Suggestion = raw ?? Strings.StayBtnTxt,
                        RawResponse = raw
                    };
                }
                catch (Exception ex)
                {
                    // Catch network or API errors and return a failed result
                    finalResult = PokerSuggestionResult.Failed(ex.Message);
                }
            }
            return finalResult;
        }

        #endregion
    }
}