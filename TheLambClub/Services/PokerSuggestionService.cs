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


            string userPrompt = $@"You are a strict Poker Logic Engine acting as an API backend.
Your goal is to output raw data metrics based on GTO principles. 
DO NOT provide explanations, conversational text, or markdown formatting.

Context Data:
- Hand: {playerHand}
- Board: {boardText}
- Stage: {stage}

Logic Rules:
1. CURRENT STRENGTH (1-10):
   - IF PRE-FLOP: Evaluate hole cards based on raw potential. High scores (8-10) for High Pairs (AA-JJ), AK/AQ. Mid scores (5-7) for Suited Connectors, Ace-X, or Low-Mid Pairs. High cards and same-suit/sequential cards increase this score.
   - IF POST-FLOP: Evaluate RELATIVE advantage. If the board pairs or connects, do not overrate strength unless you have the best kickers or full house.
2. ACTION MAPPING: 'Check'/'Call'/'Fold' = 0% Bet. Only 'Raise' if you have a range advantage or the nuts.
3. BET SIZING: Standard: 10-40% pot. Use >40% only for 'The Nuts' or polarized bluffs. 100% = All-in.
4. DANGER: High (8-10) if the board is 'wet' (3+ of same suit/sequence) or hand is easily beaten by a single higher card.

*** CRITICAL OUTPUT INSTRUCTIONS ***
- You must reply ONLY with the format below.
- NO N/A values. Use 1-10 for Strength in all stages.
- Do not add any text before or after the metrics.

Expected Output Format:
Action: [Raise, Call, Check, Fold]
Current Strength: [1-10]
Danger Level: [1-10]
Recommended Bet: [0-100]%";
            try
            {
                var client = new ChatClient(DefaultModel, _apiKey);
                ChatCompletion completion = await client.CompleteChatAsync(userPrompt).ConfigureAwait(false);
                string raw = completion.Content?.Count > 0 ? completion.Content[0].Text : null!;
                string suggestion = raw;
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
