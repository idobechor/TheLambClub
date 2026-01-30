using CommunityToolkit.Mvvm.Messaging;
using Plugin.CloudFirestore;
using System.Collections.ObjectModel;
using TheLambClub.Models;
using TheLambClub.ViewModel;

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
                    if (Players[_currentPlayerIndex] != null && !Players[_currentPlayerIndex].IsFolded&& !Players[_currentPlayerIndex].IsOut)
                        if (value != _currentPlayerIndex)
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
        public override ViewCard? ViewCard1 => CurrentPlayer == null || CurrentPlayer.FBCard1 == null ?
            new ViewCard() : new ViewCard(CurrentPlayer.FBCard1);

        public override ViewCard? ViewCard2
        {
            get
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
        public override bool IsHost => HostId == fbd.UserId;
        public Game() { }

        public override void NextTurn(bool UpDateFB)
        {
            //PlayerBeforeId = Players![CurrentPlayerIndex].Id;
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % CurrentNumOfPlayers;
            if(UpDateFB)
               UpdateFBTurnUpdate((task) => OnGameChanged?.Invoke(this, EventArgs.Empty));
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
                player.IsAllIn = false;
                player.SumOfMoneyThatThePlayerWon = 0;
                player.CurrentBet = 0;
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
            beforeCurrentPlayerIndex = selectedNumberOfPlayers - 1;
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
            if (RoundNumber == 0)
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
            int countNotFolded = 0;
            foreach (Player player in Players!)
                if (player != null && !player.IsFolded)
                    countNotFolded++;
            return countNotFolded == 1;
        }
        protected override int FirstPlayerWhichIsNotFold()
        {
            int i = 0;
            foreach (Player player in Players!)
            {
                if (player != null && !player.IsFolded)
                    break;
                i++;
            }
            return i;
        }
        protected  int FirstPlayerWhichIsNotOffLine()
        {
            int i = 0;
            foreach (Player player in Players!)
            {
                if (player != null && !player.IsOut)
                    break;
                i++;
            }
            return i;
        }

        //protected override int BeforeCurrentPlayerIndex()
        //{
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
            if (CurrentPlayer.CurrentMoney == 0)// && Players !=null&& Players![beforeCurrentPlayerIndex] !=null&& !Players![beforeCurrentPlayerIndex]!.IsAllIn)
            {
                CurrentPlayer!.IsAllIn = true;
                CurrentPlayer.RoundAllIn = RoundNumber;
            }
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
            double maxBet = Players!.Max(p => p.CurrentBet);
            double moneyToCall = Math.Abs(maxBet - CurrentPlayer!.CurrentBet);
            CurrentPlayer!.CurrentBet = maxBet;
            CurrentPlayer.CurrentMoney -= moneyToCall;
            Pot[RoundNumber] += moneyToCall;
            if (CurrentPlayer.CurrentMoney==0)// && Players !=null&& Players![beforeCurrentPlayerIndex] !=null&& !Players![beforeCurrentPlayerIndex]!.IsAllIn)
            {
                CurrentPlayer!.IsAllIn = true;
                CurrentPlayer.RoundAllIn = RoundNumber;
            }
            Dictionary<string, object> update = new()
            {
                { nameof(CurrentPlayerIndex), CurrentPlayerIndex },
                { nameof(beforeCurrentPlayerIndex), beforeCurrentPlayerIndex },
                { nameof(Players), Players! },
                { nameof(Pot), Pot }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, update, _ => { });
            if (!(EveryOneAreEqual()) || maxBet == 0)//(maxBet == 0 || !EveryOneAreEqual()||IsHappened)
                NextTurn(true);
        }
        protected override bool EveryOneIsNotRerazeing()
        {
            bool result = true;
            foreach (Player player in Players!)
                if (player != null && player.IsReRazed)
                    result = false;
            return result;
        }
        protected override void EndHand()
        {
            EndOfHand = false;
            IsPopupOpen = false;
            ChangeIsFoldedToFalse();
            for (int i = 0; i < Pot.Length; i++)
                Pot[i] = 0;
            RoundNumber = 0;
            CurrentPlayerIndex = 0;//FirstPlayerWhichIsNotOffLine();
            FillBoard();
            FillArrayAndAddCards(false, (t) => { });
            Dictionary<string, object> dict = new()
            {
                { nameof(CurrentPlayerIndex), CurrentPlayerIndex! },
                { nameof(beforeCurrentPlayerIndex), beforeCurrentPlayerIndex },
                { nameof(BoardCards), BoardCards },
                { nameof(RoundNumber), RoundNumber },
                { nameof(Players), Players! },
                { nameof(Pot), Pot! },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });

        }
        protected void EndOfRound(bool updateFB)
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
            CurrentPlayerIndex = 0;
            FillBoard();
            EndOfHand = RoundNumber == HandComplete;
            if (updateFB)
            {
                Dictionary<string, object> dict = new()
                {
                    { nameof(BoardCards), BoardCards },
                    { nameof(RoundNumber), RoundNumber },
                    { nameof(Players), Players! },
                    { nameof(CurrentPlayerIndex), CurrentPlayerIndex },
                };
                fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
            }
        }
        protected bool EveryOneAreEqual()
        {
            double maxBet = Players!.Max(p => p.CurrentBet);
            if (maxBet == 0) { return false; }
            foreach (Player player in Players!)
                if (!player.IsFolded && player.CurrentBet != maxBet)
                    return false;
            return true;
        }
        protected bool AllBetsZero()
        {
            foreach (Player player in Players!)
                if (player == null || !player.IsFolded && player.CurrentBet != 0)
                    return false;
            return true;
        }
        protected bool EveryOneIsAllIn()
        {
            foreach (Player player in Players!)
                if (player == null || !player.IsFolded && !player.IsAllIn)
                    return false;
            return true;
        }
        protected bool OnlyOneIsStayed()
        {
            foreach (Player player in Players!)
                if (player == null || !player.IsOut)
                    return false;
            return true;
        }
        protected override void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Console.WriteLine("Game OnChange called");
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                //IsHappened = RoundChanges&&CurrentPlayerIndex!=FirstPlayerWhichIsNotFold();
                bool changedToAllin = updatedGame.Players?[CurrentPlayerIndex] !=null&&
                    updatedGame.Players[CurrentPlayerIndex].CurrentMoney == updatedGame.Players[CurrentPlayerIndex].CurrentBet
                    && updatedGame.Players[updatedGame.CurrentPlayerIndex].CurrentBet!=0;
                bool currentPlayerIndexChange = CurrentPlayerIndex != updatedGame.CurrentPlayerIndex;

                bool isEndOfRound = CurrentPlayerIndex > FirstPlayerWhichIsNotOffLine() && updatedGame.CurrentPlayerIndex == FirstPlayerWhichIsNotOffLine();//&&BoardCards== updatedGame.BoardCards;//&& EveryOneAreEqual();//EveryOneIsNotRerazeing();
                Console.WriteLine("isEndOfRound:"+isEndOfRound);
                Console.WriteLine("CurrentPlayerIndex:"+ CurrentPlayerIndex);
                Console.WriteLine("updatedGame.CurrentPlayerIndex"+ updatedGame.CurrentPlayerIndex);
                bool changedToFull = CurrentNumOfPlayers < MaxNumOfPlayers && updatedGame.CurrentNumOfPlayers == MaxNumOfPlayers;
                bool isGameStarted = CurrentNumOfPlayers != updatedGame.CurrentNumOfPlayers && updatedGame.CurrentNumOfPlayers == MaxNumOfPlayers;
                bool isCheckOrCallChanged = (updatedGame.CurrentPlayer?.CurrentBet != CurrentPlayer?.CurrentBet);
                bool isHandEnded = false;
                bool ChangeTurn=CurrentPlayerIndex != updatedGame.CurrentPlayerIndex;
                string WinnerName = string.Empty;
                Dictionary<Player, HandRank> ranks = [];
                Player[] playersArray = null!;
                Players = updatedGame.Players;
                CurrentNumOfPlayers = updatedGame.CurrentNumOfPlayers;
                RoundNumber = updatedGame.RoundNumber;
                BoardCards = updatedGame.BoardCards;
                CurrentPlayerIndex = updatedGame.CurrentPlayerIndex;
                PlayerBeforeId = updatedGame.PlayerBeforeId;
                Pot = updatedGame.Pot;
                if (EveryOneIsAllIn() && IsHost && Pot.Sum() != 0)
                {
                    double sum = 0;
                    for (int i = 0; i <= RoundNumber; i++)
                        sum += Pot[i];
                    foreach (Player p in Players!)
                        p.SumOfMoneyThatThePlayerWon = (int)sum;
                    for (int i = RoundNumber; i < HandComplete + 1; i++)
                        BoardCards[i] = SetOfCards.GetRandomCard();
                    RoundNumber = HandComplete;
                    isEndOfRound = true;
                    //Dictionary<string, object> dict = new()
                    //{
                    //    { nameof(BoardCards), BoardCards },
                    //};
                    //fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
                }
                else if(CurrentPlayer!=null&&isEndOfRound&&CurrentPlayer.IsAllIn)
                {
                    double sum = 0;
                    int nonefoldedPlayers = 0;
                    foreach (Player p in Players!)
                        if (!p.IsFolded)
                            nonefoldedPlayers++;
                    for (int i = 0; i < RoundNumber; i++)
                        sum += Pot[i];
                    sum += CurrentPlayer!.CurrentBet * nonefoldedPlayers;
                    CurrentPlayer.SumOfMoneyThatThePlayerWon += (int)sum;
                    Console.WriteLine("isEndOfRound&&IsAllIn");
                }

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
                        Winner[0].CurrentMoney += Pot.Sum();
                        if (!IsPopupOpen)
                           {
                                OnwinnerSelected?.Invoke(this, new WinningPopupEvent(Winner, null!));
                                IsPopupOpen = true;
                           }
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
                        double sum = Pot.Sum();
                        for (int i = 0; i < playersArray.Length && sum > 0; i++)
                        {
                            Player curr = playersArray[i];
                            if (curr.IsAllIn)
                            {
                                double winAmount = Math.Min(sum, curr.SumOfMoneyThatThePlayerWon);
                                curr.CurrentMoney += (int)winAmount;
                                sum -= winAmount;
                            }
                            else
                            {
                                curr.CurrentMoney += (int)sum;
                                sum = 0;
                            }
                        }         
                        //Dictionary<string, object> dict = new()
                        //{
                        //    { nameof(Players), Players! },
                        //    { nameof(Pot), Pot },
                        //};
                        //fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
                        if (!IsPopupOpen)
                        {
                            OnwinnerSelected?.Invoke(this, new WinningPopupEvent(playersArray, ranks));
                            IsPopupOpen = true;
                        }
                    }
                    if (IsHost)
                    {
                        foreach (Player player in playersArray)
                            if (player.IsAllIn && player.CurrentMoney == 0)
                                player.IsOut = true;
                        EndHand();
                        isHandEnded = true;
                    }
                    if (isHandEnded)
                    {
                        IsPopupOpen=false;
                    }

                }
                if (CurrentPlayer != null && CurrentPlayer.IsOut)              
                    OnPlayerLost?.Invoke(this, EventArgs.Empty);

                //if(OnlyOneIsStayed())
                //{
                //    //להפעיל פופאפ ניצחון
                //}
                Console.WriteLine("ChangeTurn"+ ChangeTurn + "IsMyTurn"+ IsMyTurn+"");
             
                if (IsHost && changedToFull && !isHandEnded)
                    FillArrayAndAddCards(true, (t) => { });
                bool IsEndOfRound=false;
                bool IsFoldOrOut=false;
                if (IsFull && IsHost && EveryOneAreEqual() || IsHost && AllBetsZero() && isEndOfRound && !isHandEnded) //&& CurrentPlayer!.Id != Players![FirstPlayerWhichIsNotOffLine()].Id)                
                {
                    IsEndOfRound=true;
                    EndOfRound(false);
                }
                if (CurrentPlayer != null && IsMyTurn && (CurrentPlayer.IsFolded || CurrentPlayer.IsAllIn))
                {
                    IsFoldOrOut=true;
                    NextTurn(false); 
                }
                if(IsHost&& IsEndOfRound|| IsFoldOrOut)
                {
                    if(IsEndOfRound )
                    {
                        Dictionary<string, object> dict = new()
                        {
                            { nameof(BoardCards), BoardCards },
                            { nameof(RoundNumber), RoundNumber },
                            { nameof(Players), Players! },
                            { nameof(CurrentPlayerIndex), CurrentPlayerIndex },
                        };
                        fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
                    }
                    else if(IsFoldOrOut)
                    {
                        Dictionary<string, object> dict = new()
                        {
                            { nameof(CurrentPlayerIndex), CurrentPlayerIndex },
                        };
                        fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
                    }
                    IsFoldOrOut=false;
                    IsEndOfRound=false;

                }
                if (!TimerCreated)
                {
                    RegisterTimer();
                    TimerCreated = true;
                }

                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }            
        }
    }
}
