using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System;
using System.Collections.ObjectModel;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;
using TheLambClub.Services;
using TheLambClub.Views;

namespace TheLambClub.ViewModel
{
    public partial class GamePageVM : ObservableObject
    {
        private readonly Game game;
        public int PotMoney
        {
            get
            {

                OnPropertyChanged(nameof(lstOponnentsMoneyLabels));
                OnPropertyChanged(nameof(PlayerMoney));
                return game.Pot != null ? (int)game.Pot.Sum() : Keys.InitialPotsMoney;
            }
        }
        public int PlayerMoney => game != null && game.CurrentPlayer != null ? (int)game.CurrentPlayer.CurrentMoney : Keys.InitialMoney; //{ get => game != null && game.CurrentPlayer != null ? (int)game.CurrentPlayer!.CurrentMoney : 10000; }
        public string Name => game.CurrentPlayer!.Name;
        public ViewCard Card1 => game.ViewCard1!;
        public ViewCard Card2 => game.ViewCard2!;

        public string Status => game.CurrentStatus;
        public Command ShowPickYourMovePrompt { get; }
        public ObservableCollection<ViewCard>? BoardCards => game.BoardViewCards;
        public string MyName => game.MyName;
        private readonly List<Label> lstOponnentsLabels = [];
        private readonly List<Label> lstOponnentsMoneyLabels = [];

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
            game.MoneyChanged += OnMoneyChanged;
            ShowPickYourMovePrompt = new Command(ShowPickYourMovePromptFunction, IsMyTurn);
        }


        private void OnMoneyChanged(object? sender, ChangingMoneyEvent e)
        {
            for (int i = 0; i < lstOponnentsMoneyLabels.Count; i++)
            {
                if (i < game.CurrentNumOfPlayers && lstOponnentsLabels[i].Text==e.Name)
                {
                    lstOponnentsMoneyLabels[i].Text =e.Money.ToString();
                }
            }
            OnPropertyChanged(nameof(lstOponnentsMoneyLabels));
        }

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
            OnPropertyChanged(nameof(_IsMyTurn));
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
        private bool _IsMyTurn => game.IsMyTurn;
        private bool IsMyTurn(object arg)
        {
            return _IsMyTurn;
        }
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
        //לתקן כללי כתיבת קוד
        private void DisplayOponnentsNames()
        {
            int lblIndex = 0;
            for (int i = 0; i < game.CurrentNumOfPlayers; i++)
            {
                if (game.Players![i] != null && game.CurrentPlayer!.Id != game.Players[i].Id)
                {
                    lstOponnentsLabels[lblIndex].Text = game.Players[i].Name;
                    lstOponnentsLabels[lblIndex++].BackgroundColor = Colors.Red;
                }
            }
        }
        public void AddSnapshotListener()
        {
            game.AddSnapShotListener();
        }
        public void RemoveSnapshotListener()
        {
            game.RemoveSnapShotListener();
        }
    }
}