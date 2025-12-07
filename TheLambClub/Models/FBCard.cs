using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLambClub.Models
{
    public class FBCard
    {
        public int Value { get; set; }
        public enum Shapes { Club, Diamond, Heart, Spade };
        public Shapes Shape { get; set; }
        public FBCard(int shape, int value)
        {
            Shape = (Shapes)shape;
            Value = value;
        }
        public FBCard()
        {
        }
    }
}
