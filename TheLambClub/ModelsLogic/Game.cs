using CommunityToolkit.Mvvm.Messaging;
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
                if (Players != null)
                {
                    foreach (Player player in Players!)
                    {
                        if (player != null && player.Id == new FbData().UserId)
                        {
                            p = player;
                        }
                    }
                }
                return p;
            }
        }
        public override string CurrentStatus => IsFull ? Strings.PlayingStatus : Strings.WaitingStatus;
        public override bool IsMyTurn
        {
            get
            {
                bool IsMyTurn = false;
                if (Players != null)               
                    IsMyTurn = Players[CurrentPlayerIndex].Id == fbd.UserId && IsFull;               
                return IsMyTurn;
            }
        }
        public override bool IsHost=> HostId == fbd.UserId;
        public Game(){ }

        public override void NextTurn()
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % CurrentNumOfPlayers;//מעביר תור
            UpdateFBTurnUpdate((task) => OnGameChanged?.Invoke(this, EventArgs.Empty));//עידכון fb ומסך
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
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
        }
        protected override void ChangeIsFoldedToFalse()
        {
            foreach (Player player in Players!)
            {
                player?.IsFolded = false;
            }
        }
        public Game(int selectedNumberOfPlayers)
        {
            HostName = new User().UserName;
            Created = DateTime.Now;
            NumberOfPlayers = selectedNumberOfPlayers;
            CurrentNumOfPlayers = 1;
            MaxNumOfPlayers = selectedNumberOfPlayers;
            CurrentPlayerIndex = 0;
        }
        protected override void RegisterTimer()
        {
            WeakReferenceMessenger.Default.Register<AppMessage<long>>(this, (r, m) =>
            {
                OnMessageReceived(m.Value);
            });
        }
        protected override void OnMessageReceived(long timeLeft)
        {
            TimeLeft = timeLeft == Keys.FinishedSignal ? Strings.TimeUp : double.Round(timeLeft / 1000, 1).ToString();
            if (timeLeft == Keys.FinishedSignal && CurrentPlayer?.IsFolded == false)
            {
                PickedFold();
            }
            TimeLeftChanged?.Invoke(this, EventArgs.Empty);
        }
        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }

        protected override void FillArrayAndAddCards(bool upDateFB, Action<Task> OnComplete)
        {
            foreach (Player item in Players!)
            {
                if (item != null)
                {
                    item.FBCard1 = SetOfCards.GetRandomCard();
                    item.FBCard2 = SetOfCards.GetRandomCard();
                }
            }
            if (upDateFB)
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
            foreach (Player player in Players!)
            {
                if (player != null && player!.Id == fbd.UserId)
                    return;
            }
            Player newPlayer = new(MyName, fbd.UserId);
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
        public override bool IsFull { get => CurrentNumOfPlayers == MaxNumOfPlayers; set => _ = CurrentNumOfPlayers == MaxNumOfPlayers; }
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
                BoardCards[0] = SetOfCards.GetRandomCard();
                BoardCards[1] = SetOfCards.GetRandomCard();
                BoardCards[2] = SetOfCards.GetRandomCard();
            }
            else if (RoundNumber == 2)
            {
                BoardCards[3] = SetOfCards.GetRandomCard();
            }
            else if (RoundNumber == 3)
            {
                BoardCards[4] = SetOfCards.GetRandomCard();
            }
        }
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
            result=countNotFolded == 1;
            return result;
        }
        protected override int FirstPlayerWhichIsNotFold()
        {
            int i = 0;
            foreach (Player player in Players!)
            {
                if(player != null && !player.IsFolded)
                {
                    return i;
                }
                i++;
            }
           return -1;
        }
        public override void BetFunction(object obj) // פונקצית הימורים צריך לתקן
        {
            //CurrentPlayer?.CurrentMoney = CurrentPlayer.CurrentMoney - CurrentPlayer.CurrentBet;
            //if (IsFull && Players?[BeforeCurrentPlayerIndex()].CurrentBet < CurrentPlayer?.CurrentBet && Players?[BeforeCurrentPlayerIndex()].CurrentBet != 0)
            //{
            //    CurrentPlayer.IsReRazed = true;
            //    CurrentPlayerIndex = FirstPlayerWhichIsNotFold();
            //}
            //Dictionary<string, object> dict = new()
            //{
            //    { nameof(Players), Players! },
            //};
            //fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
            //NextTurn();
        }
        protected override int BeforeCurrentPlayerIndex() // פונקצית הימורים צריך לתקן
        {
            int beforeIndex = -1;
            if (CurrentPlayerIndex == 0)
            {
                for (int i = MaxNumOfPlayers - 1; i > 0; i--)
                {
                    if (Players != null && Players[i] != null && !Players[i]!.IsFolded)
                    {
                        beforeIndex= i;
                    }
                }
            }
            else
            {
                for (int i = CurrentPlayerIndex - 1; i >= 0; i--)
                {
                    if (Players != null && Players[i] != null && !Players[i]!.IsFolded)
                    {
                        beforeIndex= i;
                    }
                }
            }
            return beforeIndex;
        }
        public override void CallFunction() // פונקצית הימורים צריך לתקן
        {
            //if (Players?[BeforeCurrentPlayerIndex()].CurrentBet != CurrentPlayer?.CurrentBet)
            //{
            // CurrentPlayer?.CurrentMoney = CurrentPlayer.CurrentMoney - CurrentPlayer.CurrentBet;
            //}
            //Dictionary<string, object> dict = new()
            //{
            //    { nameof(Players), Players! },
            //};
            //fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
            NextTurn();
        }
        protected override bool EveryOneIsNotRerazeing()
        {
            bool result = true;

            foreach (Player player in Players!)
            {
                if (player != null && player.IsReRazed)
                {
                    result= false;
                }
            }
            return result;  
        }
        protected override void EndHand()
        {
            EndOfHand = false;
            ChangeIsFoldedToFalse();
            RoundNumber = 0;
            CurrentPlayerIndex = 0;
            FillBoard();
            FillArrayAndAddCards(false,(t) => { });                    
            Dictionary<string, object> dict = new()
            {
                { nameof(CurrentPlayerIndex), CurrentPlayerIndex! },
                { nameof(BoardCards), BoardCards },
                { nameof(RoundNumber), RoundNumber },
                { nameof(Players), Players! },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
        }
        protected override void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Console.WriteLine("Game OnChange called");
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                bool currentPlayerIndexChange = CurrentPlayerIndex != updatedGame.CurrentPlayerIndex;
                bool isEndOfRound = CurrentPlayerIndex > 0 && updatedGame.CurrentPlayerIndex == 0 && EveryOneIsNotRerazeing();
                bool changedToFull = CurrentNumOfPlayers < MaxNumOfPlayers && updatedGame.CurrentNumOfPlayers == MaxNumOfPlayers; 
                bool isGameStarted = CurrentNumOfPlayers != updatedGame.CurrentNumOfPlayers && updatedGame.CurrentNumOfPlayers == MaxNumOfPlayers;
                bool isHandEnded = false;
                string WinnerName = string.Empty;
                Dictionary<Player, HandRank> ranks = [];
                Player[] playersArray = null!;
                Players = updatedGame.Players;
                CurrentNumOfPlayers = updatedGame.CurrentNumOfPlayers;
                RoundNumber = updatedGame.RoundNumber;
                BoardCards = updatedGame.BoardCards;
                CurrentPlayerIndex = updatedGame.CurrentPlayerIndex;
                Console.WriteLine("CurrentNumOfPlayers" + CurrentNumOfPlayers);
                Console.WriteLine("updatedGame.CurrentNumOfPlayers" + updatedGame.CurrentNumOfPlayers);
                int beforeMePlayerIndex= BeforeCurrentPlayerIndex();
                //if (beforeMePlayerIndex >-1 && IsFull && Players?[beforeMePlayerIndex].CurrentBet != CurrentPlayer?.CurrentBet)
                //{
                //    CheckOrCall = Strings.Call + Players?[BeforeCurrentPlayerIndex()].CurrentBet + "$";
                //    OnCheckOrCallChanged?.Invoke(this, EventArgs.Empty);
                //}
                //else
                //{
                //    CheckOrCall = Strings.Check;
                //    OnCheckOrCallChanged?.Invoke(this, EventArgs.Empty);
                //}
                if ((IsOneStaying() && IsFull || RoundNumber == HandComplete) )
                {
                    if (IsOneStaying())
                    {
                        Player[] Winner = new Player[1];
                        foreach (Player player in Players!)
                        {
                            if (player != null && !player.IsFolded)
                            {
                                Winner[0] = player;
                            }
                        }
                        OnwinnerSelected?.Invoke(this, new WinningPopupEvent(Winner, null!));
                        IsPopupOpen= false;
                    }
                    else
                    {
                        foreach (Player player in Players!)
                        {
                            if (player != null && !player.IsFolded)
                            {
                                HandRank handRank = player.EvaluateBestHand(BoardCards);
                                ranks.Add(player, handRank);
                            }
                        }
                         playersArray = new Player[ranks.Count];
                        ranks.Keys.CopyTo(playersArray, 0);
                        Array.Sort(playersArray, (p1, p2) =>
                        {
                            return ranks[p2].Compare(ranks[p1]);
                        });
                        OnwinnerSelected?.Invoke(this, new WinningPopupEvent(playersArray, ranks));
                        IsPopupOpen = false;
                    }
                    if (IsHost)
                    {
                        EndHand();
                        isHandEnded= true;
                    }
                }
                if (CurrentPlayer != null && IsMyTurn && CurrentPlayer.IsFolded)
                {
                    NextTurn();
                }
                if (IsHost && isEndOfRound&&!isHandEnded)
                {
                    RoundNumber++;
                    FillBoard();
                    UpdateBoard((t) => { });
                    EndOfHand = RoundNumber == HandComplete;
                }
                if (IsHost && changedToFull && !isHandEnded)
                {
                    FillArrayAndAddCards(true,(t) => { });
                }              
                if (!TimerCreated)
                {
                    RegisterTimer();
                    TimerCreated = true;
                }
                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {            
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
