using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public abstract class SetOfCardsModel
    {
        protected  List<Card> ? cards;
        protected Random rnd= new();
        public abstract Card GetRandomCard();
        protected abstract void FillPakage();
        public abstract Card Add(Card card);
    }
}
