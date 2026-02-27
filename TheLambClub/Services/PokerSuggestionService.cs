using OpenAI.Chat;
using TheLambClub.Models;

namespace TheLambClub.Services
{
    public class PokerSuggestionService : IPokerSuggestionService
    {
        public async Task<PokerSuggestionResult> GetSuggestionAsync(
            FBCard playerCard1,
            FBCard playerCard2,
            List<FBCard> boardCards)
        {
            if (string.IsNullOrWhiteSpace(Keys.OpenAIApiKey))
                return PokerSuggestionResult.Failed(Keys.ApiKeyDosentSet);
            string playerHand = CardFormattingHelper.FormatPlayerHand(playerCard1, playerCard2);
            int boardCount = 0;
            if (boardCards != null)
            {
                foreach (FBCard c in boardCards)
                    if (c != null && c.Value > 0) boardCount++;
            }
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
            try
            {
                ChatClient client = new(Keys.DefaultModel, Keys.OpenAIApiKey);
                ChatCompletion completion = await client.CompleteChatAsync(userPrompt).ConfigureAwait(false);
                string raw = completion.Content?.Count > 0 ? completion.Content[0].Text : null!;
                string suggestion = raw;
                return new PokerSuggestionResult
                {
                    Suggestion = suggestion ?? Strings.StayBtnTxt,
                    RawResponse = raw
                };
            }
            catch (Exception ex)
            {
                return PokerSuggestionResult.Failed(ex.Message);
            }
        }

    }
}
