
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Plugin.CloudFirestore;
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
                    return Strings.WaitingStatus;
                }
                return Strings.PlayingStatus;
            }
            set;
        }

        public override bool IsMyTurn
        {
            get
            {
                if (Players == null)
                {
                    return false;
                }
                return Players[CurrentPlayerIndex].Id == fbd.UserId;
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
            CurrentPlayerIndex = 0;
            FillDummes();
            CurrentPlayer = new Player(MyName, fbd.UserId);
            CreatePlayers();
        }

        protected override void FillDummes()
        {
            for (int i = 0; i < MaxNumOfPlayers; i++)
            {
                //PlayersNames![i] = string.Empty;
                //PlayersIds![i] = string.Empty;
            }
        }

        protected override void CreatePlayers()
        {
            //int i = 0;
            //if (PlayersNames != null)
            //{
            //    foreach (string playerName in PlayersNames!)
            //    {
            //        if (playerName != string.Empty)
            //        {
            //            Player player = new(playerName, PlayersIds![i++]);
            //            Players!.Add(player);
            //        }
            //    }
            //}

            CurrentPlayer = new Player(MyName, fbd.UserId);
        }

        public Game()
        {
            CreatePlayers();
        }

        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }

        protected override void FillArrayAndAddCards(Action<Task> OnComplete)
        {
            foreach (Player item in Players!)
            {
                Console.WriteLine("item");
                if(item!=null)
                {
                    item.FBCard1 = setOfCards.GetRandomCard();
                    item.FBCard2 = setOfCards.GetRandomCard();
                    Console.WriteLine("cards has haded");
                }
            }
            UpdatePlayersArray(_ => { });
        }
        protected override void UpdatePlayersArray(Action<Task> OnComplete)
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(Players), Players! },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
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

        protected override void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
        }

        public override void UpdateGuestUser(Action<Task> OnComplete)
        {
            Console.WriteLine("Updating guest user" + CurrentNumOfPlayers);
            foreach (Player player in Players!)
            {
                if (player!=null&& player!.Id == fbd.UserId)
                    return;
            }
            Console.WriteLine("players initalized");
            Player newPlayer = new(MyName, fbd.UserId);
            Console.WriteLine(MyName+",");
            Players[CurrentNumOfPlayers] = newPlayer;
            CurrentNumOfPlayers++;
            UpdateFireBaseJoinGame(OnComplete);
        }

        protected override void UpdateFireBaseJoinGame(Action<Task> OnComplete)
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(Players), Players! },
                { nameof(CurrentNumOfPlayers), CurrentNumOfPlayers },
                { nameof(CurrentPlayerIndex), CurrentPlayerIndex },
                { nameof(RoundNumber), RoundNumber },
                { nameof(IsFull), IsFull },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }

        protected override void UpdateFBTurnUpdate(Action<Task> OnComplete)
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(CurrentPlayerIndex), CurrentPlayerIndex },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }

        protected override void UpdateBoard(Action<Task> OnComplete)
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
        protected override void FillBoard()
        {
            if (RoundNumber == 1)
            {
                BoardCards[0] = setOfCards.GetRandomCard();
                BoardCards[1] = setOfCards.GetRandomCard();
                BoardCards[2] = setOfCards.GetRandomCard();
            }
            else if (RoundNumber == 2)
            {
                BoardCards[3] = setOfCards.GetRandomCard();
            }
            else if (RoundNumber == 3)
            {
                BoardCards[4] = setOfCards.GetRandomCard();
            }
        }
        //protected void AddCardsToPlayers()
        //{
        //    for (int i = 0; i < MaxNumOfPlayers; i++)
        //    {
        //        if(Players[i]!=null)
        //        {
        //            Players![i] = Players[i];
        //        }
        //    }
        //}

        protected override void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Console.WriteLine("Game OnChange called");
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                bool gameFull=false;
                if (IsHost && CurrentPlayerIndex > 0 && updatedGame.CurrentPlayerIndex == 0)
                {
                    Console.WriteLine("Fill board");
                    RoundNumber++;
                    FillBoard();
                    UpdateBoard((t) => { });
                }
                if (IsHost && CurrentNumOfPlayers < MaxNumOfPlayers && updatedGame.CurrentNumOfPlayers == MaxNumOfPlayers)
                {
                    gameFull = true;
                }
                Players = updatedGame.Players;
                CurrentNumOfPlayers = updatedGame.CurrentNumOfPlayers;               
                RoundNumber = updatedGame.RoundNumber;
                BoardCards = updatedGame.BoardCards;
                CurrentPlayerIndex = updatedGame.CurrentPlayerIndex;
                if (gameFull)
                {
                    Console.WriteLine("Fill players cards");
                    FillArrayAndAddCards(OnComplete);
                }
                
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
