using System.Collections.Generic;
using System.Threading.Tasks;
using TheLambClub.Models;

namespace TheLambClub.Services
{
    public interface IPokerSuggestionService
    {
        Task<PokerSuggestionResult> GetSuggestionAsync(
            FBCard playerCard1,
            FBCard playerCard2,
            List<FBCard> boardCards);
    }
}
