
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
        public override string CurrentStatus => IsFull ? Strings.PlayingStatus : CurrentPlayer!.Name;
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
 
        public override bool IsFull { get => CurrentNumOfPlayers == MaxNumOfPlayers; set => _ = CurrentNumOfPlayers == MaxNumOfPlayers; }
        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id, OnComplete);
        }
        protected override void FillBoard(int round)
        {
            if (round == 0)
            {
                BoardCards[0] = null!;
                BoardCards[1] = null!;
                BoardCards[2] = null!;
                BoardCards[3] = null!;
                BoardCards[4] = null!;
            }
            if (round == 1)
            {
                BoardCards[0] = SetOfCards.GetRandomCard();
                BoardCards[1] = SetOfCards.GetRandomCard();
                BoardCards[2] = SetOfCards.GetRandomCard();
            }
            else if (round == 2)
            {
                BoardCards[3] = SetOfCards.GetRandomCard();
            }
            else if (round == 3)
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
           // if (!(EveryOneAreEqual())) //|| maxBet == 0)//(maxBet == 0 || !EveryOneAreEqual()||IsHappened)
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
            SetOfCards.FillPakage();
            ChangeIsFoldedToFalse();
            for (int i = 0; i < Pot.Length; i++)
                Pot[i] = 0;
            //RoundNumber = 0;
            CurrentPlayerIndex = 0;//FirstPlayerWhichIsNotOffLine();
            FillBoard(0);
            FillArrayAndAddCards(false, (t) => { });
            Dictionary<string, object> dict = new()
            {
                { nameof(CurrentPlayerIndex), CurrentPlayerIndex! },
                { nameof(beforeCurrentPlayerIndex), beforeCurrentPlayerIndex },
                { nameof(BoardCards), BoardCards },
                { nameof(RoundNumber), 0 },
                { nameof(Players), Players! },
                { nameof(Pot), Pot! },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });

        }
        protected void EndOfRound(int round)
        {
            foreach (Player player in Players!)
            {
                Console.WriteLine($"Current index {CurrentPlayerIndex} Current Money {CurrentPlayer?.CurrentMoney} 1  ");
                player!.CurrentBet = 0;
                player.IsReRazed = false;
                Console.WriteLine($"Current index {CurrentPlayerIndex} Current Money {CurrentPlayer?.CurrentMoney} 2  ");
            }
            Console.WriteLine("Round Number Updated" + RoundNumber + "  ," + DateTime.Now);
            CurrentPlayerIndex = 0;
            FillBoard(round);
        }

        protected bool EveryOneIsAllIn()
        {
            foreach (Player player in Players!)
                if (player == null || !player.IsFolded && !player.IsAllIn)
                    return false;
            return true;
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
        protected int CountPlayerAllIn()
        {
            int i = 0;
            foreach (Player player in Players!)
                if (player == null || (!player.IsFolded && player.IsAllIn))
                    i++;
            return i;
        }
        protected bool OnlyOneIsStayed()
        {
            foreach (Player player in Players!)
                if (player == null || !player.IsOut)
                    return false;
            return true;
        }

        protected int CountInPlayers()
        {
            int count = 0;
            foreach (Player player in Players!)
                if (player != null && !player.IsOut)
                    count++;
            return count;
        }

        protected int CountActivePlayers()
        {
            int count = 0;
            foreach (Player player in Players!)
                if (player != null && !player.IsFolded && !player.IsOut)
                    count++;
            return count;
        }

        protected override void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();

            if (updatedGame == null)
            {
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
                return;
            }

            bool isEndOfRound = IsRoundEnding(updatedGame);
            bool changedToFull = HasGameBecomeFull(updatedGame);
            bool isGameStarted = HasGameJustStarted(updatedGame);
            bool isRoundChanged = RoundNumber != updatedGame.RoundNumber;
            bool isChangeToOneStaying = updatedGame.IsOneStaying() && !IsOneStaying();

            Console.WriteLine("Game OnChange called isEndOfRound: " + isEndOfRound + " isRoundChanged: " + isRoundChanged + " isChangeToOneStaying: " + isChangeToOneStaying);

            SyncGameState(updatedGame);

            int nextRound = HandleAllInScenarios(isEndOfRound);
            if (nextRound > 0)
            {
                isEndOfRound = true;
            }
            UpdateCheckOrCallUI();

            bool isHandEnded = false;
            Player[] playersArray = null!;

            if (isRoundChanged)
            {
                Console.WriteLine("Round changed "+ RoundNumber+ "IsHost:"+ IsHost);
            }

            if ((isRoundChanged || isChangeToOneStaying) && IsHandOver())
            {
                Console.WriteLine("rasing popup");
                Console.WriteLine("isLastRoundChanged:"+ isRoundChanged+ "isChangeToOneStaying:"+ isChangeToOneStaying + "IsHandOver:"+ IsHandOver());
                Console.WriteLine();
                playersArray = HandleHandEnd();
                isHandEnded = FinalizeHandIfHost(playersArray, updatedGame);
            }

            ProcessRoundAndTurnUpdates(isEndOfRound, isHandEnded, changedToFull, nextRound);
            EnsureTimerRegistered();

            OnGameChanged?.Invoke(this, EventArgs.Empty);
        }

        #region OnChange Helper Methods

        private bool IsRoundEnding(Game updatedGame)
        {
            int firstActivePlayer = FirstPlayerWhichIsNotOffLine();
            return CurrentPlayerIndex > firstActivePlayer && updatedGame.CurrentPlayerIndex == firstActivePlayer;
        }

        private bool HasGameBecomeFull(Game updatedGame)
        {
            return CurrentNumOfPlayers < MaxNumOfPlayers && updatedGame.CurrentNumOfPlayers == MaxNumOfPlayers;
        }

        private bool HasGameJustStarted(Game updatedGame)
        {
            return CurrentNumOfPlayers != updatedGame.CurrentNumOfPlayers && updatedGame.CurrentNumOfPlayers == MaxNumOfPlayers;
        }

        private void SyncGameState(Game updatedGame)
        {
            Players = updatedGame.Players;
            CurrentNumOfPlayers = updatedGame.CurrentNumOfPlayers;
            RoundNumber = updatedGame.RoundNumber;
            BoardCards = updatedGame.BoardCards;
            CurrentPlayerIndex = updatedGame.CurrentPlayerIndex;
            PlayerBeforeId = updatedGame.PlayerBeforeId;
            Pot = updatedGame.Pot;
        }

        private int HandleAllInScenarios(bool isEndOfRound)
        {
            bool allInWithPot = EveryOneIsAllIn() && IsHost && Pot.Sum() != 0;

            if (allInWithPot)
            {
                CompleteAllInShowdown();
                return HandComplete;
            }

            bool currentPlayerAllInAtRoundEnd = CurrentPlayer != null && isEndOfRound && CurrentPlayer.IsAllIn;
            if (currentPlayerAllInAtRoundEnd)
            {
                CalculateAllInPlayerWinnings();
            }

            return 0;
        }


        private void CompleteAllInShowdown()
        {
            double totalPot = CalculateTotalPot();

            foreach (Player p in Players!)
                p.SumOfMoneyThatThePlayerWon = (int)totalPot;

            for (int i = RoundNumber; i < HandComplete + 1; i++)
                BoardCards[i] = SetOfCards.GetRandomCard();

            //RoundNumber = HandComplete; 
        }

        private void CalculateAllInPlayerWinnings()
        {
            int activePlayers = Players!.Count(p => !p.IsFolded);
            double sum = CalculatePotUpToRound(RoundNumber - 1);
            sum += CurrentPlayer!.CurrentBet * activePlayers;
            CurrentPlayer.SumOfMoneyThatThePlayerWon += (int)sum;
        }

        private double CalculateTotalPot()
        {
            double sum = 0;
            for (int i = 0; i <= RoundNumber; i++)
                sum += Pot[i];
            return sum;
        }

        private double CalculatePotUpToRound(int round)
        {
            double sum = 0;
            for (int i = 0; i <= round; i++)
                sum += Pot[i];
            return sum;
        }

        private void UpdateCheckOrCallUI()
        {
            bool needToCall = IsFull && Players?[beforeCurrentPlayerIndex].CurrentBet > CurrentPlayer?.CurrentBet;

            if (needToCall)
            {
                double callAmount = Players![beforeCurrentPlayerIndex].CurrentBet - CurrentPlayer!.CurrentBet;
                CheckOrCall = Strings.Call + callAmount + "$";
                MinBet = CalculateMinBet();
            }
            else
            {
                CheckOrCall = Strings.Check;
                MinBet = 0;
            }

            OnCheckOrCallChanged?.Invoke(this, EventArgs.Empty);
        }

        private int CalculateMinBet()
        {
            double previousBet = Players![beforeCurrentPlayerIndex].CurrentBet;
            double playerMoney = CurrentPlayer!.CurrentMoney;
            int doublePreviousBet = (int)previousBet * 2;

            int minimalBet = doublePreviousBet > playerMoney ? (int)playerMoney : doublePreviousBet;
            return playerMoney > minimalBet ? minimalBet : (int)playerMoney;
        }

        private bool IsHandOver()
        {
            return (IsOneStaying() && IsFull) || RoundNumber == HandComplete;
        }

        private Player[] HandleHandEnd()
        {
            if (IsOneStaying())
            {
                return HandleLastPlayerWins();
            }
            return HandleShowdown();
        }

        private Player[] HandleLastPlayerWins()
        {
            Player winner = Players!.First(p => p != null && !p.IsFolded);
            winner.CurrentMoney += Pot.Sum();

            if (!IsPopupOpen)
            {
                OnwinnerSelected?.Invoke(this, new WinningPopupEvent([winner], null!));
                IsPopupOpen = true;
            }

            return [winner];
        }

        private Player[] HandleShowdown()
        {
            var ranks = EvaluatePlayerHands();
            var sortedPlayers = SortPlayersByHandRank(ranks);
            DistributePotToWinners(sortedPlayers);

            if (!IsPopupOpen && !EveryOneIsAllIn())
            {
                Console.WriteLine("Showing showdown popup");
                OnwinnerSelected?.Invoke(this, new WinningPopupEvent(sortedPlayers, ranks));
                IsPopupOpen = true;
            }

            return sortedPlayers;
        }

        private Dictionary<Player, HandRank> EvaluatePlayerHands()
        {
            var ranks = new Dictionary<Player, HandRank>();
            foreach (Player player in Players!)
            {
                if (player != null && !player.IsFolded)
                {
                    ranks.Add(player, player.EvaluateBestHand(BoardCards));
                }
            }
            return ranks;
        }

        private Player[] SortPlayersByHandRank(Dictionary<Player, HandRank> ranks)
        {
            var playersArray = ranks.Keys.ToArray();
            Array.Sort(playersArray, (p1, p2) => ranks[p2].Compare(ranks[p1]));
            return playersArray;
        }

        private void DistributePotToWinners(Player[] sortedPlayers)
        {
            double remainingPot = Pot.Sum();

            foreach (Player player in sortedPlayers)
            {
                if (remainingPot <= 0) break;

                if (player.IsAllIn)
                {
                    double winAmount = Math.Min(remainingPot, player.SumOfMoneyThatThePlayerWon);
                    player.CurrentMoney += (int)winAmount;
                    remainingPot -= winAmount;
                }
                else
                {
                    player.CurrentMoney += (int)remainingPot;
                    remainingPot = 0;
                }
            }
        }

        private bool banckruptPlayerExists()
        {
            foreach (Player player in Players!)
            {
                if (player != null && player.CurrentMoney == 0)
                    return true;
            }
            return false;
        }

        private bool FinalizeHandIfHost(Player[] playersArray, Game updatedGame)
        {
            if (IsHost && playersArray != null)
                MarkBankruptPlayersAsOut(playersArray);

            if (EveryOneIsAllIn() || banckruptPlayerExists())
            {
                NotifyAllInResults(updatedGame);
            }

            if (IsHost)
            {
                EndHand();
                IsPopupOpen = false;
                return true;
            }

            IsPopupOpen = false;
            return false;
        }

        private void MarkBankruptPlayersAsOut(Player[] players)
        {
            foreach (Player player in players)
            {
                if (player.IsAllIn && player.CurrentMoney == 0)
                    player.IsOut = true;
            }
        }

        private void NotifyAllInResults(Game updatedGame)
        {
            if (banckruptPlayerExists()){
                OnPlayerLost?.Invoke(this, EventArgs.Empty);
            }
            if (!EveryOneIsAllIn()) return;

            bool currentPlayerBankrupt = updatedGame.CurrentPlayer?.IsAllIn == true && updatedGame.CurrentPlayer.CurrentMoney == 0;

            Console.WriteLine(" updatedGame.CurrentPlayer?.IsAllIn" + updatedGame.CurrentPlayer?.IsAllIn+"  "+ "updatedGame.CurrentPlayer.CurrentMoney"+ updatedGame.CurrentPlayer.CurrentMoney);
            Console.WriteLine("2" + updatedGame.Players[1].CurrentMoney);
            Console.WriteLine("2" + updatedGame.Players[2].CurrentMoney);
            if (!currentPlayerBankrupt)           
                OnWinnerSelected?.Invoke(this, EventArgs.Empty);
            else
                OnPlayerLost?.Invoke(this, EventArgs.Empty);
            OnGameDeleted?.Invoke(this, EventArgs.Empty);
        }

        private void ProcessRoundAndTurnUpdates(bool isEndOfRound, bool isHandEnded, bool changedToFull, int nextRound)
        {
            if (IsHost && changedToFull && !isHandEnded)
                FillArrayAndAddCards(true, (t) => { });

            bool shouldEndRound = ShouldEndRound(isEndOfRound, isHandEnded);
            bool shouldSkipTurn = ShouldSkipCurrentPlayerTurn();

            int round = RoundNumber + 1;
            if (shouldEndRound) {
                if (nextRound > 0){
                    round = nextRound;
                }
                EndOfRound(round);
            }

            if (shouldSkipTurn)
                NextTurn(false);

            UpdateFirebaseIfNeeded(shouldEndRound, shouldSkipTurn, round);
        }

        private bool ShouldEndRound(bool isEndOfRound, bool isHandEnded)
        {
            if (isHandEnded) return false;

            bool allBetsEqual = IsFull && IsHost && EveryOneAreEqual();
            bool roundEndWithZeroBets = IsHost && AllBetsZero() && isEndOfRound;

            return allBetsEqual || roundEndWithZeroBets;
        }

        private bool ShouldSkipCurrentPlayerTurn()
        {
            if (CurrentPlayer == null || !IsMyTurn) return false;

            return CurrentPlayer.IsFolded || CurrentPlayer.IsAllIn || CurrentPlayer.IsOut;
        }

        private void UpdateFirebaseIfNeeded(bool endedRound, bool skippedTurn, int round)
        {
            if (!IsHost && !skippedTurn) return;

            if (endedRound && IsHost)
            {
                var dict = new Dictionary<string, object>
                {
                    { nameof(BoardCards), BoardCards },
                    { nameof(RoundNumber), round },
                    { nameof(Players), Players! },
                    { nameof(CurrentPlayerIndex), CurrentPlayerIndex },
                };
                fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
            }
            else if (skippedTurn)
            {
                var dict = new Dictionary<string, object>
                {
                    { nameof(CurrentPlayerIndex), CurrentPlayerIndex },
                };
                fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
            }
        }

        private void EnsureTimerRegistered()
        {
            if (TimerCreated) return;

            RegisterTimer();
            TimerCreated = true;
        }

        #endregion


    //    protected override void OnChange(IDocumentSnapshot? snapshot, Exception? error)
    //    {
    //        Console.WriteLine("Game OnChange called");
    //        Game? updatedGame = snapshot?.ToObject<Game>();
    //        if (updatedGame != null)
    //        {
    //            //IsHappened = RoundChanges&&CurrentPlayerIndex!=FirstPlayerWhichIsNotFold();
    //            bool changedToAllin = updatedGame.Players?[CurrentPlayerIndex] != null &&
    //                updatedGame.Players[CurrentPlayerIndex].CurrentMoney == updatedGame.Players[CurrentPlayerIndex].CurrentBet
    //                && updatedGame.Players[updatedGame.CurrentPlayerIndex].CurrentBet!=0;
    //            bool currentPlayerIndexChange = CurrentPlayerIndex != updatedGame.CurrentPlayerIndex;
    //            bool numAllInChanged = CountPlayerAllIn() != updatedGame.CountPlayerAllIn();
    //            bool isEndOfRound = CurrentPlayerIndex > FirstPlayerWhichIsNotOffLine() && updatedGame.CurrentPlayerIndex == FirstPlayerWhichIsNotOffLine();//&&BoardCards== updatedGame.BoardCards;//&& EveryOneAreEqual();//EveryOneIsNotRerazeing();
    //            Console.WriteLine("isEndOfRound:"+isEndOfRound);
    //            Console.WriteLine("CurrentPlayerIndex:"+ CurrentPlayerIndex);
    //            Console.WriteLine("updatedGame.CurrentPlayerIndex"+ updatedGame.CurrentPlayerIndex);
    //            bool changedToFull = CurrentNumOfPlayers < MaxNumOfPlayers && updatedGame.CurrentNumOfPlayers == MaxNumOfPlayers;
    //            bool isGameStarted = CurrentNumOfPlayers != updatedGame.CurrentNumOfPlayers && updatedGame.CurrentNumOfPlayers == MaxNumOfPlayers;
    //            bool isCheckOrCallChanged = (updatedGame.CurrentPlayer?.CurrentBet != CurrentPlayer?.CurrentBet);
    //            bool isPotChanged = Pot.Sum() != updatedGame.Pot.Sum();
    //            bool isHandEnded = false;
    //            bool ChangeTurn = CurrentPlayerIndex != updatedGame.CurrentPlayerIndex;
    //            bool isChangeRound = RoundNumber != updatedGame.RoundNumber;
    //            string WinnerName = string.Empty;
    //            Dictionary<Player, HandRank> ranks = [];
    //            Player[] playersArray = null!;
    //            bool isActivePlayerChanged = CountActivePlayers() != updatedGame.CountActivePlayers();
    //            bool OnePlayerRemainingChange = CountInPlayers() > 1 && updatedGame.CountInPlayers() == 1;
    //            Players = updatedGame.Players;
    //            CurrentNumOfPlayers = updatedGame.CurrentNumOfPlayers;
    //            RoundNumber = updatedGame.RoundNumber;
    //            BoardCards = updatedGame.BoardCards;
    //            CurrentPlayerIndex = updatedGame.CurrentPlayerIndex;
    //            PlayerBeforeId = updatedGame.PlayerBeforeId;
    //            Pot = updatedGame.Pot;


    //            if (numAllInChanged && CountPlayerAllIn() == CountActivePlayers() && IsHost && Pot.Sum() != 0)
    //            {
    //                double sum = 0;
    //                for (int i = 0; i <= RoundNumber; i++)
    //                    sum += Pot[i];
    //                foreach (Player p in Players!)
    //                    p.SumOfMoneyThatThePlayerWon = (int)sum;
    //                for (int i = RoundNumber; i < HandComplete + 1; i++)
    //                    BoardCards[i] = SetOfCards.GetRandomCard();
    //                RoundNumber = HandComplete;
    //                isEndOfRound = true;
    //                //Dictionary<string, object> dict = new()
    //                //{
    //                //    { nameof(BoardCards), BoardCards },
    //                //};
    //                //fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
    //            }
    //            else if(numAllInChanged && CurrentPlayer != null && isEndOfRound && CurrentPlayer.IsAllIn)
    //            {
    //                double sum = 0;
    //                int nonefoldedPlayers = 0;
    //                foreach (Player p in Players!)
    //                    if (!p.IsFolded)
    //                        nonefoldedPlayers++;
    //                for (int i = 0; i < RoundNumber; i++)
    //                    sum += Pot[i];
    //                sum += CurrentPlayer!.CurrentBet * nonefoldedPlayers;
    //                CurrentPlayer.SumOfMoneyThatThePlayerWon += (int)sum;
    //                Console.WriteLine("isEndOfRound&&IsAllIn");
    //            }

    //            if (isGameStarted)
    //                updatedGame.beforeCurrentPlayerIndex = MaxNumOfPlayers - 1;

    //            if (ChangeTurn && IsFull && Players?[beforeCurrentPlayerIndex].CurrentBet > CurrentPlayer?.CurrentBet)
    //            {
    //                CheckOrCall = Strings.Call + (Players[beforeCurrentPlayerIndex].CurrentBet - CurrentPlayer.CurrentBet) + "$";
    //                int minimalBet = (int)(Players[beforeCurrentPlayerIndex].CurrentBet) * 2 > CurrentPlayer.CurrentMoney ? (int)CurrentPlayer.CurrentMoney : ((int)(Players[beforeCurrentPlayerIndex].CurrentBet) * 2);
    //                MinBet = CurrentPlayer.CurrentMoney > minimalBet ? minimalBet : (int)CurrentPlayer.CurrentMoney;
    //                OnCheckOrCallChanged?.Invoke(this, EventArgs.Empty);
    //            }
    //            else if (ChangeTurn)
    //            {
    //                CheckOrCall = Strings.Check;
    //                MinBet = 0;
    //                OnCheckOrCallChanged?.Invoke(this, EventArgs.Empty);
    //            }

    //            if (((IsOneStaying() && isActivePlayerChanged) || (isChangeRound && RoundNumber == HandComplete)) && IsFull)
    //            {
    //                if (IsOneStaying())
    //                {
    //                    Player[] Winner = new Player[1];
    //                    foreach (Player player in Players!)
    //                        if (player != null && !player.IsFolded)
    //                            Winner[0] = player;
    //                    Winner[0].CurrentMoney += Pot.Sum();
    //                    OnwinnerSelected?.Invoke(this, new WinningPopupEvent(Winner, null!));
    //                }
    //                else
    //                {
    //                    foreach (Player player in Players!)
    //                    {
    //                        if (player != null && !player.IsFolded)
    //                        {
    //                            HandRank handRank = player.EvaluateBestHand(BoardCards);
    //                            ranks.Add(player, handRank);
    //                        }
    //                    }
    //                    playersArray = new Player[ranks.Count];
    //                    ranks.Keys.CopyTo(playersArray, 0);
    //                    Array.Sort(playersArray, (p1, p2) => ranks[p2].Compare(ranks[p1]));
    //                    double sum = Pot.Sum();
    //                    for (int i = 0; i < playersArray.Length && sum > 0; i++)
    //                    {
    //                        Player curr = playersArray[i];
    //                        if (curr.IsAllIn)
    //                        {
    //                            double winAmount = Math.Min(sum, curr.SumOfMoneyThatThePlayerWon);
    //                            curr.CurrentMoney += (int)winAmount;
    //                            sum -= winAmount;
    //                        }
    //                        else
    //                        {
    //                            curr.CurrentMoney += (int)sum;
    //                            sum = 0;
    //                        }
    //                    }
    //                    if (CountActivePlayers() == CountPlayerAllIn())
    //                    {
    //                        OnwinnerSelected?.Invoke(this, new WinningPopupEvent(playersArray, ranks));
    //                    }                  
    //                }
    //                if (IsHost)
    //                {
    //                    foreach (Player player in playersArray)
    //                        if (player.IsAllIn && player.CurrentMoney == 0)
    //                            player.IsOut = true;
    //                }
    //                //עושה את הבאג של הפופאפים מאיר
    //                 if (OnePlayerRemainingChange && !CurrentPlayer.IsOut)
    //                    OnWinnerSelected?.Invoke(this, EventArgs.Empty);
    //                else if (OnePlayerRemainingChange)
    //                    OnPlayerLost?.Invoke(this, EventArgs.Empty);

    //                if (IsHost)
    //                {
    //                    EndHand();
    //                    isHandEnded = true;
    //                }             
    //            }
             
    //            if (IsHost && changedToFull && !isHandEnded)
    //                FillArrayAndAddCards(true, (t) => { });


    //            bool updateFB = false;
    //            if (IsHost && IsFull && (EveryOneAreEqual() && isPotChanged) || (AllBetsZero() && isEndOfRound && !isHandEnded)) //&& CurrentPlayer!.Id != Players![FirstPlayerWhichIsNotOffLine()].Id)                
    //            {
    //                updateFB = true;
    //                EndOfRound(false);
    //            }
                 
    //            if (CurrentPlayer != null && ChangeTurn && IsMyTurn && (CurrentPlayer.IsFolded || CurrentPlayer.IsAllIn || CurrentPlayer.IsOut))
    //            {
    //                updateFB = true;
    //                NextTurn(false); 
    //            }


    //            if(updateFB)
    //            {
    //                Dictionary<string, object> dict = new()
    //                        {
    //                            { nameof(BoardCards), BoardCards },
    //                            { nameof(RoundNumber), RoundNumber },
    //                            { nameof(Players), Players! },
    //                            { nameof(CurrentPlayerIndex), CurrentPlayerIndex },
    //                        };
    //                fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
    //            }
    //            if (!TimerCreated)
    //            {
    //                RegisterTimer();
    //                TimerCreated = true;
    //            }
    //            OnGameChanged?.Invoke(this, EventArgs.Empty);
    //        }
    //        else
    //        {
    //            OnGameDeleted?.Invoke(this, EventArgs.Empty);
    //        }
    //    }
    }
}
