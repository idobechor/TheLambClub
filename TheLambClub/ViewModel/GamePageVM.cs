using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    class GamePageVM : ObservableObject
    {
        private readonly Game game;
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
        public string MyName;
        public ICommand NextTurnCommand => new Command(NextTurn);
        public string CurrentStatus => game.CurrentStatus;
        public bool IsMyTurn => game.IsMyTurn;
        private readonly List<Label> lstOponnentsLabels = [];        
        private void NextTurn(object obj)
        {
            game.NextTurn();
            OnPropertyChanged(nameof(CurrentStatus));
        }
        private void OnGameChanged(object? sender, EventArgs e)
        {
            DisplayOponnentsNames();
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(IsMyTurn));
            OnPropertyChanged(nameof(BoardCards));
        }
        public GamePageVM(Game game, Grid grdOponnents)
        {
            MyName = game.MyName;
    
            this.game = game;
            InitOponnentsGrid(grdOponnents);
            game.OnGameChanged += OnGameChanged;
            game.OnGameDeleted += OnGameDeleted;            
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
                if (game.CurrentPlayer!.Id == game.Players?[i].Id)
                    continue;
                lstOponnentsLabels[lblIndex].Text = game.Players?[i].Name;
                lstOponnentsLabels[lblIndex++].BackgroundColor = Colors.Red;
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
            get { return new ViewCard(game.CurrentPlayer!.FBCard1); } 
        }

        public ViewCard Card2
        {
            get { return new ViewCard(game.CurrentPlayer!.FBCard2); }
        }

        public string Status
        {
            get { return game.CurrentStatus; }
        }
    }
}
