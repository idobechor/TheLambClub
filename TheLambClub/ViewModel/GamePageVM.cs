using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    class GamePageVM : ObservableObject
    {
        private readonly Game game;
        private readonly Board board = new();
        public string MyName => game.MyName;
        public ObservableCollection<Player> Players => game.Players;
        private readonly SeatsArrangement seatsArrangement = new();
        public GamePageVM(Game game)
        {
            game.OnGameChanged += OnGameChanged;
            this.game = game;
            if (!game.IsHostUser)
                game.UpdateGuestUser(OnComplete);
            ArrangePlayerSeats();
        }

        private void ArrangePlayerSeats()
        {
            double width = 400;
            double height = 600;
            seatsArrangement.ArrangeSeats(Players, width, height);
            foreach (var player in Players)
            {
                OnPropertyChanged(nameof(player.X));
                OnPropertyChanged(nameof(player.Y));
            }
        }
        //public PlayerVM Player1
        //{
        //    get => new PlayerVM(game.Player1!);
        //}
        //public PlayerVM Player2
        //{
        //    get => new PlayerVM(game.Player2!);
        //}


        private void OnGameChanged(object? sender, EventArgs e)
        { 
         //OnPropertyChanged(nameof(OpponentsNames));
        }
        public ImageSource? boardCard1
        {
            get => board.Cards[0].Source;
        }
        public ImageSource? boardCard2
        {
            get => board.Cards[1].Source;
        }
        public ImageSource? boardCard3
        {
            get => board.Cards[2].Source;
        }
        public ImageSource? boardCard4
        {
            get => board.Cards[3].Source;
        }
        public ImageSource? boardCard5
        {
            get => board.Cards[4].Source;
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
    }
}
