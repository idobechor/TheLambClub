using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public class WinningPopupEvent:EventArgs
    {
        public Dictionary<Player, HandRank> ranks = [];
        public Player[] playersArray = null!;

        public WinningPopupEvent(Player[] playersArray, Dictionary<Player, HandRank> ranks)
        {
            this.playersArray = playersArray;
            this.ranks = ranks;
        }
    }
}
