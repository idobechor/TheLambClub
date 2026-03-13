using TheLambClub.Models;
using TheLambClub.ModelsLogic;
using static TheLambClub.Models.PlayerModel;

namespace TheLambClub.Models
{
    public abstract class HandRankModel
    {
        #region properties

        public LevelsOfHands? HandType { get; set; }
        public int? PrimaryValue { get; set; }
        public int ?SecondaryValue { get; set; }
        public int[]? Kickers { get; set; }
        public FBCard[]? HandCards { get; set; }

        #endregion

        #region constructors

        public HandRankModel()
        {
        }

        #endregion

        #region public methods

        public abstract int Compare(HandRank other);
        public abstract bool IsBetter(HandRank other);

        #endregion

        #region protected methods

        protected abstract bool IsEqual(HandRank other);

        #endregion
    }
}
