using System.Collections.Generic;
using System.Threading.Tasks;
using TheLambClub.Models;

namespace TheLambClub.Services
{
    /// <summary>
    /// Defines the contract for an AI-driven poker suggestion service.
    /// Provides methods to analyze the current game state and return strategic recommendations.
    /// </summary>
    public interface IPokerSuggestionService
    {
        #region public methods

        /// <summary>
        /// Asynchronously retrieves a strategic poker suggestion based on player cards and the board state.
        /// </summary>
        /// <param name="playerCard1">The first card held by the player.</param>
        /// <param name="playerCard2">The second card held by the player.</param>
        /// <param name="boardCards">A list containing the cards currently on the board.</param>
        /// <returns>A <see cref="PokerSuggestionResult"/> containing the AI recommendation or error details.</returns>
        Task<PokerSuggestionResult> GetSuggestionAsync(
            FBCard playerCard1,
            FBCard playerCard2,
            List<FBCard> boardCards);

        #endregion
    }
}
