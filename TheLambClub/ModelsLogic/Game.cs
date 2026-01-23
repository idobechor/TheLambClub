using CommunityToolkit.Mvvm.Messaging;
using Plugin.CloudFirestore;
using System.Collections.ObjectModel;
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    public class Game : GameModel
    {
        public override int CurrentPlayerIndex
        {
            get => _currentPlayerIndex;
            set
            { 
                if (Players != null && _currentPlayerIndex >= 0 && _currentPlayerIndex < Players.Length)                
                    if (null != Players![_currentPlayerIndex]&& !Players![_currentPlayerIndex].IsFolded)
                        beforeCurrentPlayerIndex = _currentPlayerIndex;
                _currentPlayerIndex = value;
            }
        }
        public override ObservableCollection<ViewCard>? BoardViewCards
        {
            get
            {
                return [.. BoardCards.Select(c =>
                {
                    if (c == null)
                        return new ViewCard();
                    return new ViewCard(c);
                })];

            }
        }
        public override ViewCard? ViewCard1
        { get
            {
                ViewCard vc;
                if (CurrentPlayer == null || CurrentPlayer.FBCard1 == null)
                    vc = new ViewCard();
                else
                    vc = new ViewCard(CurrentPlayer.FBCard1);
                return vc;
            }
        }
        public override ViewCard? ViewCard2
        { get
            {
                ViewCard vc;
                if (CurrentPlayer == null || CurrentPlayer.FBCard2 == null)
                    vc = new ViewCard();
                else
                    vc = new ViewCard(CurrentPlayer.FBCard2);
                return vc;
            }
        }

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
            PlayerBeforeId = Players![CurrentPlayerIndex].Id;
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % CurrentNumOfPlayers;
            UpdateFBTurnUpdate((task) => OnGameChanged?.Invoke(this, EventArgs.Empty));
            Dictionary<string, object> dict = new()
            {
                { nameof(PlayerBeforeId), PlayerBeforeId! },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
        }
        public override void PickedFold()
        {
            CurrentPlayer!.IsFolded = true;
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
                player.IsFolded = false;
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
            beforeCurrentPlayerIndex = selectedNumberOfPlayers-1;
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
                { nameof(beforeCurrentPlayerIndex), beforeCurrentPlayerIndex },
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
                  { nameof(beforeCurrentPlayerIndex), beforeCurrentPlayerIndex },
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
            foreach (Player player in Players!)           
                if(player != null && !player.IsFolded)               
                    countNotFolded++;               
            return countNotFolded == 1;
        }
        protected override int FirstPlayerWhichIsNotFold()
        {
            int i = 0;
            foreach (Player player in Players!)
            {
                if(player != null && !player.IsFolded)               
                    break;           
                i++;
            }
           return i;
        }

        //protected override int BeforeCurrentPlayerIndex() {
        //    int index = CurrentPlayerIndex;
        //    if (Players != null && Players.Length == MaxNumOfPlayers)
        //    {
        //        for (int i = 0; i < Players.Length - 1; i++)
        //        {
        //            index = i; 
        //            if (Players[i] != null && Players[i].Id == PlayerBeforeId)
        //            {
        //                break;
        //            } 
        //        } 
        //    }
        //    return index;
        //}
        public override void BetFunction(object obj)
        {
            Player prevPlayer = Players![beforeCurrentPlayerIndex];
            CurrentPlayer!.CurrentMoney -= CurrentPlayer!.CurrentBet;
            Pot[RoundNumber] += CurrentPlayer.CurrentBet;
            if (IsFull && prevPlayer.CurrentBet < CurrentPlayer.CurrentBet && prevPlayer.CurrentBet != 0)
            {
                CurrentPlayer.IsReRazed = true;
                CurrentPlayerIndex = FirstPlayerWhichIsNotFold();
                foreach (Player player in Players!)
                {
                    if (player.Id != CurrentPlayer.Id)
                        player.IsReRazed = false;
                }
            }
            else          
                CurrentPlayerIndex = (CurrentPlayerIndex + 1) % CurrentNumOfPlayers;      
            Dictionary<string, object> update = new()
            {
                { nameof(CurrentPlayerIndex), CurrentPlayerIndex },
                { nameof(Players), Players },
                  { nameof(beforeCurrentPlayerIndex), beforeCurrentPlayerIndex },
                { nameof(Pot), Pot }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, update, _ => { });
            //OnMyMoneyChanged?.Invoke(this, EventArgs.Empty);
            Console.WriteLine("Betiing");
        }

        public override void CallFunction()
        {
            double moneyToCall = (Players![beforeCurrentPlayerIndex].CurrentBet - CurrentPlayer!.CurrentBet);
            if (moneyToCall > 0)
            {
                Players[CurrentPlayerIndex].CurrentBet = Players![beforeCurrentPlayerIndex].CurrentBet;
                Players[CurrentPlayerIndex].CurrentMoney -= moneyToCall;
                Pot[RoundNumber] += moneyToCall;
            }
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % CurrentNumOfPlayers;
            Dictionary<string, object> update = new()
            {
                { nameof(CurrentPlayerIndex), CurrentPlayerIndex },
                 { nameof(beforeCurrentPlayerIndex), beforeCurrentPlayerIndex },
                { nameof(Players), Players },
                { nameof(Pot), Pot }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, update, _ => { });
            //OnMyMoneyChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override bool EveryOneIsNotRerazeing()
        {
            bool result = true;
            foreach (Player player in Players!)
                if (player != null && player.IsReRazed)
                    result= false;
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
                { nameof(beforeCurrentPlayerIndex), beforeCurrentPlayerIndex },
                { nameof(BoardCards), BoardCards },
                { nameof(RoundNumber), RoundNumber },
                { nameof(Players), Players! },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
        }
        protected bool AllCall()
        {
            bool IsAllCall= true;
            double bet = -1;
            foreach (Player player in Players!)           
                if (player!=null&&player.IsReRazed)             
                    bet=player.CurrentBet;

            foreach (Player player in Players!)           
                if (player!=null&&!player.IsReRazed&&!player.IsFolded&&player.CurrentBet!=0)             
                    if(player.CurrentBet!=bet)
                    {
                        IsAllCall=false;
                        break;
                    }
                
            

            return IsAllCall;
        }
        protected void EndOfRound()
        {
            foreach (Player player in Players!)
            {
                Console.WriteLine($"Current index {CurrentPlayerIndex} Current Money {CurrentPlayer?.CurrentMoney} 1  ");
                player!.CurrentBet = 0;
                player.IsReRazed = false;
                Console.WriteLine($"Current index {CurrentPlayerIndex} Current Money {CurrentPlayer?.CurrentMoney} 2  ");
            }
            RoundNumber++;
            Console.WriteLine("Round Number Updated" + RoundNumber + "  ," + DateTime.Now);
            FillBoard();
            EndOfHand = RoundNumber == HandComplete;
            Dictionary<string, object> dict = new()
            {
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
                //    if (Players !=null&& Players![BeforeCurrentPlayerIndex()] !=null&& updatedGame.Players![BeforeCurrentPlayerIndex()].CurrentMoney!= Players![BeforeCurrentPlayerIndex()].CurrentMoney)
                //    {
                //        OnMyMoneyChanged?.Invoke(this, EventArgs.Empty);
                //    }
                IsHappened = updatedGame.BoardCards != BoardCards;
                bool RoundChanges = RoundNumber != updatedGame.RoundNumber;
                bool currentPlayerIndexChange = CurrentPlayerIndex != updatedGame.CurrentPlayerIndex;
                bool isEndOfRound = CurrentPlayerIndex > 0 && updatedGame.CurrentPlayerIndex == 0 && EveryOneIsNotRerazeing();
                bool changedToFull = CurrentNumOfPlayers < MaxNumOfPlayers && updatedGame.CurrentNumOfPlayers == MaxNumOfPlayers;
                bool isGameStarted = CurrentNumOfPlayers != updatedGame.CurrentNumOfPlayers && updatedGame.CurrentNumOfPlayers == MaxNumOfPlayers;
                bool isCheckOrCallChanged = (updatedGame.CurrentPlayer?.CurrentBet != CurrentPlayer?.CurrentBet);
                bool isHandEnded = false;
                string WinnerName = string.Empty;
                Dictionary<Player, HandRank> ranks = [];
                Player[] playersArray = null!;
                Players = updatedGame.Players;
                CurrentNumOfPlayers = updatedGame.CurrentNumOfPlayers;
                RoundNumber = updatedGame.RoundNumber;
                BoardCards = updatedGame.BoardCards;
                CurrentPlayerIndex = updatedGame.CurrentPlayerIndex;
                PlayerBeforeId = updatedGame.PlayerBeforeId;
                //if(AllCall())
                //{
                //    EndOfRound();
                //}
                if (isGameStarted)
                    updatedGame.beforeCurrentPlayerIndex = MaxNumOfPlayers - 1;
                if (IsFull && Players?[beforeCurrentPlayerIndex].CurrentBet > CurrentPlayer?.CurrentBet)
                {
                    CheckOrCall = Strings.Call + (Players[beforeCurrentPlayerIndex].CurrentBet - CurrentPlayer.CurrentBet) + "$";
                    int minimalBet = (int)(Players[beforeCurrentPlayerIndex].CurrentBet) * 2 > CurrentPlayer.CurrentMoney ? (int)CurrentPlayer.CurrentMoney : ((int)(Players[beforeCurrentPlayerIndex].CurrentBet) * 2);
                    MinBet = CurrentPlayer.CurrentMoney > minimalBet ? minimalBet : (int)CurrentPlayer.CurrentMoney;
                    OnCheckOrCallChanged?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    CheckOrCall = Strings.Check;
                    MinBet = 0;
                    OnCheckOrCallChanged?.Invoke(this, EventArgs.Empty);
                }
                if ((IsOneStaying() && IsFull || RoundNumber == HandComplete))
                {
                    if (IsOneStaying())
                    {
                        Player[] Winner = new Player[1];
                        foreach (Player player in Players!)
                            if (player != null && !player.IsFolded)
                                Winner[0] = player;
                        OnwinnerSelected?.Invoke(this, new WinningPopupEvent(Winner, null!));
                        IsPopupOpen = false;
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
                        Array.Sort(playersArray, (p1, p2) => ranks[p2].Compare(ranks[p1]));
                        OnwinnerSelected?.Invoke(this, new WinningPopupEvent(playersArray, ranks));
                        IsPopupOpen = false;
                    }
                    if (IsHost)
                    {
                        EndHand();
                        isHandEnded = true;
                    }
                }
                if (CurrentPlayer != null && IsMyTurn && CurrentPlayer.IsFolded)
                    NextTurn();
                if (CurrentPlayer != null && IsMyTurn && CurrentPlayer.IsReRazed)
                {
                    CurrentPlayer.IsReRazed = false;
                    CurrentPlayerIndex = (CurrentPlayerIndex + 1) % CurrentNumOfPlayers;
                    Dictionary<string, object> dict = new()
                    {
                    {nameof(Players), Players! },
                    {nameof(CurrentPlayerIndex), CurrentPlayerIndex! },
                    };
                    fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
                }
                Console.WriteLine($"IsHost && AllCall() && !RoundChanges && !IsHappened { IsHost && AllCall() && !RoundChanges && !IsHappened}");
                Console.WriteLine("IsHost:"+ IsHost + "  AllCall():"+ AllCall() + "!RoundChanges" + !RoundChanges + " !IsHappened"+ !IsHappened);
                
                if (IsHost && AllCall() && !IsHappened)
                {
                    EndOfRound();
                    Console.WriteLine("EndOfRound" +DateTime.Now);
                }
                if (IsFull&&IsHost &&EveryOneIsNotRerazeing()&&isEndOfRound && !isHandEnded)           
                    EndOfRound();            
                if (IsHost && changedToFull && !isHandEnded)              
                    FillArrayAndAddCards(true,(t) => { });                           
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
