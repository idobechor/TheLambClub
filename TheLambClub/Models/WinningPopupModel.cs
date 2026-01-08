using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public abstract class WinningPopupModel
    {
        protected Player[] Players { get; set; }
        protected Dictionary<Player, HandRank> Ranks { get; set; }
        public abstract string[] PlayersNames { get; }


        public WinningPopupModel(Player[] players, Dictionary<Player, HandRank> ranks)
        {
            this.Players = players;
            this.Ranks = ranks;
        }
    }
}
