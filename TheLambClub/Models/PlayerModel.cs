using Plugin.CloudFirestore.Attributes;
using TheLambClub.ModelsLogic;
using TheLambClub.ViewModel;

namespace TheLambClub.Models
{
    public abstract class PlayerModel
    {
        [Ignored]
        protected readonly SetOfCards setCards = new();
        public bool IsFolded { get; set; }= false;
        public FBCard FBCard1 { get; set; }
        public FBCard FBCard2 { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public double CurrentBet { get; set; } = 0;
        public PlayerModel(string name, string id)
        {
            //Card1 = new ViewCard();
            //Card2 = new ViewCard();
            Name = name;
            Id = id;
        }
        //public int SumOfMoney { get; set; }
        //public int CurrentBet { get; set; }
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
