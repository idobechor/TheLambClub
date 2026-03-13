using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;
using TheLambClub.Services;

namespace TheLambClub.ViewModel
{
    public partial class PickYourMovePopupPageVM : ObservableObject
    {
        #region fields

        private readonly Game game;
        private readonly Label TimeLeftLabel;
        private readonly IPokerSuggestionService? _suggestionService;
        private int timeInt;

        #endregion

        #region events

        public event Action? RequestClose;

        #endregion

        #region commands

        public ICommand GetSuggestionCommand { get; }
        public ICommand Stay => new Command(StayFunction);
        public ICommand SubmitBetCommand { get; private set; }
        public ICommand Fold => new Command(FoldFunction);

        #endregion

        #region properties

        private int _BetAmount { get; set; }
        private string? _aiSuggestionText;
        private bool _isLoadingSuggestion;
        public string TimeLeft => game.TimeLeft;
        public TimerSettings TimerSettings => game.timerSettings;
        public string CheckOrFold => game.CheckOrCall;
        public string BetAmountStr => Strings.IntoruceYourBet + _BetAmount;
        public int MinBet => game.MinBet == 0 ? 0 : game.MinBet;
        public int MaxBet => game.MaxBet;
        public int BetAmount
        {
            get => _BetAmount;
            set
            {
                _BetAmount = value;
                OnPropertyChanged(nameof(BetAmountStr));
                ((Command)SubmitBetCommand)?.ChangeCanExecute();
            }
        }
        public string? AiSuggestionText
        {
            get => _aiSuggestionText;
            private set { _aiSuggestionText = value; OnPropertyChanged(); OnPropertyChanged(nameof(AiSuggestionVisible)); }
        }
        public bool AiSuggestionVisible => !string.IsNullOrEmpty(_aiSuggestionText);
        public bool IsLoadingSuggestion
        {
            get => _isLoadingSuggestion;
            private set { _isLoadingSuggestion = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanRequestSuggestion)); }
        }
        public bool CanRequestSuggestion => _suggestionService != null && !_isLoadingSuggestion;

        #endregion

        #region constructors

        public PickYourMovePopupPageVM(Game game, Label label, IPokerSuggestionService? suggestionService = null)
        {
            GetSuggestionCommand = new Command(GetSuggestionAsync, _ => CanRequestSuggestion);
            SubmitBetCommand = new Command(BetFunction, CanSubmitBet);
            TimeLeftLabel = label;
            _suggestionService = suggestionService;
            this.game = game;
            game.OnCheckOrCallChanged += OnCheckOrCallChanged;
            game.TimeLeftChanged += OnTimeLeftChanged;
            WeakReferenceMessenger.Default.Send(new AppMessage<TimerSettings>(TimerSettings));
        }

        #endregion

        #region public methods

        public void Close()
        {
            game.TimeLeftChanged -= OnTimeLeftChanged;
            WeakReferenceMessenger.Default.Send(new AppMessage<bool>(true));
        }

        #endregion

        #region private methods

        private void OnTimeLeftChanged(object? sender, EventArgs e)
        {
            if (int.TryParse(TimeLeft, out timeInt))
            {
                if (timeInt <= 10)
                    TimeLeftLabel.TextColor = Colors.Red;
                else
                    TimeLeftLabel.TextColor = Colors.Black;
            }
            else
                RequestClose?.Invoke();
            OnPropertyChanged(nameof(TimeLeft));
        }
        private void OnCheckOrCallChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(CheckOrFold));
            OnPropertyChanged(nameof(MinBet));
        }
        private bool CanSubmitBet(object arg)
        {
            return BetAmount != 0 && game!.Players!.All(p => p.IsFolded || p.CurrentMoney > 0);
        }
        private void StayFunction(object obj)
        {
            game.CallFunction();
            RequestClose?.Invoke();
        }
        private void BetFunction(object obj)
        {
            game.CurrentPlayer!.CurrentBet = _BetAmount;
            game.BetFunction(obj);
            RequestClose?.Invoke();
        }
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
                PokerSuggestionResult result = await _suggestionService.GetSuggestionAsync(
                    game.CurrentPlayer?.FBCard1!,
                    game.CurrentPlayer?.FBCard2!,
                    [.. game.BoardCards]);
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    if (result.Success)
                        AiSuggestionText = Strings.AiSuggestsPrefix + (result.Suggestion ?? string.Empty).ToUpperInvariant();
                    else
                        AiSuggestionText = result.RawResponse ?? Strings.SuggestionUnavailable;
                });
            }
            finally
            {
                IsLoadingSuggestion = false;
                OnPropertyChanged(nameof(CanRequestSuggestion));
                (GetSuggestionCommand as Command)?.ChangeCanExecute();
            }
        }

        #endregion
    }
}
