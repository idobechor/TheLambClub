using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    class GamePageVM : ObservableObject
    {
        private readonly Game game;
        public ObservableCollection<Card> BoardCards
        {
            get
            {
                return [.. game.BoardCards.Select(c =>
                {
                    if (c == null)
                        return new Card();
                    return new Card((int)c!.Shape, c!.Value);
                })];
                
            }
            
        }


        public string MyName;
        //public ObservableCollection<Player> Players { get => game.Players; set => game.Players = value; }
        //public ObservableCollection<PlayerVM> OtherPlayers=> game.OtherPlayers;
        public ICommand NextTurnCommand => new Command(NextTurn);
        public string CurrentStatus => game.CurrentStatus;
        public bool IsMyTurn => game.IsMyTurn;
        //public Player CurrentPlayer { get=>game.CurrentPlayer; set=>game.CurrentPlayer=value; }
        //public PlayerVM CurrentPlayerVM { get; set; }
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
            //OnPropertyChanged(nameof(CurrentPlayer));
        }
        public GamePageVM(Game game, Grid grdOponnents)
        {
            MyName = game.MyName;
    
            this.game = game;
            InitOponnentsGrid(grdOponnents);
            game.OnGameChanged += OnGameChanged;
            game.OnGameDeleted += OnGameDeleted;
            //if (!game.IsHostUser)
            //    game.UpdateGuestUser(OnComplete);

            // game.OnOtherPlayersChanged += OnOtherPlayersChanged;            
        }



        private void InitOponnentsGrid(Grid grdOponnents)
        {
            int oponnentsCount = game.MaxNumOfPlayers - 1;

            // 2 rows: Label + Images row
            grdOponnents.RowDefinitions.Clear();
            grdOponnents.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // label
            grdOponnents.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // images row

            for (int i = 0; i < oponnentsCount; i++)
            {
                grdOponnents.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                // ----- Label -----
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

                // ----- Images side by side -----
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

                // Horizontal container for the two images
                StackLayout imagesRow = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                    Spacing = 3,
                    Children = { img1, img2 }
                };

                // ----- Add to grid -----
                grdOponnents.Add(lbl, i, 0);
                grdOponnents.Add(imagesRow, i, 1);
            }

            Console.WriteLine("after build grid");
        }


        private void OnGameDeleted(object? sender, EventArgs e)
        {
        }

        private void DisplayOponnentsNames()
        {
            int lblIndex = 0;
            for (int i = 0; i < game.CurrentNumOfPlayers; i++)
            {
                if (game.CurrentPlayer.Id == game.PlayersIds?[i])
                    continue;
                lstOponnentsLabels[lblIndex].Text = game.PlayersNames?[i];
                lstOponnentsLabels[lblIndex++].BackgroundColor = Colors.Cyan;
            }
        }



        //public ImageSource? boardCard1
        //{
        //    get => board.Cards[0].Source;
        //}
        //public ImageSource? boardCard2
        //{
        //    get => board.Cards[1].Source;
        //}
        //public ImageSource? boardCard3
        //{
        //    get => board.Cards[2].Source;
        //}
        //public ImageSource? boardCard4
        //{
        //    get => board.Cards[3].Source;
        //}
        //public ImageSource? boardCard5
        //{
        //    get => board.Cards[4].Source;
        //}     
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
            get { return game.CurrentPlayer.Name; }
        }

        public Card Card1
        {
            get { return game.CurrentPlayer.card1; } 
        }

        public Card Card2
        {
            get { return game.CurrentPlayer.card2; }
        }

        public string Status
        {
            get { return game.CurrentStatus; }
        }
    }
}
