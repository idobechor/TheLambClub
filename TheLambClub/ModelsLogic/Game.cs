
using CommunityToolkit.Mvvm.Messaging;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
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
                OnTurnChanged?.Invoke(this, EventArgs.Empty);
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
        public override string CurrentStatus => IsFull ? "Current Player:"+Players![CurrentPlayerIndex]!.Name : Strings.WaitingForPlayers;
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

        public override void PickedFold()
        {
            CurrentPlayer!.IsFolded = true;
            Dictionary<string, object> dict = new()
            {
                { nameof(Players), Players! },
                { nameof(CurrentPlayerIndex), (CurrentPlayerIndex + 1) % CurrentNumOfPlayers }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
        }
        protected override void ChangeIsFoldedToFalse()
        {
            foreach (Player player in Players!)
            {
                player.IsFolded = false;
                player.IsAllIn = false;
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
                { nameof(CurrentPlayerIndex), (CurrentPlayerIndex + 1) % CurrentNumOfPlayers },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }
 
        public override bool IsFull { get => CurrentNumOfPlayers == MaxNumOfPlayers; set => _ = CurrentNumOfPlayers == MaxNumOfPlayers; }
        [Ignored]
        public int MaxBet { 
            get {
                return (int) Players!.Min(p => p.IsFolded == false ? p.CurrentMoney : 10000);
            } 
        }

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
        protected int FirstPlayerWhichIsNotFolded()
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

        public override void BetFunction(object obj)
        {
            Player prevPlayer = Players![PreviousPlayerIndex()];
            CurrentPlayer!.CurrentMoney -= CurrentPlayer!.CurrentBet;
            Pot[RoundNumber] += CurrentPlayer.CurrentBet;
            if (CurrentPlayer.CurrentMoney == 0)
            {
                CurrentPlayer!.IsAllIn = true;
            }
            MoneyChanged?.Invoke(this, new ChangingMoneyEvent(CurrentPlayer.Name,(int)CurrentPlayer.CurrentMoney)) ;
            Dictionary<string, object> update = new()
            {
                { nameof(CurrentPlayerIndex), (CurrentPlayerIndex + 1) % CurrentNumOfPlayers },
                { nameof(Players), Players },
                { nameof(Pot), Pot }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, update, _ => { });
        }

        public override void CallFunction()
        {
            double maxBet = Players!.Max(p => p.CurrentBet);
            double moneyToCall = Math.Abs(maxBet - CurrentPlayer!.CurrentBet);
            CurrentPlayer!.CurrentBet = maxBet;
            CurrentPlayer.CurrentMoney -= moneyToCall;
            Pot[RoundNumber] += moneyToCall;
            if (CurrentPlayer.CurrentMoney==0)
            {
                CurrentPlayer!.IsAllIn = true;
            }
            MoneyChanged?.Invoke(this, new ChangingMoneyEvent(CurrentPlayer.Name, (int)CurrentPlayer.CurrentMoney));
            Dictionary<string, object> update = new()
            {
                { nameof(CurrentPlayerIndex), (CurrentPlayerIndex + 1) % CurrentNumOfPlayers },
                { nameof(Players), Players! },
                { nameof(Pot), Pot }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, update, _ => { });
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
            FillBoard(0);
            FillArrayAndAddCards(false, (t) => { });
            Dictionary<string, object> dict = new()
            {
                { nameof(CurrentPlayerIndex), 0! },
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
            FillBoard(round);
        }

        protected bool AnyOneIsAllIn()
        {
            foreach (Player player in Players!)
                if (!player.IsFolded && player.IsAllIn)
                    return true;
            return false;
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
                if (player != null && !player.IsFolded && player.CurrentBet != 0)
                    return false;
            return true;
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


            Console.WriteLine("CurrentPlayerIndex " + CurrentPlayerIndex);
            Console.WriteLine("Game OnChange called isEndOfRound: " + isEndOfRound + " isRoundChanged: " + isRoundChanged + " isChangeToOneStaying: " + isChangeToOneStaying);

            SyncGameState(updatedGame);

            int nextRound = 0;
            if (isEndOfRound)
            {
                nextRound = HandleAllInScenarios();
                if (nextRound > 0)
                {
                    isEndOfRound = true;
                }
            }

            UpdateCheckOrCallUI();

            bool isHandEnded = false;
            Player[] playersArray = null!;

            if (IsHandOver())
            {
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
            Console.WriteLine("CurrentPlayerIndex"+ CurrentPlayerIndex);
            Console.WriteLine("updatedGame.CurrentPlayerIndex"+ updatedGame.CurrentPlayerIndex);
            return CurrentPlayerIndex > 0 && updatedGame.CurrentPlayerIndex == 0;
            
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

        private int PreviousPlayerIndex()
        {
            int previousIndex = CurrentPlayerIndex - 1;

            while (previousIndex != CurrentPlayerIndex)
            {
                if (previousIndex < 0){
                    previousIndex = CurrentNumOfPlayers - 1;
                }
                if (!Players![previousIndex].IsFolded)
                {
                    return previousIndex;
                }
                previousIndex--;
            }
            return -1;
        }

        private int HandleAllInScenarios()
        {
            if (AnyOneIsAllIn())
            {
                double totalPot = CalculateTotalPot();

                for (int i = RoundNumber; i < HandComplete + 1; i++)
                    BoardCards[i] = SetOfCards.GetRandomCard();

                return HandComplete;
            }
            return 0;
        }


        private double CalculateTotalPot()
        {
            double sum = 0;
            for (int i = 0; i <= RoundNumber; i++)
                sum += Pot[i];
            return sum;
        }

        private void UpdateCheckOrCallUI()
        {
            bool needToCall = IsFull && Players?[PreviousPlayerIndex()].CurrentBet > CurrentPlayer?.CurrentBet;

            if (needToCall)
            {
                double callAmount = Players![PreviousPlayerIndex()].CurrentBet - CurrentPlayer!.CurrentBet;
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
            double previousBet = Players![PreviousPlayerIndex()].CurrentBet;
            double playerMoney = CurrentPlayer!.CurrentMoney;
            int doublePreviousBet = (int)previousBet * 2;

            int minimalBet = doublePreviousBet > playerMoney ? (int)playerMoney : doublePreviousBet;
            minimalBet = Math.Min(minimalBet, MaxBet);
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
            OnwinnerSelected?.Invoke(this, new WinningPopupEvent([winner], null!,0));
            return [winner];
        }

        private Player[] HandleShowdown()
        {
            var ranks = EvaluatePlayerHands();
            var sortedPlayers = SortPlayersByHandRank(ranks);
            DistributePotToWinners(sortedPlayers,ranks);
            bool found = CheckForGameOver();
            if (found)
            {
                if (AmIWinner())
                    OnWinnerSelected?.Invoke(this, EventArgs.Empty);
                else
                    OnPlayerLost?.Invoke(this, EventArgs.Empty);
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                int i =1;
               for (int j = 0; j < sortedPlayers.Length-1; j++)
                {
                    if (ranks[sortedPlayers[j]].Equals(ranks[sortedPlayers[j + 1]]))
                    {
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
                OnwinnerSelected?.Invoke(this, new WinningPopupEvent(sortedPlayers, ranks,i));
            }
            return sortedPlayers;
        }

        private bool AmIWinner()
        {
            double max = Players!.Max(p => p.CurrentMoney);
            return CurrentPlayer!.CurrentMoney == max;
        }

        private bool CheckForGameOver()
        {
            foreach (Player player in Players!)
            {
                if (player.CurrentMoney == 0)
                {
                    return true;
                }
            }
            return false;
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

        private void DistributePotToWinners(Player[] sortedPlayers, Dictionary<Player,HandRank>Dict)
        {
            double remainingPot = Pot.Sum();
            int countEven = 1;
            for (int i = 0; i < sortedPlayers.Length-1; i++) 
            {
                if (Dict[sortedPlayers[i]].Equals(Dict[sortedPlayers[i + 1]]))
                {
                    countEven++;
                }
            }
            for (int i = 0;i < countEven; i++)
            {
                sortedPlayers[i].CurrentMoney += (int)remainingPot/ countEven;
            }
        }
        private bool FinalizeHandIfHost(Player[]? playersArray, Game ? updatedGame)
        {
            if (IsHost)
            {
                EndHand();
                return true;
            }

            return false;
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

            UpdateFirebaseIfNeeded(shouldEndRound, shouldSkipTurn, round);
        }

        private bool ShouldEndRound(bool isEndOfRound, bool isHandEnded)
        {
            if (isHandEnded) return false;

            bool allBetsEqual = IsFull && IsHost && EveryOneAreEqual();
            bool roundEndWithZeroBets = IsHost && AllBetsZero() && isEndOfRound;

            Console.WriteLine("allBetsEqual " + allBetsEqual + " roundEndWithZeroBets " + roundEndWithZeroBets);

            return allBetsEqual || roundEndWithZeroBets;
        }

        private bool ShouldSkipCurrentPlayerTurn()
        {
            if (CurrentPlayer == null || !IsMyTurn) return false;

            return CurrentPlayer.IsFolded || CurrentPlayer.IsAllIn;
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
                    { nameof(CurrentPlayerIndex), 0 },
                };
                fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
            }
            else if (skippedTurn)
            {
                var dict = new Dictionary<string, object>
                {
                    { nameof(CurrentPlayerIndex), (CurrentPlayerIndex + 1) % CurrentNumOfPlayers },
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

    }
}
