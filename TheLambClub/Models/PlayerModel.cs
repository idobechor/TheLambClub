using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public abstract class PlayerModel
    {
        private readonly SetOfCards setCards = new();

        public Card card1 { get; set; }
        public Card card2 { get; set; }
        public string Name { get; set; } 
        public string Id { get; set; } 
        public PlayerModel(string name, string id)
        {
            card1 = setCards.GetRandomCard();
            card2 = setCards.GetRandomCard();
            Name = name;
            Id = id;

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
