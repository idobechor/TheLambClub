using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheLambClub.Models;

namespace TheLambClub.Services
{
    /// <summary>
    /// Formats poker cards as human-readable text for prompts (e.g. "Ace of Spades", "10 of Hearts").
    /// </summary>
    public static class CardFormattingHelper
    {
        /// <summary>
        /// Formats a single card for use in a prompt.
        /// </summary>
        public static string FormatCard(TheLambClub.Models.FBCard card)
        {
            string[] ValueNames = { "", "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King" };
            if (card == null || card.Value < 1 || card.Value > TheLambClub.Models.FBCard.CardsInShape)
                return string.Empty;
            string valueName = ValueNames[card.Value];
            string shapeName = card.Shape.ToString();
            return $"{valueName} of {shapeName}s";
        }

        /// <summary>
        /// Formats the player's two hole cards.
        /// </summary>
        public static string FormatPlayerHand(TheLambClub.Models.FBCard card1, TheLambClub.Models.FBCard card2)
        {
            string c1 = FormatCard(card1);
            string c2 = FormatCard(card2);
            if (string.IsNullOrEmpty(c1) && string.IsNullOrEmpty(c2))
                return "Player hand: (none)";
            if (string.IsNullOrEmpty(c1))
                return $"Player hand: {c2}";
            if (string.IsNullOrEmpty(c2))
                return $"Player hand: {c1}";
            return $"Player hand: {c1}, {c2}";
        }

        /// <summary>
        /// Formats the board cards (only non-null entries). Returns e.g. "Board: Ace of Spades, King of Hearts" or "Board: (none)".
        /// </summary>
        public static string FormatBoard(List<FBCard> boardCards)
        {
            if (boardCards == null)
                return "Board: (none)";
            var parts = boardCards.Where(c => c != null && c.Value > 0).Select(FormatCard).Where(s => s.Length > 0).ToList();
            if (parts.Count == 0)
                return "Board: (none)";
            return "Board: " + string.Join(", ", parts);
        }

        /// <summary>
        /// Returns a short stage label based on number of board cards: Pre-flop, Flop, Turn, River.
        /// </summary>
        public static string GetStageLabel(int boardCardCount)
        {
            switch (boardCardCount)
            {
                case 0: return "Pre-flop";
                case 3: return "Flop";
                case 4: return "Turn";
                case 5: return "River";
                default: return "Unknown";
            }
        }
    }
}
