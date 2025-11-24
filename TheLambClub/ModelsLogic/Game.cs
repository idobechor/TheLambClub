
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Plugin.CloudFirestore;
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    public class Game : GameModel
    {
        public Game(NumberOfPlayers selectedNumberOfPlayers)
        {
            HostName = new User().UserName;
            Created = DateTime.Now;
            NumberOfPlayers = selectedNumberOfPlayers;
            IsFull = false;
            CurrentNumOfPlayers = 1;
            MaxNumOfPlayers = selectedNumberOfPlayers.NumPlayers;
            PlayersNames = new string[MaxNumOfPlayers];
            Players = [];
            
        }

        public void createPlayers()
        {
            Players!.Add(new Player()); 
            //if (Players[0]!=null)
            //{
            //    Player1 = Players[0];
            //}
            //if (Players.Count==2&& !(Players[1].Name==null))
            //{
            //    Player2 = Players[1];
            //}
            //if (Players.Count==3&& !(Players[2].Name==null))
            //{
            ////    Player3 = Players[2];
            //}

        }
        public Game()
        {
            Players = [];
           
        }

      
        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }


        public override void AddSnapShotListener()
        {
            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, Id, OnChange);
        }
      
        public override void RemoveSnapShotListener()
        {
            ilr?.Remove();
            action=Actions.Deleted;
            DeleteDocument(OnComplete);
        }

        private void OnComplete(Task task)
        {
            if(task.IsCompletedSuccessfully)
                if (action == Actions.Deleted)
                    OnGameDeleted?.Invoke(this, EventArgs.Empty);
        }



        public void UpdateGuestUser(Action<Task> OnComplete)
        {
            PlayersNames?[CurrentNumOfPlayers - 1] = MyName;
            CurrentNumOfPlayers++;
            if (CurrentNumOfPlayers == MaxNumOfPlayers)
                IsFull = true;
            UpdateFireBaseJoinGame(OnComplete);
        }

        private void UpdateFireBaseJoinGame(Action<Task> OnComplete)
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(PlayersNames), PlayersNames! },
                { nameof(IsFull), IsFull },
                {  nameof(CurrentNumOfPlayers), CurrentNumOfPlayers }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
            action=Actions.Deleted;
        }

        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id, OnComplete);
        }

        private void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                IsFull = updatedGame.IsFull;
                PlayersNames = updatedGame.PlayersNames;
                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {

                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.Navigation.PopAsync();
                    Toast.Make(Strings.GameDeleted, ToastDuration.Long).Show();
                });
            }
        }
    }
}
