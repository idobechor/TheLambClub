using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public class WinningPopupEvent(Player[] playersArray, Dictionary<Player, HandRank> ranks) : EventArgs
    {
        public Dictionary<Player, HandRank> ranks = ranks;
        public Player[] playersArray = playersArray;
    }
}
