using TheLambClub.Models;
using static TheLambClub.Models.FBCard;

namespace TheLambClub.ModelsLogic
{
    public class SetOfCards:SetOfCardsModel
    {
        public SetOfCards()
        {
            cards = [];
            FillPakage();
        }
       
        public override void FillPakage()
         {
            foreach (Shapes shape in Enum.GetValues(typeof(Shapes)))
               for (int value = 1; value <= FBCard.CardsInShape; value++)
                    cards!.Add(new FBCard(((int)shape), value));
         }
        public override FBCard GetRandomCard()
        {
            FBCard card = cards![rnd.Next(cards.Count)];
            cards!.Remove(card);
            return card;
        }
        public override FBCard Add(FBCard fbcard)
        {
            cards!.Add(fbcard);
            return fbcard;
        }
    }
}
