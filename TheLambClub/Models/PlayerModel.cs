using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public abstract class PlayerModel
    {
        Random rnd = new Random();
        public Card card1 = new();
        public Card card2 = new();
        public int SumOfMoney { get; set; }
        public int CurrentBet { get; set; }
        public enum LevelsOfHands
        {
            HighCard,
            Pair,
            TwoPair,
            ThreeOfAKind,
            Straight,
            Flush,
            FullHouse,
            FourOfAKind,
            StraightFlush,
            RoyalFlush
        }
        public LevelsOfHands LevelOfHand { get; set; }
        public abstract void SetLevelOfHand(Board board);
    }
    
}
