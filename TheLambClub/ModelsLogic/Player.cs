
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
  
    public class Player : PlayerModel
    {
        public Player() : base(String.Empty, String.Empty) { }
        public Player(string playerName, string id) : base(playerName,id)
        {
        }
       
        public HandRank EvaluateBestHand(FBCard[] boardCards)
        {
            
            HandRank handRank = HandEvaluator.EvaluateBestHand(FBCard1, FBCard2, boardCards);           
            LevelOfHand = handRank.HandType;
            
            return handRank;
        }
    }
}
