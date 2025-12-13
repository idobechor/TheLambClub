using System.Windows.Input;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    public class PickYourMovePromptPageVM : ObservableObject
    {
        public event Action? RequestClose;
        private readonly Game game;
        public PickYourMovePromptPageVM(Game game)
        {
            this.game = game;
        }
        public ICommand Stay => new Command(StayFunction);
        
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
