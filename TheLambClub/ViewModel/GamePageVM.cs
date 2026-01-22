using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System;
using System.Collections.ObjectModel;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;
using TheLambClub.Views;

namespace TheLambClub.ViewModel
{
    class GamePageVM : ObservableObject
    {
        private readonly Game game;
        public int Money => game != null &&  game.CurrentPlayer != null ? (int)game.CurrentPlayer.CurrentMoney:10000; //{ get => game != null && game.CurrentPlayer != null ? (int)game.CurrentPlayer!.CurrentMoney : 10000; }
        private readonly PickYourMovePromptPageVM PYMPtPageVM = new();
        public string Name => game.CurrentPlayer!.Name;
        public ViewCard Card1 => game.ViewCard1!;

        public ViewCard Card2 => game.ViewCard2!;

        public string Status => game.CurrentStatus;
        public Command ShowPickYourMovePrompt { get; }     
        public ObservableCollection<ViewCard>? BoardCards => game.BoardViewCards;
       public string MyName=> game.MyName;
        public string CurrentStatus => game.CurrentStatus;      
        private readonly List<Label> lstOponnentsLabels = [];
        private readonly List<Label> lstOponnentsMoneyLabels = [];
        private bool _isPopupOpen=>game.IsPopupOpen;
        public GamePageVM(Game game, Grid grdOponnents)
        {
            this.game = game;
            InitOponnentsGrid(grdOponnents);
            game.OnGameChanged += OnGameChanged;
            game.OnGameDeleted += OnGameDeleted;
            game.OnMyMoneyChanged += OnMyMoneyChanged;
            ShowPickYourMovePrompt = new Command(ShowPickYourMovePromptFunction, IsMyTurn);
        }

        private void OnMyMoneyChanged(object? sender, EventArgs e)
        {
            
            //if(game.CurrentPlayerIndex>0)
            //   lstOponnentsMoneyLabels[game.CurrentPlayerIndex-1].Text = String .Empty+ game.CurrentPlayer!.CurrentMoney;
            //else          
            //    OnPropertyChanged(nameof(Money));
            //OnPropertyChanged(nameof(lstOponnentsMoneyLabels));

        }

        private void OnGameChanged(object? sender, EventArgs e)
        {
            DisplayOponnentsNames();
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(_IsMyTurn));
            OnPropertyChanged(nameof(BoardCards));
            OnPropertyChanged(nameof(Card1));
            OnPropertyChanged(nameof(Card2));
            OnPropertyChanged(nameof(PYMPtPageVM.MinBet));
            game.OnwinnerSelected += WinnerSelected;
            ((Command)ShowPickYourMovePrompt)?.ChangeCanExecute();
        } 
        private void WinnerSelected(object? sender, WinningPopupEvent winningEvent)
        {
            if (!_isPopupOpen)
            {
                Shell.Current.ShowPopupAsync(new WinningPopupPage(winningEvent.playersArray, winningEvent.ranks));
                game.IsPopupOpen = true;
            }         
        }
        private bool _IsMyTurn => game.IsMyTurn;
        private bool IsMyTurn(object arg)
        {
            return _IsMyTurn;
        }
        private async void ShowPickYourMovePromptFunction(object obj)
        {
            await Shell.Current.ShowPopupAsync(new PickYourMovePopupPage(game));
            ((Command)ShowPickYourMovePrompt).ChangeCanExecute();
        }
        private void InitOponnentsGrid(Grid grdOponnents)
        {
            int oponnentsCount = game.MaxNumOfPlayers - 1;
            grdOponnents.RowDefinitions.Clear();
            grdOponnents.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grdOponnents.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            for (int i = 0; i < oponnentsCount; i++)
            {
                grdOponnents.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                Label lbl = new()
                {
                    Text = Strings.WaitingForPlayers,
                    TextColor = Colors.Black,
                    FontSize = 10,
                    Margin = new Thickness(5),
                    Padding = new Thickness(2),
                    HorizontalTextAlignment = TextAlignment.Center,
                };
                lstOponnentsLabels.Add(lbl);             
                grdOponnents.Add(lbl, i, 0);
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
