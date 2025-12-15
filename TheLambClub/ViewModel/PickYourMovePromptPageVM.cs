using System.Windows.Input;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    public class PickYourMovePromptPageVM : ObservableObject
    {
        public event Action? RequestClose;
        private readonly Game game;
        private readonly Player player;
        public PickYourMovePromptPageVM(Game game)
        {
            this.game = game;
        }
        public ICommand Stay => new Command(StayFunction);
        //public ICommand SubmitBetCommand => new Command(UpDateBet);
        public double BetAmount{ get; set; }

        private void StayFunction(object obj)
        {
            game.NextTurn();
            RequestClose?.Invoke();
        }
        public ICommand Fold => new Command(FoldFunction);

        private void FoldFunction(object obj)
        {
            game.PickedFold();
            RequestClose?.Invoke();
        }
    }
}
