
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Plugin.CloudFirestore;
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    public class Game : GameModel
    {
        public override Player? CurrentPlayer 
        {   
            get 
            {
                Player p = null!;
                if (Players == null)
                    return p;                
                foreach (Player player in Players!)
                {
                    if (player != null && player.Id == new FbData().UserId)
                    {
                        p = player;
                    }
                }
                return p;
            }
        }
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
        }

        public override bool IsMyTurn
        {
            get
            {
                if (Players == null)
                {
                    return false;
                }
                return Players[CurrentPlayerIndex].Id == fbd.UserId && IsFull;
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
        public override void PickedFold()
        {
            CurrentPlayer?.IsFolded = true;
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % CurrentNumOfPlayers;
            Dictionary<string, object> dict = new()
            {
                { nameof(Players), Players! },
                { nameof(CurrentPlayerIndex), CurrentPlayerIndex }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }
        protected override void ChangeIsFoldedToFalse()
        {
            foreach (Player player in Players!)
            {
                if (player != null)
                {
                    player.IsFolded = false;
                }
            }
            Dictionary<string, object> dict = new()
            {
                { nameof(Players), Players! },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
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
        }

        protected override void FillDummes()
        {
            for (int i = 0; i < MaxNumOfPlayers; i++)
            {
                //PlayersNames![i] = string.Empty;
                //PlayersIds![i] = string.Empty;
            }
        }

        public Game()
        {
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
            if(RoundNumber==0)
            {
                BoardCards[0] = null!;
                BoardCards[1] = null!;
                BoardCards[2] = null!;
                BoardCards[3] = null!;
                BoardCards[4] = null!;
            }
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
        protected override bool IsOneStaying()
        {
            int countNotFolded=0;
            bool result=false;
            foreach (Player player in Players!)
            {
                if(player != null && !player.IsFolded)
                {
                    countNotFolded++;
                }
            }
            result= countNotFolded == 1;
            return result;
        }
        protected override void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Console.WriteLine("Game OnChange called");
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                bool isEndOfRound = CurrentPlayerIndex > 0 && updatedGame.CurrentPlayerIndex == 0;
                bool changedToFull = CurrentNumOfPlayers < MaxNumOfPlayers && updatedGame.CurrentNumOfPlayers == MaxNumOfPlayers;
                bool EndOfHand = (RoundNumber < updatedGame.RoundNumber && updatedGame.RoundNumber == HandComplete);
                Players = updatedGame.Players;
                CurrentNumOfPlayers = updatedGame.CurrentNumOfPlayers;               
                RoundNumber = updatedGame.RoundNumber;
                BoardCards = updatedGame.BoardCards;
                CurrentPlayerIndex = updatedGame.CurrentPlayerIndex;
                if (IsHost && isEndOfRound)
                {
                    RoundNumber++;
                    FillBoard();
                    UpdateBoard((t) => { });
                }
                if (CurrentPlayer!=null&&IsMyTurn &&CurrentPlayer.IsFolded)
                {
                    NextTurn();
                }
                if (IsOneStaying() && IsFull|| EndOfHand)
                {
                    ChangeIsFoldedToFalse();
                    RoundNumber = 0;
                    FillBoard();
                    UpdateBoard((t) => { });
                    FillArrayAndAddCards(OnComplete);
                    Console.WriteLine("give cards");
                }
                if (IsHost && changedToFull)
                {
                    FillArrayAndAddCards(OnComplete);
                    Console.WriteLine("give cards");
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
