using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheLambClub.Models;

namespace TheLambClub.Services
{
    /// <summary>
    /// Uses OpenAI chat completion to suggest raise, stay, or fold based on hand and board.
    /// API key is supplied at construction.
    /// </summary>
    public class PokerSuggestionService : IPokerSuggestionService
    {
        private const string DefaultModel = "gpt-4o-mini";
        private readonly string _apiKey;

        public PokerSuggestionService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<PokerSuggestionResult> GetSuggestionAsync(
            FBCard playerCard1,
            FBCard playerCard2,
            List<FBCard> boardCards)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                return PokerSuggestionResult.Failed("API key not set. Add your OpenAI key in app settings.");

            string playerHand = CardFormattingHelper.FormatPlayerHand(playerCard1, playerCard2);
            int boardCount = 0;
            if (boardCards != null)
            {
                foreach (var c in boardCards)
                    if (c != null && c.Value > 0) boardCount++;
            }
            string boardText = CardFormattingHelper.FormatBoard(boardCards!);
            string stage = CardFormattingHelper.GetStageLabel(boardCount);


        string userPrompt = $@"You are a professional Poker Strategy Engine. Analyze the hole cards and board to output technical metrics.
        Data:
        - Hand: {playerHand}
        - Board: {boardText}
        - Stage: {stage}
        Logic Constraints:
        - Standard bet sizing is typically 10-40%.
        - Only suggest >40% for very strong hands (nuts) or polarized bluffs.
        Output strictly in the following format (no extra text):
        Action: [Choose one: Raise, Call, Check, Fold]
        Pre-flop Strength: [1-10]
        Current Strength: [1-10]
        Danger Level: [1-10]
        Recommended Bet: [0-100]%";

            try
            {
                var client = new ChatClient(DefaultModel, _apiKey);
                ChatCompletion completion = await client.CompleteChatAsync(userPrompt).ConfigureAwait(false);
                string raw = completion.Content?.Count > 0 ? completion.Content[0].Text : null!;
                string suggestion = raw;//NormalizeSuggestion(raw);
                return new PokerSuggestionResult
                {
                    Suggestion = suggestion ?? "stay",
                    RawResponse = raw
                };
            }
            catch (Exception ex)
            {
                return PokerSuggestionResult.Failed(ex.Message);
            }
        }

        //private static string NormalizeSuggestion(string raw)
        //{
        //    if (string.IsNullOrWhiteSpace(raw))
        //        return null!;
        //    string word = raw.Trim().ToLowerInvariant();
        //    // Take first word if model added extra text
        //    int space = word.IndexOf(' ');
        //    if (space > 0)
        //        word = word.Substring(0, space);
        //    if (word == "raise") return "raise";
        //    if (word == "fold") return "fold";
        //    if (word == "stay" || word == "call" || word == "check") return "stay";
        //    return null!;
        //}
    }
}
