using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using System.Reflection;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;
using TheLambClub.Services;
using TheLambClub.Views;

namespace TheLambClub.ViewModel
{
    /// <summary>
    /// ViewModel for the Game Page, managing the state of the poker game, 
    /// UI updates, and interaction with the game logic and services.
    /// </summary>
    public partial class GamePageVM : ObservableObject
    {
        #region fields

        private readonly Game game;
        private readonly ModelsLogic.Connectivity _connectivity = new();
        private readonly List<Label> lstOponnentsLabels = [];
        private readonly List<Label> lstOponnentsMoneyLabels = [];

        #endregion

        #region commands

        /// <summary>
        /// Command to trigger the move selection popup.
        /// </summary>
        public Command ShowPickYourMovePrompt { get; }

        #endregion

        #region properties

        /// <summary>
        /// Gets the current amount of money in the pot, while triggering UI updates for related properties.
        /// </summary>
        public int PotMoney
        {
            get
            {
                OnPropertyChanged(nameof(lstOponnentsMoneyLabels));
                OnPropertyChanged(nameof(PlayerMoney));
                return game.Pot;
            }
        }

        public bool IsConnected => _connectivity.IsConnected;
        public int PlayerMoney => game != null && game.CurrentPlayer != null ? (int)game.CurrentPlayer.CurrentMoney : Keys.InitialMoney;
        public string Name => game.CurrentPlayer!.Name;
        public ViewCard Card1 => game.ViewCard1!;
        public ViewCard Card2 => game.ViewCard2!;
        public string Status => game.CurrentStatus;
        public ObservableCollection<ViewCard>? BoardCards => game.BoardViewCards;
        private bool IsMyTurnPrivate => game.IsMyTurn;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes the ViewModel, setting up event handlers for game state changes 
        /// and initializing the opponent display grid.
        /// </summary>
        /// <param name="game">The current game session.</param>
        /// <param name="grdOponnents">The UI grid to populate with opponent data.</param>
        public GamePageVM(Game game, Grid grdOponnents)
        {
            this.game = game;
            InitOponnentsGrid(grdOponnents);
            game.OnGameChanged += OnGameChanged;
            game.OnGameDeleted += OnGameDeleted;
            game.OnPlayerLost += OnPlayerLost;
            game.OnWinnerSelected += OnWinnerSelected;
            game.OnwinnerSelected += WinnerSelected;
            game.OnTurnChanged += OnTurnChanged;
            game.OnMyMoneyChanged += MoneyChanged;
            _connectivity.ConnectivityChanged += OnConnectivityChanged;
            ShowPickYourMovePrompt = new Command(ShowPickYourMovePromptFunction, IsMyTurn);
        }
        #endregion

        #region public methods

        /// <summary>
        /// Starts listening for real-time Firebase database updates.
        /// </summary>
        public void AddSnapshotListener() => game.AddSnapShotListener();

        /// <summary>
        /// Stops listening for real-time Firebase database updates.
        /// </summary>
        public void RemoveSnapshotListener() => game.RemoveSnapShotListener();

        #endregion

        #region private methods

        /// <summary>
        /// Handles updates to the players' money and refreshes the UI.
        /// </summary>
        private void MoneyChanged(object? sender, string winnerName)
        {
            if (lstOponnentsMoneyLabels != null && lstOponnentsMoneyLabels.Count + 1 == game.MaxNumOfPlayers && lstOponnentsMoneyLabels.Count != 0)
            {
                game.UpdateMoney(lstOponnentsLabels, lstOponnentsMoneyLabels, winnerName);
                OnPropertyChanged(nameof(lstOponnentsMoneyLabels));
            }
        }

        private void OnConnectivityChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsConnected));
        }

        private void OnTurnChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Status));
        }

        private void OnWinnerSelected(object? sender, EventArgs e)
        {
            Shell.Current.ShowPopupAsync(new WinGamePopup($"{Strings.Dear} {game.CurrentPlayer!.Name} {Strings.WinningMsg}"));
        }

        private void OnPlayerLost(object? sender, EventArgs e)
        {
            Shell.Current.ShowPopupAsync(new LostGamePopup($"{Strings.Dear} {game.CurrentPlayer!.Name} {Strings.LosingMsg}"));
        }

        /// <summary>
        /// Refreshes all bound UI properties when the game state changes.
        /// </summary>
        private void OnGameChanged(object? sender, EventArgs e)
        {
            DisplayOponnentsNames();
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(lstOponnentsMoneyLabels));
            OnPropertyChanged(nameof(PotMoney));
            OnPropertyChanged(nameof(PlayerMoney));
            OnPropertyChanged(nameof(IsMyTurnPrivate));
            OnPropertyChanged(nameof(BoardCards));
            OnPropertyChanged(nameof(Card1));
            OnPropertyChanged(nameof(Card2));
            ((Command)ShowPickYourMovePrompt)?.ChangeCanExecute();
        }

        private void WinnerSelected(object? sender, WinningPopupEvent winningEvent)
        {
            OnPropertyChanged(nameof(lstOponnentsMoneyLabels));
            Shell.Current.ShowPopupAsync(new WinningPopupPage(winningEvent.playersArray, winningEvent.ranks, winningEvent.numberOfWinners));
        }

        private bool IsMyTurn(object arg) => IsMyTurnPrivate;

        /// <summary>
        /// Opens the move selection popup and injects the poker suggestion service.
        /// </summary>
        private async void ShowPickYourMovePromptFunction(object obj)
        {
            IPokerSuggestionService suggestionService = Shell.Current?.Handler?.MauiContext?.Services?.GetService<IPokerSuggestionService>()!;
            await Shell.Current!.ShowPopupAsync(new PickYourMovePopupPage(game, suggestionService));
            ((Command)ShowPickYourMovePrompt).ChangeCanExecute();
        }

        /// <summary>
        /// Programmatically constructs the grid to display opponent names and current chip counts.
        /// </summary>
        private void InitOponnentsGrid(Grid grdOponnents)
        {
            int oponnentsCount = game.MaxNumOfPlayers - 1;
            grdOponnents.RowSpacing = 10;
            grdOponnents.ColumnSpacing = 10;
            grdOponnents.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grdOponnents.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            for (int i = 0; i < oponnentsCount; i++)
            {
                grdOponnents.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                Label lblName = new()
                {
                    Text = Strings.WaitingForPlayers,
                    TextColor = Colors.White,
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                lstOponnentsLabels.Add(lblName);
                grdOponnents.Add(lblName, i, 0);
                Label lblMoney = new()
                {
                    Text = Keys.InitialMoney.ToString(),
                    TextColor = Colors.White,
                    FontSize = 18,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0, 5, 0, 0)
                };
                lstOponnentsMoneyLabels.Add(lblMoney);
                grdOponnents.Add(lblMoney, i, 1);
            }
        }

        /// <summary>
        /// Handles the event when a game is deleted by navigating away and notifying the user.
        /// </summary>
        private void OnGameDeleted(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PopAsync();
                Toast.Make(Strings.GameDeleted, ToastDuration.Long).Show();
            });
        }

        private void DisplayOponnentsNames()
        {
            game.DisplayOponnentsNames(lstOponnentsLabels);
        }

        #endregion
    }
}