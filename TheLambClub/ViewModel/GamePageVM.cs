using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;
using TheLambClub.Views;

namespace TheLambClub.ViewModel
{
    class GamePageVM : ObservableObject
    {
        private readonly Game game;  
        public string TimeLeft => game.TimeLeft;
        private void OnTimeLeftChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(TimeLeft));
        }
        public Command ShowPickYourMovePrompt { get; }
        public ObservableCollection<ViewCard> BoardCards
        {
            get
            {
                return [.. game.BoardCards.Select(c =>
                {
                    if (c == null)
                        return new ViewCard();
                    return new ViewCard(c);
                })];
                
            }            
        }
        public string MyName=> game.MyName;
        //public ICommand Stay => new Command(StayFunction);

        //private void StayFunction(object obj)
        //{
        //    game.NextTurn();
        //}
        //public ICommand Fold => new Command(FoldFunction);

        //private void FoldFunction(object obj)
        //{
        //    game.PickedFold();
        //}

        public string CurrentStatus => game.CurrentStatus;      
        private readonly List<Label> lstOponnentsLabels = [];        
        //private void NextTurn(object obj)
        //{
        //    game.NextTurn();
        //    OnPropertyChanged(nameof(CurrentStatus));
        //}
        private void OnGameChanged(object? sender, EventArgs e)
        {
            DisplayOponnentsNames();
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(_IsMyTurn));
            OnPropertyChanged(nameof(BoardCards));
            OnPropertyChanged(nameof(Card1));
            OnPropertyChanged(nameof(Card2));
            ((Command)ShowPickYourMovePrompt).ChangeCanExecute();
        }
        public GamePageVM(Game game, Grid grdOponnents)
        {
           this.game=game;
            InitOponnentsGrid(grdOponnents);
            game.OnGameChanged += OnGameChanged;
            game.OnGameDeleted += OnGameDeleted;
            game.TimeLeftChanged += OnTimeLeftChanged;
            ShowPickYourMovePrompt = new Command(ShowPickYourMovePromptFunction, IsMyTurn);
        }
        public GamePageVM()
        {                       
        }
        private bool _IsMyTurn=> game.IsMyTurn;
        private bool IsMyTurn(object arg)
        {
            return _IsMyTurn;
        }

        private async void ShowPickYourMovePromptFunction(object obj)
        {
            await Shell.Current.ShowPopupAsync(new PickYourMovePopupPage(game));
            Console.WriteLine("check can excute can? " + ((Command)ShowPickYourMovePrompt).CanExecute(null));
            ((Command)ShowPickYourMovePrompt).ChangeCanExecute();
        }

        private void InitOponnentsGrid(Grid grdOponnents)
        {
            int oponnentsCount = game.MaxNumOfPlayers - 1;
            grdOponnents.RowDefinitions.Clear();
            grdOponnents.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // label
            grdOponnents.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // images row

            for (int i = 0; i < oponnentsCount; i++)
            {
                grdOponnents.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                Label lbl = new()
                {
                    Text = "Waiting",
                    TextColor = Colors.Black,
                    FontSize = 16,
                    Margin = new Thickness(5),
                    Padding = new Thickness(2),
                    HorizontalTextAlignment = TextAlignment.Center,
                };
                lstOponnentsLabels.Add(lbl);
                Image img1 = new Image
                {
                    Source = "backofcard.jpg",
                    HeightRequest = 40,
                    WidthRequest = 40,
                    HorizontalOptions = LayoutOptions.Center
                };

                Image img2 = new Image
                {
                    Source = "backofcard.jpg",
                    HeightRequest = 40,
                    WidthRequest = 40,
                    HorizontalOptions = LayoutOptions.Center
                };
                StackLayout imagesRow = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                    Spacing = 3,
                    Children = { img1, img2 }
                };
                grdOponnents.Add(lbl, i, 0);
                grdOponnents.Add(imagesRow, i, 1);
            }
        }


        private void OnGameDeleted(object? sender, EventArgs e)
        {
        }

        private void DisplayOponnentsNames()
        {
            int lblIndex = 0;
            for (int i = 0; i < game.CurrentNumOfPlayers; i++)
            {
                if (game.Players![i] != null && game.CurrentPlayer!.Id == game.Players?[i].Id)
                    continue;
                if (game.Players?[i]!=null)
                {
                    lstOponnentsLabels[lblIndex].Text = game.Players?[i].Name;
                    lstOponnentsLabels[lblIndex++].BackgroundColor = Colors.Red;
                }
               
            }
        }     
        private void OnComplete(Task task)
        {
            if(!task.IsCompletedSuccessfully)
                Toast.Make(Strings.JoinGameErr, ToastDuration.Long).Show();
        }

        public void AddSnapshotListener()
        {
            game.AddSnapShotListener();
        }

        public void RemoveSnapshotListener()
        {
            game.RemoveSnapShotListener();
        }

        public string Name
        {
            get { return game.CurrentPlayer!.Name; }
        }
        public ViewCard Card1
        {
            get
            {
                if ( game.CurrentPlayer==null|| game.CurrentPlayer.FBCard1 == null)
                    return new ViewCard();         
                return new ViewCard(game.CurrentPlayer.FBCard1);
            }
        }
        public ViewCard Card2
        {
            get
            {
                if (game.CurrentPlayer == null || game.CurrentPlayer.FBCard2 == null)
                    return new ViewCard();
                return new ViewCard(game.CurrentPlayer.FBCard2);
            }
        }

        public string Status
        {
            get { return game.CurrentStatus; }
        }
    }
}
