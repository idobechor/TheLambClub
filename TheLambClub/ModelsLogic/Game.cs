using CommunityToolkit.Mvvm.Messaging;
using Plugin.CloudFirestore;
using System.Collections.ObjectModel;
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    public class Game : GameModel
    {
        #region Properties
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
        public override bool IsFull { get => CurrentNumOfPlayers == MaxNumOfPlayers; set => _ = CurrentNumOfPlayers == MaxNumOfPlayers; }
        public override string CurrentStatus => IsFull ? Strings.CurrentTurnTxt + Players![CurrentPlayerIndex]!.Name : Strings.WaitingForPlayers;
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
        #endregion

        #region Constructor
        public Game() { }
        public Game(int selectedNumberOfPlayers)
        {
            HostName = new User().UserName;
            Created = DateTime.Now;
            NumberOfPlayers = selectedNumberOfPlayers;
            CurrentNumOfPlayers = 1;
            MaxNumOfPlayers = selectedNumberOfPlayers;
            CurrentPlayerIndex = 0;
        }
        #endregion

        #region Public Methods
        public override void PickedFold()
        {
            CurrentPlayer!.IsFolded = true;
            if (!IsOneStaying())
            {
                Dictionary<string, object> update = new()
            {
                { nameof(CurrentPlayerIndex), (CurrentPlayerIndex + 1) % CurrentNumOfPlayers },
                { nameof(Players), Players! },
            };
                fbd.UpdateFields(Keys.GamesCollection, Id, update, _ => { });
            }
            else
            {
                Dictionary<string, object> update = new()
                {

                    { nameof(Players), Players! },
                };
                fbd.UpdateFields(Keys.GamesCollection, Id, update, _ => { });
            }
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
        public override void UpdateGuestUser(Action<Task> OnComplete)
        {
            bool alreadyInGame = false;
            foreach (Player player in Players!)
                if (player != null && player!.Id == fbd.UserId)
                {
                    alreadyInGame = true;
                    break;
                }
            if (!alreadyInGame)
            {
                Player newPlayer = new(MyName, fbd.UserId);
                Players[CurrentNumOfPlayers] = newPlayer;
                CurrentNumOfPlayers++;
                UpdateFireBaseJoinGame(OnComplete);
            }
        }
        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id, OnComplete);
        }
        public override void BetFunction(object obj)
        {
            int pot = 0;
            Player prevPlayer = Players![PreviousPlayerIndex()];
            CurrentPlayer!.CurrentMoney -= CurrentPlayer!.CurrentBet;
            pot += (int)CurrentPlayer.CurrentBet + Pot;
            if (CurrentPlayer.CurrentMoney == 0)
                CurrentPlayer!.IsAllIn = true;
            if (!EveryOneAreEqual())
            {
                Dictionary<string, object> update = new()
            {
                { nameof(CurrentPlayerIndex), (CurrentPlayerIndex + 1) % CurrentNumOfPlayers },
                { nameof(Players), Players! },
                { nameof(Pot), pot }
            };
                fbd.UpdateFields(Keys.GamesCollection, Id, update, _ => { });
            }
            else
            {
                Dictionary<string, object> update = new()
                {
                    { nameof(Players), Players! },
                    { nameof(Pot), pot }
                };
                fbd.UpdateFields(Keys.GamesCollection, Id, update, _ => { });
            }
        }
        public override void CallFunction()
        {
            double maxBet = Players!.Max(p => p.CurrentBet);
            double moneyToCall = Math.Abs(maxBet - CurrentPlayer!.CurrentBet);
            CurrentPlayer!.CurrentBet = maxBet;
            CurrentPlayer.CurrentMoney -= moneyToCall;
            int pot = (int)moneyToCall + Pot;
            if (CurrentPlayer.CurrentMoney == 0)
                CurrentPlayer!.IsAllIn = true;
            if (!EveryOneAreEqual())
            {
                Dictionary<string, object> update = new()
                {
                    { nameof(CurrentPlayerIndex), (CurrentPlayerIndex + 1) % CurrentNumOfPlayers },
                    { nameof(Players), Players! },
                    { nameof(Pot), pot }
                };
                fbd.UpdateFields(Keys.GamesCollection, Id, update, _ => { });
            }
            else
            {
                Dictionary<string, object> update = new()
             {
                 { nameof(Players), Players! },
                 { nameof(Pot), pot }
             };
                fbd.UpdateFields(Keys.GamesCollection, Id, update, _ => { });
            }

        }
        public override int PreviousPlayerIndex()
        {
            int previousIndex = CurrentPlayerIndex - 1;
            while (previousIndex != CurrentPlayerIndex)
            {
                if (previousIndex < 0 && CurrentNumOfPlayers - 1 < Players!.Length)
                    previousIndex = CurrentNumOfPlayers - 1;
                if (!Players![previousIndex].IsFolded)
                    return previousIndex;
                previousIndex--;
            }
            return 0;
        }
        public void DisplayOponnentsNames(List<Label> lstOponnentsLabels)
        {
            int lblIndex = 0;
            for (int i = 0; i < CurrentNumOfPlayers; i++)
                if (Players![i] != null && CurrentPlayer!.Id != Players[i].Id)
                {
                    lstOponnentsLabels[lblIndex].Text = Players[i].Name;
                    lstOponnentsLabels[lblIndex++].BackgroundColor = Colors.Red;
                }
        }
        public void UpdateMoney(List<Label> lstOponnentsLabels, List<Label> lstOponnentsMoneyLabels)
        {
            for (int i = 0; i < lstOponnentsMoneyLabels.Count; i++)
                if (i < CurrentNumOfPlayers && lstOponnentsLabels[i].Text == Players![PreviousPlayerIndex()]!.Name)
                    lstOponnentsMoneyLabels[i].Text = Players![PreviousPlayerIndex()].CurrentMoney.ToString();
        }
        #endregion

        #region Protected Methods
        protected override void ChangeIsFoldedToFalse()
        {
            foreach (Player player in Players!)
            {
                player.IsFolded = false;
                player.IsAllIn = false;
                player.CurrentBet = 0;
            }
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
                PickedFold();
            TimeLeftChanged?.Invoke(this, EventArgs.Empty);
        }
        protected override void FillArrayAndAddCards(bool upDateFB, Action<Task> OnComplete)
        {
            foreach (Player item in Players!)
                if (item != null)
                {
                    item.FBCard1 = SetOfCards.GetRandomCard();
                    item.FBCard2 = SetOfCards.GetRandomCard();
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
        protected override void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
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
                BoardCards[3] = SetOfCards.GetRandomCard();
            else if (round == 3)
                BoardCards[4] = SetOfCards.GetRandomCard();
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
            Pot = 0;
            FillBoard(0);
            FillArrayAndAddCards(false, (t) => { });
        }
        protected override void EndOfRound(int round)
        {
            foreach (Player player in Players!)
            {
                player!.CurrentBet = 0;
                player.IsReRazed = false;
            }
            FillBoard(round);
        }
        protected override bool AnyOneIsAllIn()
        {
            bool r = false;
            foreach (Player p in Players!)
            {
                if (!p.IsFolded && p.IsAllIn)
                {
                    r = true;
                    break;
                }
            }
            return r;
        }
        protected override bool EveryOneAreEqual()
        {
            bool r = false;
            if (Players != null && !Players.Any(p => p == null))
            {
                double m = Players.Max(p => p.CurrentBet);
                if (m != 0)
                {
                    r = true;
                    foreach (Player p in Players)
                        if (!p.IsFolded && p.CurrentBet != m)
                        {
                            r = false;
                            break;
                        }
                }
            }
            return r;
        }
        protected override bool AllBetsZero()
        {
            bool r = true;
            foreach (Player p in Players!)
                if (p != null && !p.IsFolded && p.CurrentBet != 0)
                {
                    r = false;
                    break;
                }
            return r;
        }
        protected override bool IsRoundEnding(Game updatedGame) => CurrentPlayerIndex == MaxNumOfPlayers - 1 && updatedGame.CurrentPlayerIndex == 0;
        protected override bool HasGameBecomeFull(Game updatedGame) => CurrentNumOfPlayers < MaxNumOfPlayers && updatedGame.CurrentNumOfPlayers == MaxNumOfPlayers;
        protected override bool HasGameJustStarted(Game updatedGame) => CurrentNumOfPlayers != updatedGame.CurrentNumOfPlayers && updatedGame.CurrentNumOfPlayers == MaxNumOfPlayers;
        protected override void SyncGameState(Game updatedGame)
        {
            Players = updatedGame.Players;
            CurrentNumOfPlayers = updatedGame.CurrentNumOfPlayers;
            RoundNumber = updatedGame.RoundNumber;
            BoardCards = updatedGame.BoardCards;
            CurrentPlayerIndex = updatedGame.CurrentPlayerIndex;
            Pot = updatedGame.Pot;
        }
        protected override int HandleAllInScenarios()
        {
            int round = 0;
            if (AnyOneIsAllIn())
            {
                _ = CalculateTotalPot();
                for (int i = RoundNumber; i < HandComplete + 1; i++)
                    BoardCards[i] = SetOfCards.GetRandomCard();
                round = HandComplete;
            }
            return round;
        }
        protected override double CalculateTotalPot()
        {
            double sum = 0;
            sum += Pot;
            return sum;
        }
        protected override void UpdateCheckOrCallUI()
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
        protected override int CalculateMinBet()
        {
            double previousBet = Players![PreviousPlayerIndex()].CurrentBet;
            double playerMoney = CurrentPlayer!.CurrentMoney;
            int doublePreviousBet = (int)previousBet * 2;
            int minimalBet = doublePreviousBet > playerMoney ? (int)playerMoney : doublePreviousBet;
            minimalBet = Math.Min(minimalBet, MaxBet);
            return playerMoney > minimalBet ? minimalBet : (int)playerMoney;
        }
        protected override bool IsHandOver() => (IsOneStaying() && IsFull) || RoundNumber == HandComplete;
        protected override Player[] HandleHandEnd() => IsOneStaying() ? HandleLastPlayerWins() : HandleShowdown();
        protected override Player[] HandleLastPlayerWins()
        {
            Player winner = Players!.First(p => p != null && !p.IsFolded);
            winner.CurrentMoney += Pot;
            OnwinnerSelected?.Invoke(this, new WinningPopupEvent([winner], null!, 0));
            return [winner];
        }
        protected override Player[] HandleShowdown()
        {
            Dictionary<Player, HandRank> ranks = EvaluatePlayerHands();
            Player[] sortedPlayers = SortPlayersByHandRank(ranks);
            DistributePotToWinners(sortedPlayers, ranks);
            bool found = CheckForGameOver();
            if (found)
            {
                if (AmIWinner())
                    OnWinnerSelected?.Invoke(this, EventArgs.Empty);
                else
                    OnPlayerLost?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                int i = 1;
                for (int j = 0; j < sortedPlayers.Length - 1; j++)
                {
                    if (ranks[sortedPlayers[j]].Equals(ranks[sortedPlayers[j + 1]]))
                        i++;
                    else
                        break;
                }
                OnwinnerSelected?.Invoke(this, new WinningPopupEvent(sortedPlayers, ranks, i));
            }
            return sortedPlayers;
        }
        protected override bool AmIWinner()
        {
            double max = Players!.Max(p => p.CurrentMoney);
            return CurrentPlayer!.CurrentMoney == max;
        }
        protected override bool CheckForGameOver()
        {
            bool isGameOver = false;
            foreach (Player player in Players!)
                if (player.CurrentMoney == 0)
                {
                    isGameOver = true;
                    break;
                }
            return isGameOver;
        }
        protected override Dictionary<Player, HandRank> EvaluatePlayerHands()
        {
            Dictionary<Player, HandRank> ranks = [];
            foreach (Player player in Players!)
                if (player != null && !player.IsFolded)
                    ranks.Add(player, player.EvaluateBestHand(BoardCards));
            return ranks;
        }
        protected override Player[] SortPlayersByHandRank(Dictionary<Player, HandRank> ranks)
        {
            Player[] playersArray = [.. ranks.Keys];
            Array.Sort(playersArray, (p1, p2) => ranks[p2].Compare(ranks[p1]));
            return playersArray;
        }
        protected override void DistributePotToWinners(Player[] sortedPlayers, Dictionary<Player, HandRank> Dict)
        {
            double remainingPot = Pot;
            int countEven = 1;
            for (int i = 0; i < sortedPlayers.Length - 1; i++)
                if (Dict[sortedPlayers[i]].Equals(Dict[sortedPlayers[i + 1]]))
                    countEven++;
            for (int i = 0; i < countEven; i++)
                sortedPlayers[i].CurrentMoney += (int)remainingPot / countEven;
        }
        protected override bool FinalizeHandIfHost()
        {
            if (IsHost)
                EndHand();
            return IsHost;
        }
        protected override void ProcessRoundAndTurnUpdates(bool isEndOfRound, bool isHandEnded, bool changedToFull, int nextRound)
        {
            if (IsHost && changedToFull && !isHandEnded)
                FillArrayAndAddCards(true, (t) => { });
            bool shouldEndRound = ShouldEndRound(isEndOfRound, isHandEnded);
            bool shouldSkipTurn = ShouldSkipCurrentPlayerTurn() && !isHandEnded;
            int round = RoundNumber;
            if (shouldEndRound)
            {
                round = RoundNumber + 1;
                if (nextRound > 0)
                    round = nextRound;
                EndOfRound(round);
            }
            UpdateFirebaseIfNeeded(shouldEndRound, shouldSkipTurn, round, isHandEnded);
        }
        protected override bool ShouldEndRound(bool isEndOfRound, bool isHandEnded)
        {
            bool res = false;
            if (!isHandEnded)
            {
                bool allBetsEqual = IsFull && IsHost && EveryOneAreEqual();
                bool roundEndWithZeroBets = IsHost && AllBetsZero() && isEndOfRound;
                res = allBetsEqual || roundEndWithZeroBets;
            }
            return res;
        }
        protected override void UpdateFirebaseIfNeeded(bool endedRound, bool skippedTurn, int round, bool isEndOfHand)
        {
            if (!(!IsHost && !skippedTurn))
            {
                if (endedRound && IsHost && !isEndOfHand)
                {
                    Dictionary<string, object> dict = new()
                    {
                        { nameof(BoardCards), BoardCards },
                        { nameof(RoundNumber), round },
                        { nameof(Players), Players! },
                        { nameof(CurrentPlayerIndex), 0 },
                    };
                    fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
                }
                else if (IsHost && isEndOfHand)
                {
                    Dictionary<string, object> dict = new()
                    {
                        { nameof(CurrentPlayerIndex), 0! },
                        { nameof(BoardCards), BoardCards },
                        { nameof(RoundNumber),0  },
                        { nameof(Players), Players! },
                        { nameof(Pot), 0 },
                    };
                    fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
                }
                else if (skippedTurn)
                {
                    Dictionary<string, object> dict = new()
                    {
                        { nameof(CurrentPlayerIndex), (CurrentPlayerIndex + 1) % CurrentNumOfPlayers },
                    };
                    fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
                }
            }
        }
        protected override void EnsureTimerRegistered()
        {
            if (!TimerCreated)
            {
                RegisterTimer();
                TimerCreated = true;
            }
        }
        protected override void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                bool isChangeToOneStaying = updatedGame.IsOneStaying() && !IsOneStaying();
                bool isPotMoneyChanged = updatedGame.Pot != Pot;
                bool changedToFull = HasGameBecomeFull(updatedGame);
                bool isGameStarted = HasGameJustStarted(updatedGame);
                bool ChangedToEveryOneAreEqual = !EveryOneAreEqual() && updatedGame.EveryOneAreEqual();
                bool isRoundChanged = RoundNumber != updatedGame.RoundNumber;
                bool isEndOfRound = IsRoundEnding(updatedGame) && !isRoundChanged && !(isChangeToOneStaying);
                SyncGameState(updatedGame);
                int nextRound = 0;
                if (isPotMoneyChanged && EveryOneAreEqual())
                    nextRound = HandleAllInScenarios();
                UpdateCheckOrCallUI();
                bool isHandEnded = false;
                Player[] playersArray = null!;
                if (IsHandOver())
                {
                    playersArray = HandleHandEnd();
                    isHandEnded = FinalizeHandIfHost();
                  
                }
                ProcessRoundAndTurnUpdates(isEndOfRound, isHandEnded, changedToFull, nextRound);
                EnsureTimerRegistered();
                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
            else
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}