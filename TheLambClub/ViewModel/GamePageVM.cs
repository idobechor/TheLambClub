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
        private readonly Board board = new();
        public string MyName;
        public ObservableCollection<Player> Players { get => game.Players; set => game.Players = value; }
        public ObservableCollection<PlayerVM> OtherPlayers=> game.OtherPlayers;
        public ICommand NextTurnCommand => new Command(NextTurn);
        public string CurrentStatus => game.CurrentStatus;
        public bool IsMyTurn => CurrentPlayer.IsCurrentTurn;
        public Player CurrentPlayer { get=>game.CurrentPlayer; set=>game.CurrentPlayer=value; }
        public PlayerVM CurrentPlayerVM { get; set; }
        

        private void NextTurn(object obj)
        {

            game.NextTurn();
            OnPropertyChanged(nameof(CurrentStatus));
        }
        private void OnGameChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Players));
            //OnPropertyChanged(nameof(OtherPlayers));
            //OnPropertyChanged(nameof(CurrentPlayer));
        }
        public GamePageVM(Game game)
        {
            CurrentPlayerVM = new PlayerVM(game.CurrentPlayer);
            board = new Board();
            MyName = game.MyName;
    
            this.game = game;
            if (!game.IsHostUser)
                game.UpdateGuestUser(OnComplete);

            game.OnOtherPlayersChanged += OnOtherPlayersChanged;            
        }

        private void OnGameDeleted(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnOtherPlayersChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(OtherPlayers));
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
    }
}
