
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

        public override bool IsHost
        {
            get
            {
                return HostId == fbd.UserId;
            }
        }

        public override void NextTurn()
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % CurrentNumOfPlayers;
            UpdateFBTurnUpdate((task) => OnGameChanged?.Invoke(this, EventArgs.Empty));
        }

        public Game(NumberOfPlayers selectedNumberOfPlayers)
        {
            HostId = new User().fbd.UserId; 
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
                { nameof(RoundNumber), RoundNumber },
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

        private void UpdateBoard(Action<Task> OnComplete)
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(BoardCards), BoardCards },
                { nameof(RoundNumber), RoundNumber },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }

        public override bool IsFull
        {
            get { return CurrentNumOfPlayers == MaxNumOfPlayers; }

            set => _ = CurrentNumOfPlayers == MaxNumOfPlayers;           
        }

        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id, OnComplete);
        }
        private void FillBoard()
        {
           if(RoundNumber==1)
           {
                BoardCards[0]= setOfCards.GetRandomCard();
                ViewCard[0] = new Card(((int)BoardCards[0].Shape), BoardCards[0].Value);
                BoardCards[1]= setOfCards.GetRandomCard();
                ViewCard[1] = new Card(((int)BoardCards[1].Shape), BoardCards[1].Value);
                BoardCards[2]= setOfCards.GetRandomCard();
                ViewCard[2] = new Card(((int)BoardCards[2].Shape), BoardCards[2].Value);
            }
           else if(RoundNumber==2)
            {
                BoardCards[3] = setOfCards.GetRandomCard();
                ViewCard[3] = new Card(((int)BoardCards[3].Shape), BoardCards[3].Value);
            }
           else if(RoundNumber==3)
            {
                BoardCards[4] = setOfCards.GetRandomCard();
                ViewCard[4] = new Card(((int)BoardCards[4].Shape), BoardCards[4].Value);
            }

        }
        private void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                Console.WriteLine("on change " + IsHost + " CurrentPlayerIndex: " + CurrentPlayerIndex + " updatedGame.CurrentPlayerIndex " + updatedGame.CurrentPlayerIndex);
                if (IsHost && CurrentPlayerIndex > 0 && updatedGame.CurrentPlayerIndex == 0)
                {
                    RoundNumber++;
                    FillBoard();
                    UpdateBoard((t) => { });
                }
                CurrentNumOfPlayers = updatedGame.CurrentNumOfPlayers;
                CurrentPlayerIndex = updatedGame.CurrentPlayerIndex;
                PlayersNames = updatedGame.PlayersNames;
                PlayersIds = updatedGame.PlayersIds;
                RoundNumber = updatedGame.RoundNumber;
                BoardCards = updatedGame.BoardCards;
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
