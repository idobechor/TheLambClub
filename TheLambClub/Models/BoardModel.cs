using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public abstract class BoardModel
    {
        public Card[] Cards = new Card[5];
        public BoardModel(FBCard[]BoardCards) 
        {

        }
    }
}
