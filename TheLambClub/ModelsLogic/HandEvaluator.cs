using System.Collections.Generic;
using TheLambClub.Models;
using static TheLambClub.Models.FBCard;
using static TheLambClub.Models.PlayerModel;

namespace TheLambClub.ModelsLogic
{
    public  class HandEvaluator : HandEvaluatorModel
    {
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
            {
                firstFive[i] = allCards[i];
            }
            HandRank bestHand = EvaluateHighCard(firstFive); 
           
            List<List<FBCard>> combinations = GetCombinationsList(allCards, 5);
            
            for (int i = 0; i < combinations.Count; i++)
            {
                FBCard[] combinationArray = combinations[i].ToArray();
                HandRank currentHand = EvaluateHand(combinationArray);
                if (currentHand.IsBetter(bestHand))
                {
                    bestHand = currentHand;
                }
            }
            
            return bestHand;
        }
        public  override HandRank EvaluateHand(FBCard[] cards)
        {
            bool found=false;
            HandRank bestHand = null!;
            if (CheckRoyalFlush(cards) != null && !found)
            {
                bestHand = CheckRoyalFlush(cards)!;
                found = true;
            }

            if (CheckStraightFlush(cards) != null && !found)
            {
                bestHand = CheckStraightFlush(cards)!;
                found = true;
            }

            if (null != CheckFourOfAKind(cards)! && !found)
            { 
                bestHand = CheckFourOfAKind(cards)!; 
                found = true;
            }

            if (CheckFullHouse(cards)! != null && !found)
            {
                bestHand = CheckFullHouse(cards)!; 
                found = true;
            }             

            if (null != CheckFlush(cards)! && !found)
            { 
                bestHand = CheckFlush(cards)!; 
                found = true;
            }

            if (null != CheckStraight(cards)!&&!found)
            {
                bestHand = CheckStraight(cards)!; 
                found = true;
            }

            if (null != CheckThreeOfAKind(cards)! && !found)
            {
                bestHand= CheckThreeOfAKind(cards)!;
                found = true;
            }

            if (null != CheckTwoPair(cards)! && !found)
            {
                bestHand = CheckTwoPair(cards)!;
                found = true;
            }

            if (CheckPair(cards)! != null && !found)
            {
                bestHand = CheckPair(cards)!;
               found= true;
            }
            if (!found)
            {
                bestHand= EvaluateHighCard(cards);
            }
            return bestHand;
        }
        public override int[] BubbleSort(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                for (int j = 0; j < arr.Length - i - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }
            return arr;
        }
       public override int[] SortDescending(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                for (int j = i + 1; j < arr.Length; j++)
                {
                    if (arr[i] < arr[j])
                    {
                        int temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
                }
            }
            return arr;
        }
        protected override HandRank? CheckRoyalFlush(FBCard[] cards)
        {
            HandRank StraightFlush = CheckStraightFlush(cards)!;
            HandRank handRank = null!;
            if (StraightFlush != null && StraightFlush?.PrimaryValue==14)
                handRank= new HandRank
                         {
                          HandType = LevelsOfHands.RoyalFlush,
                          PrimaryValue = 14, // ברויאל פלאש האס הוא תמיד יהיה הקלף הגבוהה ביותר
                          HandCards = cards
                         };
            return handRank;
        }  
        protected override HandRank? CheckStraightFlush(FBCard[] cards)
        {
            HandRank flush = CheckFlush(cards)!;
            HandRank handRank = null!;
            if (flush != null)
            {
                List<FBCard> flushCardsList = [];
                foreach (FBCard card in cards)
                {
                    if (card.Shape == flush.HandCards[0].Shape)
                    {
                        flushCardsList.Add(card);
                    }
                }
                HandRank straight = CheckStraight(flushCardsList.ToArray())!;
                if (straight != null)
                {
                    handRank= new HandRank
                             {
                        HandType = LevelsOfHands.StraightFlush,
                        PrimaryValue = straight.PrimaryValue,
                        HandCards = cards
                             };
                }
            }

            return handRank;
        }
        protected override HandRank? CheckFourOfAKind(FBCard[] cards)
        {
            HandRank handRank = null!;
            int[]CountArr = new int[13];
            bool foundFour = false;
            int fourValue = 0;
            foreach (FBCard card in cards)
            {
                CountArr[card.Value - 1]++;
            }
            for (int i = 0; i < CountArr.Length; i++)
            {
                if (CountArr[i] == 4)
                {
                    if (i + 1 == 1)
                    {
                        fourValue = 14;
                    }
                    else if (fourValue < i + 1)
                    {
                        fourValue = i + 1;
                    }
                    foundFour = true;
                }
            }
            if (foundFour)
            {
                int normalizedFourValue = fourValue == 1 ? 14 : fourValue;
                int kicker = 0;
                for (int i = 0; i < cards.Length; i++)
                {
                    if (cards[i].Value != fourValue)
                    {
                        int cardValue = cards[i].Value == 1 ? 14 : cards[i].Value;
                        if (cardValue > kicker)
                        {
                            kicker = cardValue;
                        }
                    }
                }               
                handRank= new HandRank
                {
                    HandType = LevelsOfHands.FourOfAKind,
                    PrimaryValue = normalizedFourValue,
                    Kickers = new[] { kicker },
                    HandCards = cards
                };
            }
            
            return handRank;
        }
        protected override HandRank? CheckFullHouse(FBCard[] cards)
        {
            HandRank threeOfAKindRank = CheckThreeOfAKind(cards)!;
            HandRank pair = CheckPair(cards)!;
            HandRank handRank =null!;

            if (threeOfAKindRank!=null&&pair!=null)
            {               
                handRank= new HandRank
                {
                    HandType = LevelsOfHands.FullHouse,
                    PrimaryValue = threeOfAKindRank.PrimaryValue,
                    SecondaryValue = pair.PrimaryValue
                };
            }
            return handRank;
        }
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
            {
                dict[card.Shape]++;
            }

            if (!(dict[Shapes.Club] < 5 && dict[Shapes.Diamond] < 5 && dict[Shapes.Heart] < 5 && dict[Shapes.Spade] < 5))
            {
              Shapes flushShape = dict[Shapes.Club] >= 5 ? Shapes.Club :
              dict[Shapes.Diamond] >= 5 ? Shapes.Diamond :
              dict[Shapes.Heart] >= 5 ? Shapes.Heart :
              Shapes.Spade;

                List<FBCard> flushCardsList = [];
                foreach (FBCard card in cards)
                {
                    if (card.Shape == flushShape)
                        flushCardsList.Add(card);
                }

                FBCard[] sortedFlushCards = SortCardsDescendingBubbleSort(flushCardsList.ToArray());

                FBCard[] finalFlushCards = new FBCard[5];
                int[] finalKickers = new int[4];
                for (int i = 0; i < 5; i++)
                {
                    finalFlushCards[i] = sortedFlushCards[i];
                }
                for (int i = 1; i < 5; i++)
                {
                    finalKickers[i - 1] = sortedFlushCards[i].Value == 1 ? 14 : sortedFlushCards[i].Value;
                }

                int primaryValue = sortedFlushCards[0].Value == 1 ? 14 : sortedFlushCards[0].Value;

                handRank= new HandRank
                {
                    HandType = LevelsOfHands.Flush,
                    PrimaryValue = primaryValue,
                    Kickers = finalKickers,
                    HandCards = finalFlushCards
                };
            }
            return handRank;
        }
        protected override FBCard[] SortCardsDescendingBubbleSort(FBCard[] cards)
        {
            FBCard[] sortedCards = new FBCard[cards.Length];
            Array.Copy(cards, sortedCards, cards.Length);

            for (int i = 0; i < sortedCards.Length - 1; i++)
            {
                int maxIndex = i;

                for (int j = i + 1; j < sortedCards.Length; j++)
                {
                    int valueJ = sortedCards[j].Value == 1 ? 14 : sortedCards[j].Value;      // Ace = 14
                    int valueMax = sortedCards[maxIndex].Value == 1 ? 14 : sortedCards[maxIndex].Value;

                    if (valueJ > valueMax)
                    {
                        maxIndex = j;
                    }
                }
                if (maxIndex != i)
                {
                    FBCard temp = sortedCards[i];
                    sortedCards[i] = sortedCards[maxIndex];
                    sortedCards[maxIndex] = temp;
                }
            }

            return sortedCards;
        }
        protected override HandRank? CheckStraight(FBCard[] cards)
        {
            //בסטרייט אין לנו שימוש בכפילויות ולכן נרצה להסיר אותם
            HandRank? handRank = null!;
            List<int> distinctValuesList = [];
            for (int i = 0; i < cards.Length; i++)
            {
                int value = cards[i].Value == 1 ? 14 : cards[i].Value;
                bool alreadyExists = false;
                for (int j = 0; j < distinctValuesList.Count; j++)
                {
                    if (distinctValuesList[j] == value)
                    {
                        alreadyExists = true;
                        break;
                    }
                }
                if (!alreadyExists)
                {
                    distinctValuesList.Add(value);
                }
            }
                //העברתי למערך בגלל שהפעולה היחידה שיש לי שממינת זה למערכים
            int[] distinctValues = distinctValuesList.ToArray();
            distinctValues=BubbleSort(distinctValues);
            // Check for regular straight
            for (int i = 0; i <= distinctValues.Length - 5; i++)
            {
                bool isStraight = true;
                for (int j = 1; j < 5; j++)
                {
                    if (distinctValues[i + j] != distinctValues[i] + j)
                    {
                        isStraight = false;
                        break;//ראינו שאין סטרייט אז נצא מהלולאה 
                    }
                }
                
                if (isStraight)
                {
                    handRank= new HandRank
                    {
                        HandType = LevelsOfHands.Straight,
                        PrimaryValue = distinctValues[i + 4], 
                        HandCards = cards
                    };
                }
            }
            
            // בגלל שלאס יש 2 ערכים יש לנו מקרה קצה שצריך לטפל בו (המרנו את אס ל14 לפני)
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
                handRank= new HandRank
                {
                    HandType = LevelsOfHands.Straight,
                    PrimaryValue = 5, 
                    HandCards = cards
                };
            }

           return handRank;
        }   
        protected override HandRank? CheckThreeOfAKind(FBCard[] cards)
        {
            HandRank handRank = null!;
            int threeOfAKindValue = 0;
            int[] CountArr = new int[13];
            int[] CardsValues = new int[cards.Length];
            //בגלל שבפוקר לוקחים את ה5 קלפים הכי טובים אז יש רק 3 קיקרים
            int[] kickers = new int[2];
            for (int i = 0; i < cards.Length; i++)
            {
                CardsValues[i] = cards[i].Value == LowValueOfAce ? HighValueOfAce : cards[i].Value;
            }
            for (int i = 0; i < CountArr.Length; i++)
                CountArr[i] = FindPairValue(CardsValues, i + 1);
            for (int i = 0; i < CountArr.Length; i++)
            {
                if (CountArr[i] == 3 && i + 1 > threeOfAKindValue)
                {
                    threeOfAKindValue = i + 1;
                }
            }
            CardsValues = SortDescending(CardsValues);
            int k = 0;
            for (int i = 0; i < CardsValues.Length && k < 2; i++)
            {
                if (CardsValues[i] != threeOfAKindValue)
                {
                    kickers[k++] = CardsValues[i];
                }
            }
            if (threeOfAKindValue != 0)
                handRank= new HandRank
                {
                    HandType = PlayerModel.LevelsOfHands.ThreeOfAKind,
                    PrimaryValue = threeOfAKindValue,
                    Kickers = kickers,
                    HandCards = cards
                };                      
            return handRank;
        }
        protected override HandRank? CheckTwoPair(FBCard[] cards)
        {
            HandRank handRank = null!;
            int FirstPair = 0;
            int SecondPair = 0;
            int[] CountArr = new int[13];
            int[] CardsValues = new int[cards.Length];        
            int kicker = 0;
            for (int i = 0; i < cards.Length; i++)
            {
                CardsValues[i] = cards[i].Value == LowValueOfAce ? HighValueOfAce : cards[i].Value;
            }
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
                    {
                        SecondPair = value;
                    }
                }
            }
            for (int i = 0; i < CardsValues.Length; i++)
            {
                if (CardsValues[i] != FirstPair && CardsValues[i] != SecondPair)
                {
                    if (CardsValues[i] > kicker)
                        kicker = CardsValues[i];
                }
            }
            if (FirstPair != 0 && SecondPair != 0)
            {
                int highPair = FirstPair > SecondPair ? FirstPair : SecondPair;
                int lowPair = FirstPair < SecondPair ? FirstPair : SecondPair;

                handRank= new HandRank
                {
                    HandType = PlayerModel.LevelsOfHands.TwoPair,
                    PrimaryValue = highPair,
                    SecondaryValue = lowPair,
                    Kickers = new[] { kicker },
                    HandCards = cards
                };
            }
            
            return handRank;
        }
        protected override HandRank? CheckPair(FBCard[] cards)
        {     
            HandRank? handRank = null!;
            int pair = 0;
            int[] CountArr = new int[13];
            int[] CardsValues = new int[cards.Length];
            //בגלל שבפוקר לוקחים את ה5 קלפים הכי טובים אז יש רק 3 קיקרים
            int[] kickers = new int[3];
            for (int i = 0; i < cards.Length; i++)
            {
                CardsValues[i] = cards[i].Value == LowValueOfAce ? HighValueOfAce : cards[i].Value;
            }
            for (int i = 0; i < CountArr.Length; i++)
                CountArr[i]= FindPairValue(CardsValues, i+1);
          for (int i = 0; i < CountArr.Length; i++)
            {
                if (CountArr[i]==2)
                {
                    pair = i + 1;
                }
            }
             CardsValues = SortDescending(CardsValues);
            int k = 0;
            for (int i = 0; i < CardsValues.Length && k < 3; i++)
            {
                if (CardsValues[i] != pair)
                {
                    kickers[k++] = CardsValues[i];
                }
            }
            if (pair != 0)
                handRank= new HandRank
                {
                    HandType = LevelsOfHands.Pair,
                    PrimaryValue = pair,
                    Kickers = kickers,
                    HandCards = cards
                };
            return handRank;
        }
        protected override int FindPairValue(int[] cards,int num)
        {
            int count = 0;
            foreach (int card in cards) 
            {
            if (card == num)
                count++;
            }
            return count;
        }

        protected override HandRank EvaluateHighCard(FBCard[] cards)
        {
            int[] kickersArray = new int[cards.Length];
            //אסים שווים גם 1 וגם 14 ולכן אין ברירה אלה להמיר אותם ל14
            for (int i = 0; i < cards.Length; i++)
            {
                kickersArray[i] = cards[i].Value == LowValueOfAce ? HighValueOfAce : cards[i].Value;
            }
            kickersArray =SortDescending(kickersArray);
            
            int[] kickers = new int[4];
            for (int i = 1; i < kickersArray.Length; i++)
            {
                kickers[i - 1] = kickersArray[i];
            }
            
            return new HandRank
            {
                HandType = LevelsOfHands.HighCard,
                PrimaryValue = kickersArray[0],
                Kickers = kickers,
                HandCards = cards
            };
        }
        protected override  List<List<FBCard>> GetCombinationsList(FBCard[] cards, int k)
        {
            List<List<FBCard>> result = [];
            GetCombinationsRecursive(cards, k, 0, new List<FBCard>(), result);
            return result;
        }
        private static void GetCombinationsRecursive(FBCard[] cards, int k, int startIndex, 
            List<FBCard> current, List<List<FBCard>> result)
        {
            if (current.Count == k)
            {
                List<FBCard> combination = [];
                for (int i = 0; i < current.Count; i++)
                {
                    combination.Add(current[i]);
                }
                result.Add(combination);
                return;
            }
            
            for (int i = startIndex; i < cards.Length; i++)
            {
                current.Add(cards[i]);
                GetCombinationsRecursive(cards, k, i + 1, current, result);
                current.RemoveAt(current.Count - 1);
            }
        }
    }
}
