using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public abstract class WinningPopupModel(Player[] players, Dictionary<Player, HandRank> ranks)
    {
        #region properties

        protected Player[] Players { get; set; } = players;
        protected Dictionary<Player, HandRank> Ranks { get; set; } = ranks;
        public abstract string[] PlayersNames { get; }
        #endregion
    }
}
