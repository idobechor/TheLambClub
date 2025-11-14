using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    public class Board: BoardModel
    {
        public Board() 
        {
            Cards[CurrentCardIndex]= new Card();
            CurrentCardIndex++;
            Cards[CurrentCardIndex]= new Card();
            CurrentCardIndex++;
            Cards[CurrentCardIndex] = new Card();
            CurrentCardIndex++;
        }
    }
}
