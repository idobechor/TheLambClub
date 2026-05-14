using TheLambClub.ModelsLogic;
using static TheLambClub.Models.PlayerModel;

namespace TheLambClub.Models
{
    /// <summary>
    /// Represents an abstract base model for ranking poker hands, providing the necessary 
    /// properties and methods to compare different hand strengths.
    /// </summary>
    public abstract class HandRankModel
    {
        #region properties

        /// <summary>
        /// Gets or sets the category of the poker hand (e.g., Flush, Straight, Pair).
        /// </summary>
        public LevelsOfHands? HandType { get; set; }

        /// <summary>
        /// Gets or sets the primary numerical value used for comparison (e.g., the value of a Pair).
        /// </summary>
        public int? PrimaryValue { get; set; }

        /// <summary>
        /// Gets or sets the secondary numerical value used to break ties (e.g., the second pair in a Two-Pair hand).
        /// </summary>
        public int? SecondaryValue { get; set; }

        /// <summary>
        /// Gets or sets the array of kicker cards used to determine the winner when hand types and values are equal.
        /// </summary>
        public int[]? Kickers { get; set; }

        /// <summary>
        /// Gets or sets the specific cards that constitute the ranked hand.
        /// </summary>
        public FBCard[]? HandCards { get; set; }

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HandRankModel"/> class.
        /// </summary>
        public HandRankModel()
        {
        }

        #endregion

        #region public methods

        /// <summary>
        /// Compares the current hand rank with another hand rank.
        /// </summary>
        /// <param name="other">The other <see cref="HandRank"/> to compare against.</param>
        /// <returns>An integer indicating whether this hand is better (1), worse (-1), or equal (0).</returns>
        public abstract int Compare(HandRank other);

        /// <summary>
        /// Determines if the current hand rank is stronger than the specified hand rank.
        /// </summary>
        /// <param name="other">The other <see cref="HandRank"/> to evaluate.</param>
        /// <returns>True if this hand is better; otherwise, false.</returns>
        public abstract bool IsBetter(HandRank other);

        #endregion
    }
}