using TheLambClub.Models;
using static TheLambClub.Models.FBCard;
using static TheLambClub.Models.PlayerModel;

namespace TheLambClub.ModelsLogic
{
    /// <summary>
    /// Responsible for evaluating the strength of poker hands.
    /// Inherits from HandEvaluatorModel and implements the logic for checking various combinations.
    /// </summary>
    public class HandEvaluator : HandEvaluatorModel
    {
        #region public methods
        /// <summary>
        /// Evaluates the best possible hand from a combination of player cards and board cards.
        /// </summary>
        /// <param name="Card1">Player's first hole card.</param>
        /// <param name="Card2">Player's second hole card.</param>
        /// <param name="boardCards">An array of 5 cards currently on the board.</param>
        /// <returns>A HandRank object representing the highest found ranking.</returns>
        public override HandRank EvaluateBestHand(FBCard Card1, FBCard Card2, FBCard[] boardCards)
        {
            FBCard[] allCards =
            [
                Card1,
                Card2,
                boardCards[0],
                boardCards[1],
                boardCards[2],
                boardCards[3],
                boardCards[4]
            ];
            FBCard[] firstFive = new FBCard[5];
            for (int i = 0; i < 5; i++)
                firstFive[i] = allCards[i];
            HandRank bestHand = EvaluateHighCard(firstFive);
            List<List<FBCard>> combinations = GetCombinationsList(allCards, 5);
            for (int i = 0; i < combinations.Count; i++)
            {
                FBCard[] combinationArray = [.. combinations[i]];
                HandRank currentHand = EvaluateHand(combinationArray);
                if (currentHand.IsBetter(bestHand))
                    bestHand = currentHand;
            }
            return bestHand;
        }
        /// <summary>
        /// Evaluates and ranks a specific 5-card poker hand.
        /// </summary>
        /// <param name="cards">Array of 5 cards to check.</param>
        /// <returns>The HandRank from Royal Flush down to High Card.</returns>
        public override HandRank EvaluateHand(FBCard[] cards)
        {
            HandRank bestHand;
            if (CheckRoyalFlush(cards) != null)
                bestHand = CheckRoyalFlush(cards)!;
            else if (CheckStraightFlush(cards) != null)
                bestHand = CheckStraightFlush(cards)!;
            else if (null != CheckFourOfAKind(cards)!)
                bestHand = CheckFourOfAKind(cards)!;
            else if (CheckFullHouse(cards)! != null)
                bestHand = CheckFullHouse(cards)!;
            else if (null != CheckFlush(cards)!)
                bestHand = CheckFlush(cards)!;
            else if (null != CheckStraight(cards)!)
                bestHand = CheckStraight(cards)!;
            else if (null != CheckThreeOfAKind(cards)!)
                bestHand = CheckThreeOfAKind(cards)!;
            else if (null != CheckTwoPair(cards)!)
                bestHand = CheckTwoPair(cards)!;
            else if (CheckPair(cards)! != null)
                bestHand = CheckPair(cards)!;
            else
                bestHand = EvaluateHighCard(cards);
            return bestHand;
        }
        /// <summary>
        /// Performs a Bubble Sort on an integer array in ascending order.
        /// </summary>
        /// <param name="arr">Integer array to be sorted.</param>
        /// <returns>The sorted array.</returns>
        public override int[] BubbleSort(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
                for (int j = 0; j < arr.Length - i - 1; j++)
                    if (arr[j] > arr[j + 1])
                        (arr[j + 1], arr[j]) = (arr[j], arr[j + 1]);
            return arr;
        }
        /// <summary>
        /// Sorts an integer array in descending order.
        /// </summary>
        /// <param name="arr">Integer array to be sorted.</param>
        /// <returns>The sorted array in descending order.</returns>
        public override int[] SortDescending(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
                for (int j = i + 1; j < arr.Length; j++)
                    if (arr[i] < arr[j])
                        (arr[j], arr[i]) = (arr[i], arr[j]);
            return arr;
        }
        #endregion
        #region protected methods
        /// <summary>
        /// Checks if the hand qualifies as a Royal Flush.
        /// </summary>
        /// <param name="cards">Array of cards to evaluate.</param>
        /// <returns>HandRank if it is a Royal Flush, otherwise null.</returns>
        protected override HandRank? CheckRoyalFlush(FBCard[] cards)
        {
            HandRank StraightFlush = CheckStraightFlush(cards)!;
            HandRank handRank = null!;
            if (StraightFlush != null && StraightFlush?.PrimaryValue == 14)
                handRank = new HandRank
                {
                    HandType = LevelsOfHands.RoyalFlush,
                    PrimaryValue = 14,
                    HandCards = cards
                };
            return handRank;
        }
        /// <summary>
        /// Checks if the hand qualifies as a Straight Flush.
        /// </summary>
        /// <param name="cards">Array of cards to evaluate.</param>
        /// <returns>HandRank if it is a Straight Flush, otherwise null.</returns>
        protected override HandRank? CheckStraightFlush(FBCard[] cards)
        {
            HandRank flush = CheckFlush(cards)!;
            HandRank handRank = null!;
            if (flush != null)
            {
                List<FBCard> flushCardsList = [];
                foreach (FBCard card in cards)
                    if (card.Shape == flush.HandCards![0].Shape)
                        flushCardsList.Add(card);
                HandRank straight = CheckStraight([.. flushCardsList])!;
                if (straight != null)
                {
                    handRank = new HandRank
                    {
                        HandType = LevelsOfHands.StraightFlush,
                        PrimaryValue = straight.PrimaryValue,
                        HandCards = cards
                    };
                }
            }
            return handRank;
        }
        /// <summary>
        /// Checks if the hand contains Four of a Kind.
        /// </summary>
        /// <param name="cards">Array of cards to evaluate.</param>
        /// <returns>HandRank if Four of a Kind is found, otherwise null.</returns>
        protected override HandRank? CheckFourOfAKind(FBCard[] cards)
        {
            HandRank handRank = null!;
            int[] CountArr = new int[13];
            bool foundFour = false;
            int fourValue = 0;
            foreach (FBCard card in cards)
                CountArr[card.Value - 1]++;
            for (int i = 0; i < CountArr.Length; i++)
            {
                if (CountArr[i] == 4)
                {
                    if (i + 1 == 1)
                        fourValue = 14;
                    else if (fourValue < i + 1)
                        fourValue = i + 1;
                    foundFour = true;
                }
            }
            if (foundFour)
            {
                int normalizedFourValue = fourValue == 1 ? 14 : fourValue;
                int kicker = 0;
                for (int i = 0; i < cards.Length; i++)
                    if (cards[i].Value != fourValue)
                    {
                        int cardValue = cards[i].Value == 1 ? 14 : cards[i].Value;
                        if (cardValue > kicker)
                            kicker = cardValue;
                    }
                handRank = new HandRank
                {
                    HandType = LevelsOfHands.FourOfAKind,
                    PrimaryValue = normalizedFourValue,
                    Kickers = [kicker],
                    HandCards = cards
                };
            }
            return handRank;
        }
        /// <summary>
        /// Checks if the hand qualifies as a Full House.
        /// </summary>
        /// <param name="cards">Array of cards to evaluate.</param>
        /// <returns>HandRank if it is a Full House, otherwise null.</returns>
        protected override HandRank? CheckFullHouse(FBCard[] cards)
        {
            HandRank threeOfAKindRank = CheckThreeOfAKind(cards)!;
            HandRank pair = CheckPair(cards)!;
            HandRank handRank = null!;
            if (threeOfAKindRank != null && pair != null)
            {
                handRank = new HandRank
                {
                    HandType = LevelsOfHands.FullHouse,
                    PrimaryValue = threeOfAKindRank.PrimaryValue,
                    SecondaryValue = pair.PrimaryValue
                };
            }
            return handRank;
        }
        /// <summary>
        /// Checks if the hand qualifies as a Flush.
        /// </summary>
        /// <param name="cards">Array of cards to evaluate.</param>
        /// <returns>HandRank if it is a Flush, otherwise null.</returns>
        protected override HandRank? CheckFlush(FBCard[] cards)
        {
            Dictionary<Shapes, int> dict = new()
                {
                    { Shapes.Club, 0 },
                    { Shapes.Diamond, 0 },
                    { Shapes.Heart, 0 },
                    { Shapes.Spade, 0 }
                };
            HandRank handRank = null!;
            foreach (FBCard card in cards)
                dict[card.Shape]++;
            if (!(dict[Shapes.Club] < 5 && dict[Shapes.Diamond] < 5 && dict[Shapes.Heart] < 5 && dict[Shapes.Spade] < 5))
            {
                Shapes flushShape = dict[Shapes.Club] >= 5 ? Shapes.Club :
                dict[Shapes.Diamond] >= 5 ? Shapes.Diamond :
                dict[Shapes.Heart] >= 5 ? Shapes.Heart :
                Shapes.Spade;
                List<FBCard> flushCardsList = [];
                foreach (FBCard card in cards)
                    if (card.Shape == flushShape)
                        flushCardsList.Add(card);
                FBCard[] sortedFlushCards = SortCardsDescendingBubbleSort([.. flushCardsList]);

                FBCard[] finalFlushCards = new FBCard[5];
                int[] finalKickers = new int[4];
                for (int i = 0; i < 5; i++)
                    finalFlushCards[i] = sortedFlushCards[i];
                for (int i = 1; i < 5; i++)
                    finalKickers[i - 1] = sortedFlushCards[i].Value == 1 ? 14 : sortedFlushCards[i].Value;
                int primaryValue = sortedFlushCards[0].Value == 1 ? 14 : sortedFlushCards[0].Value;
                handRank = new HandRank
                {
                    HandType = LevelsOfHands.Flush,
                    PrimaryValue = primaryValue,
                    Kickers = finalKickers,
                    HandCards = finalFlushCards
                };
            }
            return handRank;
        }
        /// <summary>
        /// Sorts an array of FBCards in descending order by their value, handling Aces as high.
        /// </summary>
        /// <param name="cards">Array of cards to sort.</param>
        /// <returns>A sorted array of cards.</returns>
        protected override FBCard[] SortCardsDescendingBubbleSort(FBCard[] cards)
        {
            FBCard[] sortedCards = new FBCard[cards.Length];
            Array.Copy(cards, sortedCards, cards.Length);

            for (int i = 0; i < sortedCards.Length - 1; i++)
            {
                int maxIndex = i;

                for (int j = i + 1; j < sortedCards.Length; j++)
                {
                    int valueJ = sortedCards[j].Value == 1 ? 14 : sortedCards[j].Value;
                    int valueMax = sortedCards[maxIndex].Value == 1 ? 14 : sortedCards[maxIndex].Value;

                    if (valueJ > valueMax)
                        maxIndex = j;
                }
                if (maxIndex != i)
                    (sortedCards[maxIndex], sortedCards[i]) = (sortedCards[i], sortedCards[maxIndex]);
            }

            return sortedCards;
        }
        /// <summary>
        /// Checks if the hand qualifies as a Straight.
        /// </summary>
        /// <param name="cards">Array of cards to evaluate.</param>
        /// <returns>HandRank if it is a Straight, otherwise null.</returns>
        protected override HandRank? CheckStraight(FBCard[] cards)
        {
            HandRank? handRank = null!;
            List<int> distinctValuesList = [];
            for (int i = 0; i < cards.Length; i++)
            {
                int value = cards[i].Value == 1 ? 14 : cards[i].Value;
                bool alreadyExists = false;
                for (int j = 0; j < distinctValuesList.Count; j++)
                    if (distinctValuesList[j] == value)
                    {
                        alreadyExists = true;
                        break;
                    }
                if (!alreadyExists)
                    distinctValuesList.Add(value);
            }
            int[] distinctValues = [.. distinctValuesList];
            distinctValues = BubbleSort(distinctValues);
            for (int i = 0; i <= distinctValues.Length - 5; i++)
            {
                bool isStraight = true;
                for (int j = 1; j < 5; j++)
                    if (distinctValues[i + j] != distinctValues[i] + j)
                    {
                        isStraight = false;
                        break;
                    }
                if (isStraight)
                {
                    handRank = new HandRank
                    {
                        HandType = LevelsOfHands.Straight,
                        PrimaryValue = distinctValues[i + 4],
                        HandCards = cards
                    };
                }
            }
            bool hasAce = false;
            bool hasTwo = false;
            bool hasThree = false;
            bool hasFour = false;
            bool hasFive = false;
            for (int i = 0; i < cards.Length; i++)
            {
                int value = cards[i].Value;
                if (value == 1) hasAce = true;
                else if (value == 2) hasTwo = true;
                else if (value == 3) hasThree = true;
                else if (value == 4) hasFour = true;
                else if (value == 5) hasFive = true;
            }
            if (hasAce && hasTwo && hasThree && hasFour && hasFive)
            {
                handRank = new HandRank
                {
                    HandType = LevelsOfHands.Straight,
                    PrimaryValue = 5,
                    HandCards = cards
                };
            }
            return handRank;
        }
        /// <summary>
        /// Checks if the hand contains Three of a Kind.
        /// </summary>
        /// <param name="cards">Array of cards to evaluate.</param>
        /// <returns>HandRank if Three of a Kind is found, otherwise null.</returns>
        protected override HandRank? CheckThreeOfAKind(FBCard[] cards)
        {
            HandRank handRank = null!;
            int threeOfAKindValue = 0;
            int[] CountArr = new int[14];
            int[] CardsValues = new int[cards.Length];
            int[] kickers = new int[2];
            for (int i = 0; i < cards.Length; i++)
            {
                CardsValues[i] = cards[i].Value == LowValueOfAce ? HighValueOfAce : cards[i].Value;
            }
            for (int i = 0; i < CountArr.Length; i++)
                CountArr[i] = FindPairValue(CardsValues, i + 1);
            for (int i = 0; i < CountArr.Length; i++)
                if (CountArr[i] == 3 && i + 1 > threeOfAKindValue)
                    threeOfAKindValue = i + 1;
            CardsValues = SortDescending(CardsValues);
            int k = 0;
            for (int i = 0; i < CardsValues.Length && k < 2; i++)
                if (CardsValues[i] != threeOfAKindValue)
                    kickers[k++] = CardsValues[i];
            if (threeOfAKindValue != 0)
                handRank = new HandRank
                {
                    HandType = PlayerModel.LevelsOfHands.ThreeOfAKind,
                    PrimaryValue = threeOfAKindValue,
                    Kickers = kickers,
                    HandCards = cards
                };
            return handRank;
        }
        /// <summary>
        /// Checks if the hand qualifies as Two Pair.
        /// </summary>
        /// <param name="cards">Array of cards to evaluate.</param>
        /// <returns>HandRank if two pairs are found, otherwise null.</returns>
        protected override HandRank? CheckTwoPair(FBCard[] cards)
        {
            HandRank handRank = null!;
            int FirstPair = 0;
            int SecondPair = 0;
            int[] CountArr = new int[14]; // 1..13 + ace(14)
            int[] CardsValues = new int[cards.Length];
            int kicker = 0;
            for (int i = 0; i < cards.Length; i++)
                CardsValues[i] = cards[i].Value == LowValueOfAce ? HighValueOfAce : cards[i].Value;
            for (int i = 0; i < CountArr.Length; i++)
                CountArr[i] = FindPairValue(CardsValues, i + 1);
            for (int i = 0; i < CountArr.Length; i++)
            {
                if (CountArr[i] == 2)
                {
                    int value = i + 1;

                    if (value > FirstPair)
                    {
                        SecondPair = FirstPair;
                        FirstPair = value;
                    }
                    else if (value > SecondPair)
                        SecondPair = value;
                }
            }
            for (int i = 0; i < CardsValues.Length; i++)
                if (CardsValues[i] != FirstPair && CardsValues[i] != SecondPair)
                    if (CardsValues[i] > kicker)
                        kicker = CardsValues[i];
            if (FirstPair != 0 && SecondPair != 0)
            {
                int highPair = FirstPair > SecondPair ? FirstPair : SecondPair;
                int lowPair = FirstPair < SecondPair ? FirstPair : SecondPair;

                handRank = new HandRank
                {
                    HandType = PlayerModel.LevelsOfHands.TwoPair,
                    PrimaryValue = highPair,
                    SecondaryValue = lowPair,
                    Kickers = [kicker],
                    HandCards = cards
                };
            }

            return handRank;
        }
        /// <summary>
        /// Checks if the hand contains a Pair.
        /// </summary>
        /// <param name="cards">Array of cards to evaluate.</param>
        /// <returns>HandRank if a pair is found, otherwise null.</returns>
        protected override HandRank? CheckPair(FBCard[] cards)
        {
            HandRank? handRank = null!;
            int pair = 0;
            int[] CountArr = new int[14]; // 1..13 + ace(14)
            int[] CardsValues = new int[cards.Length];
            int[] kickers = new int[3];
            for (int i = 0; i < cards.Length; i++)
                CardsValues[i] = cards[i].Value == LowValueOfAce ? HighValueOfAce : cards[i].Value;
            for (int i = 0; i < CountArr.Length; i++)
                CountArr[i] = FindPairValue(CardsValues, i + 1);
            for (int i = 0; i < CountArr.Length; i++)
                if (CountArr[i] == 2)
                    pair = i + 1;
            CardsValues = SortDescending(CardsValues);
            int k = 0;
            for (int i = 0; i < CardsValues.Length && k < 3; i++)
                if (CardsValues[i] != pair)
                    kickers[k++] = CardsValues[i];
            if (pair != 0)
                handRank = new HandRank
                {
                    HandType = LevelsOfHands.Pair,
                    PrimaryValue = pair,
                    Kickers = kickers,
                    HandCards = cards
                };
            return handRank;
        }
        /// <summary>
        /// Counts how many times a specific value appears in an array of cards.
        /// </summary>
        /// <param name="cards">Array of card values.</param>
        /// <param name="num">The value to search for.</param>
        /// <returns>The number of occurrences.</returns>
        protected override int FindPairValue(int[] cards, int num)
        {
            int count = 0;
            foreach (int card in cards)
                if (card == num)
                    count++;
            return count;
        }
        /// <summary>
        /// Evaluates the hand based on the lowest rank - High Card.
        /// </summary>
        /// <param name="cards">Array of cards to evaluate.</param>
        /// <returns>A HandRank representing the High Card and relevant kickers.</returns>
        protected override HandRank EvaluateHighCard(FBCard[] cards)
        {
            int[] kickersArray = new int[cards.Length];
            for (int i = 0; i < cards.Length; i++)
                kickersArray[i] = cards[i].Value == LowValueOfAce ? HighValueOfAce : cards[i].Value;
            kickersArray = SortDescending(kickersArray);

            int[] kickers = new int[4];
            for (int i = 1; i < kickersArray.Length; i++)
                kickers[i - 1] = kickersArray[i];
            return new HandRank
            {
                HandType = LevelsOfHands.HighCard,
                PrimaryValue = kickersArray[0],
                Kickers = kickers,
                HandCards = cards
            };
        }
        /// <summary>
        /// Generates a list of all possible combinations of a set of cards of a given size.
        /// </summary>
        /// <param name="cards">The full array of cards.</param>
        /// <param name="five">The number of cards in each combination (typically 5).</param>
        /// <returns>A list of lists containing the card combinations.</returns>
        protected override List<List<FBCard>> GetCombinationsList(FBCard[] cards, int five)
        {
            List<List<FBCard>> result = [];
            int n = cards.Length;
            if (five <= n && five > 0)
            {
                int[] indices = new int[five];
                for (int i = 0; i < five; i++)
                    indices[i] = i;
                while (true)
                {
                    List<FBCard> combination = [];
                    for (int j = 0; j < five; j++)
                        combination.Add(cards[indices[j]]);
                    result.Add(combination);
                    //  Find the rightmost index that has not yet reached its maximum possible value
                    int i = five - 1;
                    while (i >= 0 && indices[i] == n - five + i)
                        i--;
                    //  If all indices are at their maximum (i < 0), we have found all combinations
                    if (i < 0)
                        break;
                    //  Increment the found index by 1
                    indices[i]++;
                    //  Reset all indices to the right of 'i' to be strictly increasing (e.g., [1, 5, 6] -> [2, 3, 4])
                    for (int j = i + 1; j < five; j++)
                        indices[j] = indices[j - 1] + 1;
                }
            }
            return result; 
        }
        #endregion
    }
}