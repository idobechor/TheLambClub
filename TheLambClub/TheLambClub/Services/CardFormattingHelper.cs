using System.Collections.Generic;
using System.Linq;
using TheLambClub.Models;

namespace TheLambClub.Services
{
    public static class CardFormattingHelper
    {
        #region public methods
        public static string FormatCard(TheLambClub.Models.FBCard card)
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

        public static string FormatPlayerHand(TheLambClub.Models.FBCard card1, TheLambClub.Models.FBCard card2)
        {
            List<string> validCards = new[] { FormatCard(card1), FormatCard(card2) }
                .Where(c => !string.IsNullOrEmpty(c))
                .ToList();

            string result = Strings.PlayerHandPrefix +
                (validCards.Count == 0 ? Strings.None : string.Join(", ", validCards));

            return result;
        }

        public static string FormatBoard(List<FBCard> boardCards)
        {
            List<string> parts = boardCards != null
                ? boardCards.Where(c => c != null && c.Value > 0)
                            .Select(FormatCard)
                            .Where(s => !string.IsNullOrEmpty(s))
                            .ToList()
                : new List<string>();

            string result = Strings.BoardPrefix +
                (parts.Count > 0 ? string.Join(", ", parts) : Strings.None);

            return result;
        }

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
