using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    public class HandRank: HandRankModel
    {
        public override int Compare(HandRank other)
        {
            int res=0;
            if (other != null)
            {
                int myType = (int)HandType;
                int otherType = (int)other.HandType;
                if (myType != otherType)
                {
                    res = myType - otherType;
                }
                else if (PrimaryValue != other.PrimaryValue)
                {
                    res = PrimaryValue - other.PrimaryValue;
                }
                else if (SecondaryValue != other.SecondaryValue)
                {
                    res = SecondaryValue - other.SecondaryValue;
                }
                else
                {
                    int minLength = Math.Min(Kickers.Length, other.Kickers.Length);
                    for (int i = 0; i < minLength; i++)
                    {
                        if (Kickers[i] != other.Kickers[i])
                        {
                            res= Kickers[i] - other.Kickers[i];
                        }
                    }
                }
            }           
            return res; 
        }
        public override bool IsBetter(HandRank other)
        {
            return Compare(other) > 0;
        }
        protected override bool IsEqual(HandRank other)
        {
            return Compare(other) == 0;
        }
        public override string ToString()
        {
            return Strings.HisRankIs+HandType.ToString();
        }
    }
}
