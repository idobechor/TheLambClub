namespace TheLambClub.Models
{
    public abstract class SetOfCardsModel
    {
        #region fields

        protected List<FBCard>? cards;
        protected Random rnd = new();

        #endregion

        #region public methods

        public abstract FBCard GetRandomCard();
        public abstract void FillPakage();
        public abstract FBCard Add(FBCard card);

        #endregion
    }
}
