using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public abstract class PlayerModel
    {
        private readonly SetOfCards setCards = new();
        Random rnd = new Random();
        public Card card1;
        public Card card2;
        public PlayerModel()
        {
            card1 = setCards.GetRandomCard();
            card2 = setCards.GetRandomCard();
        }
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
