using Plugin.CloudFirestore.Attributes;
using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    /// <summary>
    /// Represents an abstract base model for a player in the game, containing identity, 
    /// financial status, and card-related information.
    /// </summary>
    /// <param name="name">The display name of the player.</param>
    /// <param name="id">The unique identifier for the player.</param>
    public abstract class PlayerModel(string name, string id)
    {
        #region properties
        /// <summary>
        /// Gets or sets a value indicating whether the player has folded their hand in the current round.
        /// </summary>
        public bool IsFolded { get; set; } = false;
        /// <summary>
        /// Gets or sets the first private card assigned to the player.
        /// </summary>
        public FBCard? FBCard1 { get; set; }
        /// <summary>
        /// Gets or sets the second private card assigned to the player.
        /// </summary>
        public FBCard? FBCard2 { get; set; }
        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        public string Name { get; set; } = name;
        /// <summary>
        /// Gets or sets the unique ID of the player.
        /// </summary>
        public string Id { get; set; } = id;
        /// <summary>
        /// Gets or sets the amount of money the player has currently contributed to the pot in the active round.
        /// </summary>
        public double CurrentBet { get; set; }
        /// <summary>
        /// Gets or sets the total amount of money the player currently possesses.
        /// </summary>
        public double CurrentMoney { get; set; } = 10000;
        /// <summary>
        /// Gets or sets a value indicating whether the player has committed all their remaining money to the pot.
        /// </summary>
        public bool IsAllIn { get; set; } = false;
        /// <summary>
        /// Defines the possible ranking levels of a poker hand from lowest to highest.
        /// </summary>
        public enum LevelsOfHands
        {
            HighCard,
            Pair,
            TwoPair,
            ThreeOfAKind,
            Straight,
            Flush,
            FullHouse,
            FourOfAKind,
            StraightFlush,
            RoyalFlush
        }
        #endregion
        #region public methods
        /// <summary>
        /// Evaluates and returns the best possible hand rank for the player using their cards and the community board.
        /// </summary>
        /// <param name="boardCards">The array of community cards currently on the table.</param>
        /// <returns>The highest <see cref="HandRank"/> the player can achieve.</returns>
        public abstract HandRank EvaluateBestHand(FBCard[] boardCards);
        #endregion
    }
}