using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public abstract class WinningPopupModel
    {
        #region properties

        protected Player[] Players { get; set; }
        protected Dictionary<Player, HandRank> Ranks { get; set; }
        public abstract string[] PlayersNames { get; }

        #endregion

        #region constructors

        public WinningPopupModel(Player[] players, Dictionary<Player, HandRank> ranks, int numUpWinners)
        {
            this.Players = players;
            this.Ranks = ranks;
        }

        #endregion
    }
}
