using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    public class PickYourMovePromptPageVM : ObservableObject
    {
        public event Action? RequestClose;
        private readonly Game game;
        private readonly Label TimeLeftLabel ;
        private int timeInt;

        public string TimeLeft => game.TimeLeft;
        public TimerSettings timerSettings  => game.timerSettings;

        private void OnTimeLeftChanged(object? sender, EventArgs e)
        {
            if (int.TryParse(TimeLeft, out timeInt))
            {
                if (timeInt <= 10)
                {
                    TimeLeftLabel.TextColor = Colors.Red;
                }
                else
                {
                    TimeLeftLabel.TextColor = Colors.Black;
                }
            }
            OnPropertyChanged(nameof(TimeLeft));
        }
        private int _betAmount { get; set; }
        public string CheckOrFold=>game.CheckOrCall;
        public string BetAmountStr =>Strings.IntoruceYourBet+_betAmount;
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

        public PickYourMovePromptPageVM(Game game,Label label)
        {
            TimeLeftLabel = label;
            this.game = game;
            game.OnCheckOrCallChanged+= OnCheckOrCallChanged;
            game.TimeLeftChanged += OnTimeLeftChanged;
            WeakReferenceMessenger.Default.Send(new AppMessage<TimerSettings>(timerSettings));
        }

        public void Close()
        {
            game.TimeLeftChanged -= OnTimeLeftChanged;
            WeakReferenceMessenger.Default.Send(new AppMessage<bool>(true));
        }
        
        private void OnCheckOrCallChanged(object? sender, EventArgs e)
        {
           OnPropertyChanged(nameof(CheckOrFold));
        }

        public ICommand Stay => new Command(StayFunction);

        public ICommand SubmitBetCommand => new Command(BetFunction);

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
