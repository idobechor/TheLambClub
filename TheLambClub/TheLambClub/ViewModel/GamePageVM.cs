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
    public partial class GamePageVM : ObservableObject
    {
        #region fields

        private readonly Game game;
        private readonly List<Label> lstOponnentsLabels = [];
        private readonly List<Label> lstOponnentsMoneyLabels = [];

        #endregion

        #region commands

        public Command ShowPickYourMovePrompt { get; }

        #endregion

        #region properties

        public int PotMoney
        {
            get
            {
                OnPropertyChanged(nameof(lstOponnentsMoneyLabels));
                OnPropertyChanged(nameof(PlayerMoney));
                return game.Pot;
            }
        }
        public int PlayerMoney => game != null && game.CurrentPlayer != null ? (int)game.CurrentPlayer.CurrentMoney : Keys.InitialMoney;
        public string Name => game.CurrentPlayer!.Name;
        public ViewCard Card1 => game.ViewCard1!;
        public ViewCard Card2 => game.ViewCard2!;
        public string Status => game.CurrentStatus;
        public ObservableCollection<ViewCard>? BoardCards => game.BoardViewCards;
        public string MyName => game.MyName;
        private bool IsMyTurnPrivate => game.IsMyTurn;

        #endregion

        #region constructors

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
            game.OnMyMoneyChanged+= MoneyChanged;
            ShowPickYourMovePrompt = new Command(ShowPickYourMovePromptFunction, IsMyTurn);
        }

        private void MoneyChanged(object? sender, string winnerName)
        {
            if (lstOponnentsMoneyLabels != null && lstOponnentsMoneyLabels.Count + 1 == game.MaxNumOfPlayers && lstOponnentsMoneyLabels.Count != 0)
            {
                game.UpdateMoney(lstOponnentsLabels, lstOponnentsMoneyLabels, winnerName);
                OnPropertyChanged(nameof(lstOponnentsMoneyLabels));
            }
        }


        #endregion

        #region public methods

        public void AddSnapshotListener() => game.AddSnapShotListener();
        public void RemoveSnapshotListener() => game.RemoveSnapShotListener();

        #endregion

        #region private methods
        private void OnTurnChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Status));
        }
        private void OnWinnerSelected(object? sender, EventArgs e)
        {

            Shell.Current.ShowPopupAsync(new WinGamePopup(Strings.Dear+game.CurrentPlayer!.Name+Strings.WinningMsg));
        }
        private void OnPlayerLost(object? sender, EventArgs e)
        {
            Shell.Current.ShowPopupAsync(new LostGamePopup(Strings.Dear + game.CurrentPlayer!.Name + Strings.LosingMsg));
        }
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
        private async void ShowPickYourMovePromptFunction(object obj)
        {
            IPokerSuggestionService suggestionService = Shell.Current?.Handler?.MauiContext?.Services?.GetService<IPokerSuggestionService>()!;
            await Shell.Current!.ShowPopupAsync(new PickYourMovePopupPage(game, suggestionService));
            ((Command)ShowPickYourMovePrompt).ChangeCanExecute();
        }
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
