namespace TheLambClub.Models
{
    /// <summary>
    /// Represents a playing card model designed for Firebase integration or data handling.
    /// </summary>
    public class FBCard
    {
        #region fields

        /// <summary>
        /// Represents the total number of cards in a single suit/shape.
        /// </summary>
        public const int CardsInShape = 13;

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the numerical value of the card.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Defines the possible suits or shapes of a card.
        /// </summary>
        public enum Shapes { Club, Diamond, Heart, Spade };

        /// <summary>
        /// Gets or sets the suit/shape of the card.
        /// </summary>
        public Shapes Shape { get; set; }

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FBCard"/> class with a specific shape and value.
        /// </summary>
        /// <param name="shape">The integer representation of the card's shape.</param>
        /// <param name="value">The numerical value of the card.</param>
        public FBCard(int shape, int value)
        {
            Shape = (Shapes)shape;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FBCard"/> class with default values.
        /// </summary>
        public FBCard()
        {
        }

        #endregion
    }
}