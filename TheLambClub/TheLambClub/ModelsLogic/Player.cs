using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    public class Player : PlayerModel
    {
        #region constructors

        public Player() : base(String.Empty, String.Empty) { }
        public Player(string playerName, string id) : base(playerName, id)
        {
        }

        #endregion

        #region public methods

        public override HandRank EvaluateBestHand(FBCard[] boardCards)
        {
            HandRank handRank = new HandEvaluator().EvaluateBestHand(FBCard1!, FBCard2!, boardCards);
            LevelOfHand = (LevelsOfHands)handRank.HandType!;
            return handRank;
        }

        #endregion
    }
}
