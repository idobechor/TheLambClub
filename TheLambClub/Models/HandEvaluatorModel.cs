using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    /// <summary>
    /// Represents an abstract base model for evaluating poker hands, defining constants 
    /// and methods required to determine the strength of card combinations.
    /// </summary>
    public abstract class HandEvaluatorModel
    {
        #region fields

        /// <summary>
        /// The high numerical value assigned to an Ace.
        /// </summary>
        protected const int HighValueOfAce = 14;

        /// <summary>
        /// The low numerical value assigned to an Ace (e.g., for a low straight).
        /// </summary>
        protected const int LowValueOfAce = 1;

        #endregion

        #region public methods

        /// <summary>
        /// Evaluates the best possible five-card hand from a combination of two player cards and the community board cards.
        /// </summary>
        /// <param name="Card1">The first private player card.</param>
        /// <param name="Card2">The second private player card.</param>
        /// <param name="boardCards">The array of community cards on the board.</param>
        /// <returns>The highest achievable <see cref="HandRank"/>.</returns>
        public abstract HandRank EvaluateBestHand(FBCard Card1, FBCard Card2, FBCard[] boardCards);

        /// <summary>
        /// Evaluates the rank of a specific set of five cards.
        /// </summary>
        /// <param name="cards">The array of cards to evaluate.</param>
        /// <returns>The calculated <see cref="HandRank"/>.</returns>
        public abstract HandRank EvaluateHand(FBCard[] cards);

        /// <summary>
        /// Sorts an integer array using the bubble sort algorithm.
        /// </summary>
        /// <param name="arr">The array to be sorted.</param>
        /// <returns>A sorted array of integers.</returns>
        public abstract int[] BubbleSort(int[] arr);

        /// <summary>
        /// Sorts an integer array in descending order.
        /// </summary>
        /// <param name="arr">The array to be sorted.</param>
        /// <returns>A sorted array of integers in descending order.</returns>
        public abstract int[] SortDescending(int[] arr);

        #endregion

        #region protected methods

        /// <summary>
        /// Checks if the cards form a Royal Flush.
        /// </summary>
        /// <param name="cards">The cards to check.</param>
        /// <returns>A <see cref="HandRank"/> if true; otherwise, null.</returns>
        protected abstract HandRank? CheckRoyalFlush(FBCard[] cards);

        /// <summary>
        /// Checks if the cards form a Straight Flush.
        /// </summary>
        /// <param name="cards">The cards to check.</param>
        /// <returns>A <see cref="HandRank"/> if true; otherwise, null.</returns>
        protected abstract HandRank? CheckStraightFlush(FBCard[] cards);

        /// <summary>
        /// Checks if the cards contain Four of a Kind.
        /// </summary>
        /// <param name="cards">The cards to check.</param>
        /// <returns>A <see cref="HandRank"/> if true; otherwise, null.</returns>
        protected abstract HandRank? CheckFourOfAKind(FBCard[] cards);

        /// <summary>
        /// Checks if the cards form a Full House.
        /// </summary>
        /// <param name="cards">The cards to check.</param>
        /// <returns>A <see cref="HandRank"/> if true; otherwise, null.</returns>
        protected abstract HandRank? CheckFullHouse(FBCard[] cards);

        /// <summary>
        /// Checks if the cards form a Flush.
        /// </summary>
        /// <param name="cards">The cards to check.</param>
        /// <returns>A <see cref="HandRank"/> if true; otherwise, null.</returns>
        protected abstract HandRank? CheckFlush(FBCard[] cards);

        /// <summary>
        /// Sorts an array of cards in descending order using the bubble sort algorithm.
        /// </summary>
        /// <param name="cards">The cards to sort.</param>
        /// <returns>An array of sorted cards.</returns>
        protected abstract FBCard[] SortCardsDescendingBubbleSort(FBCard[] cards);

        /// <summary>
        /// Checks if the cards form a Straight.
        /// </summary>
        /// <param name="cards">The cards to check.</param>
        /// <returns>A <see cref="HandRank"/> if true; otherwise, null.</returns>
        protected abstract HandRank? CheckStraight(FBCard[] cards);

        /// <summary>
        /// Checks if the cards contain Three of a Kind.
        /// </summary>
        /// <param name="cards">The cards to check.</param>
        /// <returns>A <see cref="HandRank"/> if true; otherwise, null.</returns>
        protected abstract HandRank? CheckThreeOfAKind(FBCard[] cards);

        /// <summary>
        /// Checks if the cards form Two Pairs.
        /// </summary>
        /// <param name="cards">The cards to check.</param>
        /// <returns>A <see cref="HandRank"/> if true; otherwise, null.</returns>
        protected abstract HandRank? CheckTwoPair(FBCard[] cards);

        /// <summary>
        /// Checks if the cards form a single Pair.
        /// </summary>
        /// <param name="cards">The cards to check.</param>
        /// <returns>A <see cref="HandRank"/> if true; otherwise, null.</returns>
        protected abstract HandRank? CheckPair(FBCard[] cards);

        /// <summary>
        /// Finds the value of a pair that appears a specific number of times in the set.
        /// </summary>
        /// <param name="cards">The array of card values.</param>
        /// <param name="num">The frequency count to look for.</param>
        /// <returns>The numerical value of the pair.</returns>
        protected abstract int FindPairValue(int[] cards, int num);

        /// <summary>
        /// Evaluates the highest card in the set when no better hand rank is found.
        /// </summary>
        /// <param name="cards">The cards to evaluate.</param>
        /// <returns>A <see cref="HandRank"/> representing High Card.</returns>
        protected abstract HandRank EvaluateHighCard(FBCard[] cards);

        /// <summary>
        /// Generates all possible combinations of size k from a given array of cards.
        /// </summary>
        /// <param name="cards">The total pool of cards.</param>
        /// <param name="k">The number of cards in each combination.</param>
        /// <returns>A list of card combinations.</returns>
        protected abstract List<List<FBCard>> GetCombinationsList(FBCard[] cards, int k);

        #endregion
    }
}