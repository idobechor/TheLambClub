using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    public class Player : PlayerModel
    {
        public Player(string playerName, string id) : base(playerName,id)
        {
        }

        //public override void SetLevelOfHand(Board b)
        //{
        //    int numOfPairs = 0;
        //    int numOfSameShapeCard1 = 0;
        //    int numOfSameShapeCard2 = 0;
        //    int sumNumOfSameShape = 0;
        //    int card1NumOfPairs = 0;
        //    int card2NumOfPairs = 0;
        //    bool hasStraight = true;
        //    int[] sortedArr = GetSortedDistinctRanks(b.Cards);
        //    for (int i = 0; i < sortedArr.Length - 1; i++)
        //    {
        //        if (sortedArr[i] != sortedArr[i + 1])
        //        {
        //            hasStraight = false;
        //            break;
        //        }
        //    }
        //    for (int i = 0; i < b.Cards.Length; i++)
        //    {
        //        if (b.Cards[i].Value == card1.Value)
        //        {
        //            card1NumOfPairs++;
        //        }
        //        if (b.Cards[i].Value == card2.Value)
        //        {
        //            card2NumOfPairs++;
        //        }
        //        if (b.Cards[i].Shape == card1.Shape)
        //        {
        //            numOfSameShapeCard1++;
        //        }
        //        if (b.Cards[i].Shape == card2.Shape)
        //        {
        //            numOfSameShapeCard2++;
        //        }
        //    }
        //    if (card1.Shape == card2.Shape)
        //    {
        //        sumNumOfSameShape = numOfSameShapeCard1 + numOfSameShapeCard2 + 1;
        //    }
        //    if (numOfPairs == 1)
        //    {
        //        LevelOfHand = LevelsOfHands.Pair;
        //    }
        //    if (numOfPairs == 2)
        //    {
        //        LevelOfHand = LevelsOfHands.TwoPair;
        //    }
        //    if (card1NumOfPairs == 2 || card2NumOfPairs == 2)
        //    {
        //        LevelOfHand = LevelsOfHands.ThreeOfAKind;
        //    }
        //    if (hasStraight)
        //    {
        //        LevelOfHand = LevelsOfHands.Straight;
        //    }
        //    if (sumNumOfSameShape >= 5)
        //    {
        //        LevelOfHand = LevelsOfHands.Flush;
        //    }
        //    else
        //    {
        //        LevelOfHand = LevelsOfHands.HighCard;
        //    }
        ////}
        //public int[] GetSortedDistinctRanks(Card[] hand)
        //{
        //    return hand
        //        .Select(c => (int)c.Value)
        //        .Distinct()
        //        .OrderBy(value => value)
        //        .ToArray();
        //}


    }
}
