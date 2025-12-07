
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Plugin.CloudFirestore;
using System.Security.Cryptography.Xml;
using TheLambClub.Models;
using TheLambClub.ViewModel;

namespace TheLambClub.ModelsLogic
{
    public class Game : GameModel
    {
        public override string CurrentStatus
        {
            get 
            { 
                if (!IsFull)
                {
                    return "Waiting for Players";
                }
                return "Playing";
            }
            set;
        }

        public override bool IsMyTurn
        {
            get
            {
                if (PlayersIds == null)
                {
                    return false;
                }
                return PlayersIds[CurrentPlayerIndex] == fbd.UserId;
            }
        }

        public override void NextTurn()
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % CurrentNumOfPlayers;
            UpdateFBTurnUpdate((t) => OnGameChanged?.Invoke(this, EventArgs.Empty));
        }

        public Game(NumberOfPlayers selectedNumberOfPlayers)
        {
            HostName = new User().UserName;
            Created = DateTime.Now;
            NumberOfPlayers = selectedNumberOfPlayers;
            CurrentNumOfPlayers = 1;
            MaxNumOfPlayers = selectedNumberOfPlayers.NumPlayers;
            CurrentPlayerIndex=0;
            PlayersNames = new string[MaxNumOfPlayers];
            PlayersIds = new string[MaxNumOfPlayers];
            FillDummes();
            CurrentPlayer = new Player(MyName, fbd.UserId);
            Console.WriteLine("Game constructor");
            createPlayers();
        }

        private void FillDummes()
        {
            for (int i = 0; i < MaxNumOfPlayers; i++) 
            {
                PlayersNames![i] = string.Empty;
                PlayersIds![i] = string.Empty;
            }
        }

        private void createPlayers()
        {
            int i = 0;
            if (PlayersNames != null) {
                foreach (string playerName in PlayersNames!)
                {
                    if (playerName != string.Empty)
                    {
                        Player player = new(playerName, PlayersIds![i++]);
                        Players!.Add(player);
                    }
                }
            }
            
            CurrentPlayer = new Player(MyName, fbd.UserId);
        }

        public Game()
        {
            Console.WriteLine("Game empty constructor");
            createPlayers();
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
            DeleteDocument(OnComplete);
        }

        private void OnComplete(Task task)
        {
            if(task.IsCompletedSuccessfully)
               OnGameDeleted?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateGuestUser(Action<Task> OnComplete)
        {
            foreach (string id in PlayersIds!)
            {
                if (id == fbd.UserId)
                    return;
            }

            Console.WriteLine("joining game");
            PlayersNames?[CurrentNumOfPlayers] = MyName;
            PlayersIds?[CurrentNumOfPlayers] = fbd.UserId;
            CurrentNumOfPlayers++;
            UpdateFireBaseJoinGame(OnComplete);
        }

        private void UpdateFireBaseJoinGame(Action<Task> OnComplete)
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(PlayersNames), PlayersNames! },
                { nameof(PlayersIds), PlayersIds! },
                { nameof(CurrentNumOfPlayers), CurrentNumOfPlayers },
                { nameof(CurrentPlayerIndex), CurrentPlayerIndex },
                { nameof(IsFull), IsFull },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }

        private void UpdateFBTurnUpdate(Action<Task> OnComplete)
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(CurrentPlayerIndex), CurrentPlayerIndex },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }

        public override bool IsFull
        {
            get { return CurrentNumOfPlayers == MaxNumOfPlayers; }
            set;
        } 

        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id, OnComplete);
        }

        private void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Console.WriteLine("received game changed");
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                CurrentNumOfPlayers = updatedGame.CurrentNumOfPlayers;
                CurrentPlayerIndex = updatedGame.CurrentPlayerIndex;
                PlayersNames = updatedGame.PlayersNames;
                PlayersIds = updatedGame.PlayersIds;
                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {

                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.Navigation.PopAsync();
                    Toast.Make(Strings.GameDeleted, ToastDuration.Long).Show();
                });
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
