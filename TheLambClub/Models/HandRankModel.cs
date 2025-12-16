using TheLambClub.Models;
using TheLambClub.ModelsLogic;
using static TheLambClub.Models.PlayerModel;

namespace TheLambClub.Models
{
    public abstract class HandRankModel
    {
     
        public HandRankModel()
        {
        }   
        public LevelsOfHands HandType { get; set; }
        public int PrimaryValue { get; set; }
        public int SecondaryValue { get; set; }        
        public int[] Kickers { get; set; } = Array.Empty<int>();//כרגע זה הפיתרון היחידי שמצאתי אם אפשר תתקן        
        public FBCard[] HandCards { get; set; } = Array.Empty<FBCard>();//כרגע זה הפיתרון היחידי שמצאתי אם אפשר תתקן
        public abstract int Compare(HandRank other);
        protected abstract bool IsBetter(HandRank other);
        protected abstract bool IsEqual(HandRank other);


    }
}

