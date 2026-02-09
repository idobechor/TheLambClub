using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;
using TheLambClub.Services;

namespace TheLambClub.ViewModel
{
    public class PickYourMovePromptPageVM : ObservableObject
    {
        public event Action? RequestClose;
        private readonly Game game;
        private readonly Label TimeLeftLabel;
        private readonly IPokerSuggestionService? _suggestionService;
        private int timeInt;
        public string TimeLeft => game.TimeLeft;
        public TimerSettings TimerSettings => game.timerSettings;

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
            else
            {
                RequestClose?.Invoke();
            }
            OnPropertyChanged(nameof(TimeLeft));
        }
        private int _betAmount { get; set; }
        public string CheckOrFold => game.CheckOrCall;
        public string BetAmountStr => Strings.IntoruceYourBet + _betAmount;
        public int MinBet => game.MinBet == 0 ? 0 : game.MinBet - 1;
        public int MaxBet => game.MaxBet;
        public int BetAmount
        {
            get => _betAmount;
            set
            {
                if (_betAmount != value)
                    _betAmount = value;              
                OnPropertyChanged(nameof(BetAmountStr));
                ((Command)SubmitBetCommand)?.ChangeCanExecute();
            }
        }

        private string? _aiSuggestionText;
        public string? AiSuggestionText
        {
            get => _aiSuggestionText;
            private set { _aiSuggestionText = value; OnPropertyChanged(); OnPropertyChanged(nameof(AiSuggestionVisible)); }
        }

        public bool AiSuggestionVisible => !string.IsNullOrEmpty(_aiSuggestionText);

        private bool _isLoadingSuggestion;
        public bool IsLoadingSuggestion
        {
            get => _isLoadingSuggestion;
            private set { _isLoadingSuggestion = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanRequestSuggestion)); }
        }

        public bool CanRequestSuggestion => _suggestionService != null && !_isLoadingSuggestion;

        public ICommand GetSuggestionCommand { get; }

        public PickYourMovePromptPageVM(Game game, Label label, IPokerSuggestionService? suggestionService = null)
        {
            GetSuggestionCommand = new Command(GetSuggestionAsync, _ => CanRequestSuggestion);
            SubmitBetCommand = new Command(BetFunction, CanSubmitBet);
            TimeLeftLabel = label;
            _suggestionService = suggestionService;
            this.game = game;
            game.OnCheckOrCallChanged+= OnCheckOrCallChanged;
            game.TimeLeftChanged += OnTimeLeftChanged;
            WeakReferenceMessenger.Default.Send(new AppMessage<TimerSettings>(TimerSettings));
        }

        public void Close()
        {
            game.TimeLeftChanged -= OnTimeLeftChanged;
            WeakReferenceMessenger.Default.Send(new AppMessage<bool>(true));
            //game.IsHappened= false;
        }
        
        private void OnCheckOrCallChanged(object? sender, EventArgs e)
        {
           OnPropertyChanged(nameof(CheckOrFold));
            OnPropertyChanged(nameof(MinBet));
        }

        public ICommand Stay => new Command(StayFunction);

        public ICommand SubmitBetCommand { get; private set; }

        private bool CanSubmitBet(object arg)
        {
            return BetAmount!=0&&game!.Players!.All(p => p.IsFolded || p.CurrentMoney > 0);
        }

        private void StayFunction(object obj)
        {
            game.CallFunction();
            RequestClose?.Invoke();
        }

        private void BetFunction(object obj)
        {
            if (_betAmount != MinBet)
                game.CurrentPlayer!.CurrentBet = _betAmount;
            game.BetFunction(obj);
            RequestClose?.Invoke();
        }
        public ICommand Fold => new Command(FoldFunction);

        private void FoldFunction(object obj)
        {
            game.PickedFold();
            RequestClose?.Invoke();
        }

        private async void GetSuggestionAsync(object obj)
        {
            if (_suggestionService == null) return;
            IsLoadingSuggestion = true;
            AiSuggestionText = null;
            OnPropertyChanged(nameof(CanRequestSuggestion));
            (GetSuggestionCommand as Command)?.ChangeCanExecute();

            try
            {
                var result = await _suggestionService.GetSuggestionAsync(
                    game.CurrentPlayer?.FBCard1!,
                    game.CurrentPlayer?.FBCard2!,
                    new List<FBCard>(game.BoardCards));

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    if (result.Success)
                        AiSuggestionText = "AI suggests: " + (result.Suggestion ?? "").ToUpperInvariant();
                    else
                        AiSuggestionText = result.RawResponse ?? "Suggestion unavailable.";
                });
            }
            finally
            {
                IsLoadingSuggestion = false;
                OnPropertyChanged(nameof(CanRequestSuggestion));
                (GetSuggestionCommand as Command)?.ChangeCanExecute();
            }
        }
    }
}
