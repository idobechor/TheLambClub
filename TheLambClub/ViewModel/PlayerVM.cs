using System.Windows.Input;
using TheLambClub.ModelsLogic;
using TheLambClub.Models;
namespace TheLambClub.ViewModel
{
    public class PlayerVM
    {
        public Player player;
        public PlayerVM(Player player)
        {
            this.player = player;
        }
        public ImageSource? card1
        {
            get => player.Card1.Source;
        }
        public ImageSource? card2
        {
            get => player.Card2.Source;
        }
        public string Name  => player.Name;

    }
}
