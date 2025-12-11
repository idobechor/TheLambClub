using TheLambClub.Models;
using static TheLambClub.Models.FBCard;
using static TheLambClub.Models.ViewCardModel;

namespace TheLambClub.ModelsLogic
{
    public class SetOfCards:SetOfCardsModel
    {
        public SetOfCards()
        {
            cards = [];
            usedCards = [];
            FillPakage();
        }
        protected override bool IsExist(FBCard currCard)
        {
            bool res = false;
            foreach (FBCard card in usedCards!)
            {
                if (currCard.Shape == card.Shape && currCard.Value == card.Value)
                {
                    res = true;
                }
            }
            return res;
        }
        protected override void FillPakage()
         {
           foreach (Shapes shape in Enum.GetValues(typeof(Shapes)))
               for (int value = 1; value <= ViewCard.CardsInShape; value++)
                    cards!.Add(new FBCard(((int)shape), value));
         }
        public override FBCard GetRandomCard()
        {
            FBCard card = null!;
            while (card == null)
                card = cards![rnd.Next(cards.Count)];
            usedCards!.Add(card);
            return card;

        }
        public override FBCard Add(FBCard fbcard)
        {
            cards!.Add(fbcard);
            return fbcard;
        }
    }
}
