using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    /// <summary>
    /// Represents a player in the game, handling personal data and hand evaluation logic.
    /// </summary>
    public class Player : PlayerModel
    {
        #region constructors

        /// <summary>
        /// Initializes a new instance of the Player class with default empty values.
        /// </summary>
        public Player() : base(String.Empty, String.Empty) { }

        /// <summary>
        /// Initializes a new instance of the Player class with a specific name and ID.
        /// </summary>
        /// <param name="playerName">The name of the player.</param>
        /// <param name="id">The unique identifier for the player.</param>
        public Player(string playerName, string id) : base(playerName, id)
        {
        }

        #endregion

        #region public methods

        /// <summary>
        /// Evaluates the strongest poker hand possible using the player's hole cards and the board cards.
        /// </summary>
        /// <param name="boardCards">The cards currently on the community board.</param>
        /// <returns>A HandRank object representing the best hand found.</returns>
        public override HandRank EvaluateBestHand(FBCard[] boardCards)
        {
            HandRank handRank = new HandEvaluator().EvaluateBestHand(FBCard1!, FBCard2!, boardCards);
            return handRank;
        }

        #endregion
    }
}