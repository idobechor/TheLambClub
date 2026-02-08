using System.Collections.Generic;
using System.Threading.Tasks;
using TheLambClub.Models;

namespace TheLambClub.Services
{
    /// <summary>
    /// Suggests a poker action (raise, stay, fold) using AI based on player hand and board.
    /// </summary>
    public interface IPokerSuggestionService
    {
        /// <summary>
        /// Gets a suggestion for the current situation. Reads API key from SecureStorage.
        /// </summary>
        /// <param name="playerCard1">Player's first hole card.</param>
        /// <param name="playerCard2">Player's second hole card.</param>
        /// <param name="boardCards">Community board cards (only non-null entries are used).</param>
        /// <returns>Result with Suggestion ("raise", "stay", "fold") or failure.</returns>
        Task<PokerSuggestionResult> GetSuggestionAsync(
            FBCard playerCard1,
            FBCard playerCard2,
            List<FBCard> boardCards);
    }
}
