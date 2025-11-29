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
        public string MyName;
        public ObservableCollection<Player> Players;
        public ObservableCollection<Player> OtherPlayers { get; set; }

        public PlayerVM CurrentPlayer { get; set; }


        public GamePageVM(Game game)
        {
            CurrentPlayer = new PlayerVM(game.CurrentPlayer);
            board = new Board();
            MyName = game.MyName;
            Players = new ObservableCollection<Player> (game.Players);
            OtherPlayers = new ObservableCollection<Player>(game.OtherPlayers);
       
            this.game = game;
            if (!game.IsHostUser)
                game.UpdateGuestUser(OnComplete);           
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
