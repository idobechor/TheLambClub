using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public abstract class HandEvaluatorModel
    {
        protected const int HighValueOfAce = 14;
        protected const int LowValueOfAce = 1;
        public abstract HandRank EvaluateBestHand(FBCard Card1, FBCard Card2, FBCard[] boardCards);
        public abstract HandRank EvaluateHand(FBCard[] cards);
        public abstract int[] BubbleSort(int[] arr);
        public abstract int[] SortDescending(int[] arr);
        protected abstract HandRank? CheckRoyalFlush(FBCard[] cards);
        protected abstract HandRank? CheckStraightFlush(FBCard[] cards);
        protected abstract HandRank? CheckFourOfAKind(FBCard[] cards);
        protected abstract HandRank? CheckFullHouse(FBCard[] cards);
        protected abstract HandRank? CheckFlush(FBCard[] cards);
        protected abstract FBCard[] SortCardsDescendingBubbleSort(FBCard[] cards);
        protected abstract HandRank? CheckStraight(FBCard[] cards);
        protected abstract HandRank? CheckThreeOfAKind(FBCard[] cards);
        protected abstract HandRank? CheckTwoPair(FBCard[] cards);
        protected abstract HandRank? CheckPair(FBCard[] cards);
        protected abstract int FindPairValue(int[] cards, int num);
        protected abstract HandRank EvaluateHighCard(FBCard[] cards);
        protected abstract List<List<FBCard>> GetCombinationsList(FBCard[] cards, int k);
    }
}
