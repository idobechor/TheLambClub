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
        public GamePageVM(Game game)
        {
            game.OnGameChanged += OnGameChanged;
            this.game = game;
            if (!game.IsHostUser)
                game.UpdateGuestUser(OnComplete);
        }
      
        //Player 1 view
        public string player1Name => game.Player1!.Name;
        public ImageSource player1Card1 => game.Player1!.card1.Source;
        public ImageSource player1Card2 => game.Player1!.card2.Source;
        //player 2 view
        public string player2Name => game.Player2!.Name;
        public ImageSource player2Card1 => game.Player2!.card1.Source;
        public ImageSource player2Card2 => game.Player2!.card2.Source;
        //Player3 view
        public string player3Name => game.Player3!.Name;
        public ImageSource player3Card1 => game.Player3!.card1.Source;
        public ImageSource player3Card2 => game.Player3!.card2.Source;

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
