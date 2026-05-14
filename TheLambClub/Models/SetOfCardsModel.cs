namespace TheLambClub.Models
{
    /// <summary>
    /// Represents an abstract base model for a deck or a collection of playing cards, 
    /// providing methods for card management and randomized selection.
    /// </summary>
    public abstract class SetOfCardsModel
    {
        #region fields

        /// <summary>
        /// The list containing the card objects currently in the set.
        /// </summary>
        protected List<FBCard>? Cards { get; set; }
        /// <summary>
        /// An instance of the <see cref="Random"/> class used for shuffling or selecting random cards.
        /// </summary>
        protected Random rnd = new();
        #endregion

        #region public methods

        /// <summary>
        /// Selects and returns a random card from the set, typically removing it from the collection.
        /// </summary>
        /// <returns>A randomly selected <see cref="FBCard"/>.</returns>
        public abstract FBCard GetRandomCard();

        /// <summary>
        /// Populates the set with a full package of cards (e.g., a standard 52-card deck).
        /// </summary>
        public abstract void FillPakage();
        /// <summary>
        /// Adds a specific card to the current set.
        /// </summary>
        /// <param name="card">The card to be added to the collection.</param>
        /// <returns>The <see cref="FBCard"/> that was added.</returns>
        public abstract FBCard Add(FBCard card);
        #endregion
    }
}