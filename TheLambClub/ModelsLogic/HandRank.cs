using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    /// <summary>
    /// Represents the rank of a poker hand and provides logic for comparing 
    /// its strength against other hands.
    /// </summary>
    public class HandRank : HandRankModel
    {
        #region public methods

        /// <summary>
        /// Compares the current hand rank with another to determine which is stronger.
        /// Follows standard poker hierarchy: Hand Type -> Primary Value -> Secondary Value -> Kickers.
        /// </summary>
        /// <param name="other">The other hand rank to compare against.</param>
        /// <returns>
        /// A positive integer if this hand is better, 
        /// a negative integer if the other hand is better, 
        /// or 0 if they are exactly equal (a tie).
        /// </returns>
        public override int Compare(HandRank other)
        {
            int res = 0;
            if (other != null)
            {
                int myType = (int)HandType!;
                int otherType = (int)other.HandType!;

                // 1. Compare the hand category (e.g., Flush vs Straight)
                if (myType != otherType)
                    res = myType - otherType;
                // 2. Compare the primary value (e.g., Pair of Aces vs Pair of Kings)
                else if (PrimaryValue != other.PrimaryValue)
                    res = (int)(PrimaryValue - other.PrimaryValue)!;
                // 3. Compare the secondary value (relevant for Full House or Two Pair)
                else if (SecondaryValue != other.SecondaryValue)
                    res = (int)(SecondaryValue - other.SecondaryValue)!;
                // 4. Compare kickers (tie-breakers) in descending order
                else if (Kickers != null && other.Kickers != null)
                {
                    int minLength = Math.Min(Kickers.Length, other.Kickers.Length);
                    for (int i = 0; i < minLength; i++)
                        if (Kickers[i] != other.Kickers[i])
                        {
                            res = Kickers[i] - other.Kickers[i];
                            break; // Stop at the first different kicker
                        }
                }
            }
            return res;
        }

        /// <summary>
        /// Determines if the current hand is stronger than the specified hand.
        /// </summary>
        /// <param name="other">The hand rank to compare.</param>
        /// <returns>True if this hand is stronger; otherwise, false.</returns>
        public override bool IsBetter(HandRank other)
        {
            return Compare(other) > 0;
        }

        /// <summary>
        /// Returns a string representation of the hand rank for display purposes.
        /// </summary>
        /// <returns>A string indicating the hand type.</returns>
        public override string ToString()
        {
            return Strings.HisRankIs + HandType.ToString();
        }

        #endregion

    }
}