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


            string userPrompt = $@"You are a professional Poker Strategy Engine. 
        Analyze the hole cards and board to output technical metrics based on GTO principles.
        Data:
        - Hand: {playerHand}
        - Board: {boardText}
        - Stage: {stage}
        Technical Rules & Logic Refinements:
        1. PLAYING THE BOARD: If the board shows a strong hand (Trips, Straight, Full House), everyone shares it. Your 'Current Strength' must reflect your RELATIVE advantage. Having a Full House because the board has one does NOT make your strength 10/10 unless your hole cards improve it.
        2. KICKER AWARENESS: In paired or trips boards, your 'Current Strength' depends heavily on your Kicker. A low kicker in a shared-strength scenario should result in a low 'Current Strength'.
        3. ACTION LOGIC: 
           - 'Check' or 'Fold' = 0% Bet.
           - 'Call' = 0% Bet.
           - Only 'Raise' if you have a range advantage or a strong nut-draw.
        4. BET SIZING: 
           - Standard: 10-40% of the pot. 
           - 100% = All-in. Use >40% only for 'The Nuts' or polarized bluffs.
        5. BOARD TEXTURE & DANGER: 
           - Danger is HIGH (8-10) if the board is 'wet' (3+ cards of same suit/sequence) or if your hand is easily beaten by a single higher card.
        6. STAGE SENSITIVITY: Omit 'Current Strength' if Stage is 'Pre-flop'.
        Output Format:
        Action: [Raise, Call, Check, Fold]
        Current Strength: [1-10] (Omit if Pre-flop)
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
