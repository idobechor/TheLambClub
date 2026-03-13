namespace TheLambClub.Models
{
    public class FBCard
    {
        #region fields

        public const int CardsInShape = 13;

        #endregion

        #region properties

        public int Value { get; set; }
        public enum Shapes { Club, Diamond, Heart, Spade };
        public Shapes Shape { get; set; }

        #endregion

        #region constructors

        public FBCard(int shape, int value)
        {
            Shape = (Shapes)shape;
            Value = value;
        }
        public FBCard()
        {
        }

        #endregion
    }
}
