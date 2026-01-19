
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
  
    public class Player : PlayerModel
    {
        public Player() : base(String.Empty, String.Empty) { }
        public Player(string playerName, string id) : base(playerName,id)
        {
            if (CurrentMoney == 0)
            {
                CurrentMoney = 10000;
                CurrentBet = 0;
            }
        }
        public override HandRank EvaluateBestHand(FBCard[] boardCards)
        {            
            HandRank handRank = new HandEvaluator().EvaluateBestHand(FBCard1!, FBCard2!, boardCards);           
            LevelOfHand = handRank.HandType;           
            return handRank;
        }
    }
}
