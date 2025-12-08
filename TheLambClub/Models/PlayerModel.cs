using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public abstract class PlayerModel
    {
        protected readonly SetOfCards setCards = new();

        public FBCard FBCard1 { get; set; }
        public FBCard FBCard2 { get; set; }
        public Card card1 { get; set; }
        public Card card2 { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public bool IsCurrentTurn { get; set; }
        public PlayerModel(string name, string id)
        {
           FBCard1 = setCards.GetRandomCard();
            FBCard2 = setCards.GetRandomCard();
            card1 = new Card();
            card2 = new Card();
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
        //public abstract void SetLevelOfHand(Board board);
    }
}
