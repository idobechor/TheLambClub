using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    public class SetOfCards:SetOfCardsModel
    {
        public SetOfCards()
        {
            cards = [];
            FillPakage();
        }
         protected override void FillPakage()
         {
           foreach (CardModel.Shapes shape in Enum.GetValues(typeof(CardModel.Shapes)))
               for (int value = 1; value <= Card.CardsInShape; value++)
                  cards!.Add(new Card(shape, value));
         }
        public override Card GetRandomCard()
        {
            return cards![rnd.Next(cards.Count)];
        }
        public override Card Add(Card card)
        {
            cards!.Add(card);
            return card;
        }
    }
}
