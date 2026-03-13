using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;
using TheLambClub.Services;

namespace TheLambClub.ViewModel
{
    public partial class PickYourMovePromptPageVM : ObservableObject
    {
        #region fields

        private readonly Game game;
        private readonly Label TimeLeftLabel;
        private readonly IPokerSuggestionService? _suggestionService;

        #endregion

        #region events

        public event Action? RequestClose;

        #endregion

        #region commands

        public ICommand GetSuggestionCommand { get; }
        public ICommand SubmitBetCommand { get; private set; }

        #endregion

        #region properties

        private int BetAmountPrivate { get; set; }
        private string? _aiSuggestionText;
        private bool _isLoadingSuggestion;
        public string TimeLeft => game.TimeLeft;
        public TimerSettings TimerSettings => game.timerSettings;
        public string CheckOrFold => game.CheckOrCall;
        public string BetAmountStr => Strings.IntoruceYourBet + BetAmountPrivate;
        public int MinBet => game.MinBet == 0 ? 0 : game.MinBet;
        public int MaxBet => game.MaxBet;
        public int BetAmount
        {
            get => BetAmountPrivate;
            set
            {
                BetAmountPrivate = value;
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

        public PickYourMovePromptPageVM(Game game, Label label, IPokerSuggestionService? suggestionService = null)
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
            if (int.TryParse(TimeLeft, out int time))
                TimeLeftLabel.TextColor = time <= 10 ? Colors.Red : Colors.Black;
            else
                RequestClose?.Invoke();
            OnPropertyChanged(nameof(TimeLeft));
        }
        private void OnCheckOrCallChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(CheckOrFold));
            OnPropertyChanged(nameof(MinBet));
        }
        private bool CanSubmitBet(object arg) => BetAmount != 0 && game!.Players!.All(p => p.IsFolded || p.CurrentMoney > 0);
        private void BetFunction(object obj)
        {
            game.CurrentPlayer!.CurrentBet = BetAmountPrivate;
            game.BetFunction(obj);
            RequestClose?.Invoke();
        }
        private async void GetSuggestionAsync(object obj)
        {
            if (_suggestionService != null)
            {
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
                            AiSuggestionText = Strings.AiSuggestionTxt + (result.Suggestion ?? String.Empty).ToUpperInvariant();
                        else
                            AiSuggestionText = result.RawResponse ?? Strings.DefaultUnavailableMessage;
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

        #endregion
    }
}
