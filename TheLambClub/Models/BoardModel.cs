using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public class BoardModel
    {
        public Card[] Cards = new Card[5];
        public int CurrentCardIndex = 0;
    }
}
