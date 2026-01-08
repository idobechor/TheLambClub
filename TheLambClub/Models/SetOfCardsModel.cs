
namespace TheLambClub.Models
{
    public abstract class SetOfCardsModel
    {
        protected  List<FBCard> ? cards;
        protected  List<FBCard> ? usedCards;
        protected Random rnd= new();
        public abstract FBCard GetRandomCard();
        protected abstract void FillPakage();
        public abstract FBCard Add(FBCard card);
        protected abstract bool IsExist(FBCard currCard);
    }
}
