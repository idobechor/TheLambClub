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

            if (card == null || card.Value < 1 || card.Value >= valueNames.Length)
                return string.Empty;

            string valueName = valueNames[card.Value];
            string shapeName = card.Shape.ToString();

            return $"{valueName}{Strings.OfJoiner}{shapeName}{Strings.PluralS}";
        }

        public static string FormatPlayerHand(TheLambClub.Models.FBCard card1, TheLambClub.Models.FBCard card2)
        {
            string c1 = FormatCard(card1);
            string c2 = FormatCard(card2);

            if (string.IsNullOrEmpty(c1) && string.IsNullOrEmpty(c2))
                return $"{Strings.PlayerHandPrefix}{Strings.None}";

            if (string.IsNullOrEmpty(c1))
                return $"{Strings.PlayerHandPrefix}{c2}";

            if (string.IsNullOrEmpty(c2))
                return $"{Strings.PlayerHandPrefix}{c1}";

            return $"{Strings.PlayerHandPrefix}{c1}, {c2}";
        }

        public static string FormatBoard(List<FBCard> boardCards)
        {
            if (boardCards == null)
                return $"{Strings.BoardPrefix}{Strings.None}";

            List<string> parts = [.. boardCards
                .Where(c => c != null && c.Value > 0)
                .Select(FormatCard)
                .Where(s => !string.IsNullOrEmpty(s))];

            if (parts.Count == 0)
                return $"{Strings.BoardPrefix}{Strings.None}";

            return Strings.BoardPrefix + string.Join(", ", parts);
        }

        public static string GetStageLabel(int boardCardCount)
        {
            return boardCardCount switch
            {
                0 => Strings.StagePreFlop,
                3 => Strings.StageFlop,
                4 => Strings.StageTurn,
                5 => Strings.StageRiver,
                _ => Strings.StageUnknown
            };
        }

        #endregion
    }
}
