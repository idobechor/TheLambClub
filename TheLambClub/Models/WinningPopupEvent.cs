using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public class WinningPopupEvent(Player[] playersArray, Dictionary<Player, HandRank> ranks, int numberOfWinners) : EventArgs
    {
        public Dictionary<Player, HandRank> ranks = ranks;
        public Player[] playersArray = playersArray;
        public int numberOfWinners = numberOfWinners;
    }
}
