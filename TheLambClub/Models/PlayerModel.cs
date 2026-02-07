using Plugin.CloudFirestore.Attributes;
using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public abstract class PlayerModel(string name, string id)
    {
        [Ignored]
        protected readonly SetOfCards setCards = new();
        public bool IsFolded { get; set; }= false;
        public FBCard ?FBCard1 { get; set; }
        public FBCard ?FBCard2 { get; set; }
        public string Name { get; set; } = name;
        public string Id { get; set; } = id;
        public double CurrentBet { get; set; }
        private double _currentMoney = 10000; // ערך התחלתי

        public double CurrentMoney
        {
            get => _currentMoney;
            set => _currentMoney = value;                   
            
        }
        public bool IsReRazed { get; set; } = false;

        public bool IsAllIn { get; set; } = false;
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
        [Ignored]
        public LevelsOfHands LevelOfHand { get; set; }
        public abstract HandRank EvaluateBestHand(FBCard[] boardCards);
    }
}
