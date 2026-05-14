using TheLambClub.Models;
using static TheLambClub.Models.FBCard;

namespace TheLambClub.ModelsLogic
{
    /// <summary>
    /// Represents a deck or a set of playing cards, providing functionality to 
    /// initialize, manage, and draw cards randomly.
    /// </summary>
    public class SetOfCards : SetOfCardsModel
    {
        #region constructors

        /// <summary>
        /// Initializes a new instance of the SetOfCards class and fills it with a full deck.
        /// </summary>
        public SetOfCards()
        {
            Cards = [];
            FillPakage();
        }

        #endregion

        #region public methods

        /// <summary>
        /// Populates the card collection with a standard set of cards based on available shapes and values.
        /// Clears any existing cards before filling.
        /// </summary>
        public override void FillPakage()
        {
            Cards!.Clear();
            foreach (Shapes shape in Enum.GetValues(typeof(Shapes)))
                for (int value = 1; value <= FBCard.CardsInShape; value++)
                    Cards!.Add(new FBCard(((int)shape), value));
        }

        /// <summary>
        /// Selects a random card from the current set, removes it from the collection, and returns it.
        /// </summary>
        /// <returns>A random FBCard object from the set.</returns>
        public override FBCard GetRandomCard()
        {
            FBCard card = Cards![rnd.Next(Cards.Count)];
            Cards!.Remove(card);
            return card;
        }

        /// <summary>
        /// Adds a specific card back into the set.
        /// </summary>
        /// <param name="fbcard">The card to be added.</param>
        /// <returns>The card that was added.</returns>
        public override FBCard Add(FBCard fbcard)
        {
            Cards!.Add(fbcard);
            return fbcard;
        }

        #endregion
    }
}