using OpenAI.Chat;
using TheLambClub.Models;

namespace TheLambClub.Services
{
    public class PokerSuggestionService(string apiKey) : IPokerSuggestionService
    {
        #region fields
        private readonly string _apiKey = apiKey;

        #endregion
        #region public methods       
        public async Task<PokerSuggestionResult> GetSuggestionAsync(
       FBCard playerCard1,
       FBCard playerCard2,
       List<FBCard> boardCards)
        {
            PokerSuggestionResult finalResult;

            if (string.IsNullOrWhiteSpace(Keys.OpenAIApiKey))
                finalResult = PokerSuggestionResult.Failed(Keys.ApiKeyDosentSet);
            else
            {
                try
                {
                    string playerHand = CardFormattingHelper.FormatPlayerHand(playerCard1, playerCard2);
                    int boardCount = 0;

                    if (boardCards != null)
                        foreach (FBCard c in boardCards)
                            if (c != null && c.Value > 0) boardCount++;
                    string boardText = CardFormattingHelper.FormatBoard(boardCards!);
                    string stage = CardFormattingHelper.GetStageLabel(boardCount);
                    string userPrompt = $"""
                {Strings.userPromptExp}
                Context Data:
                - Hand: {playerHand}
                - Board: {boardText}
                - Stage: {stage}
                
                {Strings.Rulls}
                """;
                    ChatClient client = new(Keys.DefaultModel, Keys.OpenAIApiKey);
                    ChatCompletion completion = await client.CompleteChatAsync(userPrompt).ConfigureAwait(false);
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
                    finalResult = PokerSuggestionResult.Failed(ex.Message);
                }
            }
            return finalResult;
        }

        #endregion
    }
}
