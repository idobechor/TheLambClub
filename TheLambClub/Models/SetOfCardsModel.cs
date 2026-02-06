
namespace TheLambClub.Models
{
    public abstract class SetOfCardsModel
    {
        protected  List<FBCard> ? cards;
        protected Random rnd= new();
        public abstract FBCard GetRandomCard();
        public abstract void FillPakage();
        public abstract FBCard Add(FBCard card);
    }
}
