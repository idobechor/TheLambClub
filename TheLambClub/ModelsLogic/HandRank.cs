using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    public class HandRank: HandRankModel
    {
        public override int Compare(HandRank other)
        {
            if (other == null) return 1;
            int myType = (int)HandType;
            int otherType = (int)other.HandType;
            if (myType != otherType)
            {
                return myType - otherType;
            }
            if (PrimaryValue != other.PrimaryValue)
            {
                return PrimaryValue - other.PrimaryValue;
            }
            if (SecondaryValue != other.SecondaryValue)
            {
                return SecondaryValue - other.SecondaryValue;
            }
            int minLength = Math.Min(Kickers.Length, other.Kickers.Length);
            for (int i = 0; i < minLength; i++)
            {
                if (Kickers[i] != other.Kickers[i])
                {
                    return Kickers[i] - other.Kickers[i];
                }
            }
            //אם הם באמת שווים
            return 0;
        }
        protected override bool IsBetter(HandRank other)
        {
            return Compare(other) > 0;
        }
        protected override bool IsEqual(HandRank other)
        {
            return Compare(other) == 0;
        }
    }
}
