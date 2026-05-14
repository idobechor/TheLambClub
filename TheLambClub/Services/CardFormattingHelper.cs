using System.Collections.Generic;
using System.Linq;
using TheLambClub.Models;

namespace TheLambClub.Services
{
    /// <summary>
    /// Provides utility methods for formatting card data and game state information into human-readable strings.
    /// </summary>
    public static class CardFormattingHelper
    {
        #region public methods

        /// <summary>
        /// Converts a single FBCard object into a formatted string (e.g., "Ace of Spades").
        /// </summary>
        /// <param name="card">The card to format.</param>
        /// <returns>A localized string representation of the card, or an empty string if the card is invalid.</returns>
        public static string FormatCard(FBCard card)
        {
            string[] valueNames = [
                string.Empty, Strings.ValAce, Strings.ValTwo, Strings.ValThree, Strings.ValFour,
            Strings.ValFive, Strings.ValSix, Strings.ValSeven, Strings.ValEight,
            Strings.ValNine, Strings.ValTen, Strings.ValJack, Strings.ValQueen, Strings.ValKing
            ];
            string result = (card != null && card.Value >= 1 && card.Value < valueNames.Length)
                ? $"{valueNames[card.Value]}{Strings.OfJoiner}{card.Shape}{Strings.PluralS}"
                : string.Empty;
            return result;
        }

        /// <summary>
        /// Formats the player's two hole cards into a single display string.
        /// </summary>
        /// <param name="card1">The first hole card.</param>
        /// <param name="card2">The second hole card.</param>
        /// <returns>A string representing the player's current hand.</returns>
        public static string FormatPlayerHand(FBCard card1, FBCard card2)
        {
            List<string> validCards = [.. new[] { FormatCard(card1), FormatCard(card2) }.Where(c => !string.IsNullOrEmpty(c))];
            string result = Strings.PlayerHandPrefix +
                (validCards.Count == 0 ? Strings.None : string.Join(", ", validCards));
            return result;
        }

        /// <summary>
        /// Formats the collection of cards currently on the community board into a single string.
        /// </summary>
        /// <param name="boardCards">A list of cards on the board.</param>
        /// <returns>A string representing the board state.</returns>
        public static string FormatBoard(List<FBCard> boardCards)
        {
            List<string> parts = boardCards != null
                ? [.. boardCards.Where(c => c != null && c.Value > 0)
                            .Select(FormatCard)
                            .Where(s => !string.IsNullOrEmpty(s))]
                : [];

            string result = Strings.BoardPrefix +
                (parts.Count > 0 ? string.Join(", ", parts) : Strings.None);

            return result;
        }

        /// <summary>
        /// Determines the current poker stage label based on the number of cards on the board.
        /// </summary>
        /// <param name="boardCardCount">The number of community cards currently dealt.</param>
        /// <returns>A localized label for the game stage (e.g., Flop, Turn, River).</returns>
        public static string GetStageLabel(int boardCardCount)
        {
            string result = boardCardCount switch
            {
                0 => Strings.StagePreFlop,
                3 => Strings.StageFlop,
                4 => Strings.StageTurn,
                5 => Strings.StageRiver,
                _ => Strings.StageUnknown
            };

            return result;
        }

        #endregion
    }
}