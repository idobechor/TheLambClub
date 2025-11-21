using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public abstract class BoardModel
    {
        private readonly SetOfCards setOfCards = new();
        public Card[] Cards = new Card[5];
        public int CurrentCardIndex = 3;
        public BoardModel() 
        {
            Cards[0]= new SetOfCards().GetRandomCard();
            Cards[1]= new SetOfCards().GetRandomCard();
            Cards[2]= new SetOfCards().GetRandomCard();
            Cards[3]= new SetOfCards().GetRandomCard();
            Cards[4]= new SetOfCards().GetRandomCard();
        }
    }
}
