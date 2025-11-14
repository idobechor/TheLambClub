using Microsoft.Maui.Controls.Shapes;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    public class Card:CardModel
    {
        public Card()
        {
            Shape = (Shapes)rnd.Next(0, 5);
            Value = rnd.Next(2, 15);
        }
    }
}   
