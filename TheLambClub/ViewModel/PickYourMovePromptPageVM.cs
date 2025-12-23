using System.Windows.Input;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    public class PickYourMovePromptPageVM : ObservableObject
    {
        public event Action? RequestClose;
        private readonly Game game;
        private int _betAmount { get; set; }
        public string CheckOrFold=>game.CheckOrCall;
        public string BetAmountStr =>$"your bet amount is:{_betAmount}";
        public int BetAmount
        {
            get => _betAmount;
            set
            {
                if (_betAmount != value)
                {
                    _betAmount = value;
                }
                game.CurrentPlayer?.CurrentBet = _betAmount;
                OnPropertyChanged(nameof(BetAmountStr));
            }
        }
        public PickYourMovePromptPageVM(Game game)
        {
            this.game = game;
            game.OnCheckOrCallChanged+= POnCheckOrCallChanged;
        }

        private void POnCheckOrCallChanged(object? sender, EventArgs e)
        {
           OnPropertyChanged(nameof(CheckOrFold));
        }

        public ICommand Stay => new Command(StayFunction);

   

        public ICommand SubmitBetCommand => new Command(BetFunction);
        //public ICommand SubmitBetCommand => new Command(UpDateBet);

        private void StayFunction(object obj)
        {
            game.CallFunction();
            RequestClose?.Invoke();
        }

        private void BetFunction(object obj)
        {
            game.BetFunction(obj);
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
