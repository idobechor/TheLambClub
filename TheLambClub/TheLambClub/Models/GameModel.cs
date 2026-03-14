using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System.Collections.ObjectModel;
using TheLambClub.ModelsLogic;
namespace TheLambClub.Models
{
    public abstract class GameModel
    {
        #region fields

        [Ignored]
        protected IListenerRegistration? ilr;
        [Ignored]
        protected FbData fbd = new();
        [Ignored]
        protected int _currentPlayerIndex;
        [Ignored]
        protected bool TimerCreated = false;
        [Ignored]
        protected const int HandComplete = 4;
        [Ignored]
        public TimerSettings timerSettings = new(Keys.TimerTotalTime, Keys.TimerInterval);
        public int Pot = 0;
        public string PlayerBeforeId = string.Empty;
        [Ignored]
        public bool IsHappened = false;

        #endregion

        #region events

        [Ignored]
        public EventHandler<WinningPopupEvent>? OnwinnerSelected;
        [Ignored]
        public EventHandler<ChangingMoneyEvent>? MoneyChanged;
        [Ignored]
        public EventHandler? TimeLeftChanged;
        [Ignored]
        public EventHandler? TimeLeftFinished;
        [Ignored]
        public EventHandler? OpenMyTurnPopUp;
        [Ignored]
        public EventHandler? OnTurnChanged;
        [Ignored]
        public EventHandler? OnGameDeleted;
        [Ignored]
        public EventHandler? OnPlayerLost;
        [Ignored]
        public EventHandler? OnWinnerSelected;
        [Ignored]
        public EventHandler? OnGameChanged;
        [Ignored]
        public EventHandler? OnCheckOrCallChanged;
        [Ignored]
        public EventHandler<string>? OnMyMoneyChanged;

        #endregion

        #region properties

        [Ignored]
        public abstract Player? CurrentPlayer { get; }
        [Ignored]
        public abstract string CurrentStatus { get; }
        [Ignored]
        protected SetOfCards SetOfCards { get; } = new SetOfCards();
        [Ignored]
        public string TimeLeft { get; protected set; } = string.Empty;
        [Ignored]
        public int MaxBet => (int)Players!.Min(p => p.IsFolded == false ? p.CurrentMoney : 10000);
        public FBCard[] BoardCards { get; set; } = new FBCard[5];
        public int RoundNumber { get; set; }
        public abstract int CurrentPlayerIndex { get; set; }
        public string HostName { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public int MaxNumOfPlayers { get; set; }
        public int CurrentNumOfPlayers { get; set; } = 1;
        [Ignored]
        public int MinBet { get; set; }
        public abstract bool IsFull { get; set; }
        [Ignored]
        public string Id { get; set; } = string.Empty;
        [Ignored]
        public string MyName { get; set; } = new User().UserName;
        public string? HostId { get; set; } = string.Empty;
        [Ignored]
        public abstract bool IsHost { get; }
        [Ignored]
        public string NumOfPlayersName => $"{MaxNumOfPlayers }";
        [Ignored]
        public int? NumberOfPlayers { get; set; }
        [Ignored]
        public abstract bool IsMyTurn { get; }
        [Ignored]
        public bool CanICheck { get; set; } = true;
        [Ignored]
        public string CheckOrCall { get; set; } = Strings.Check;
        public Player[]? Players { get; set; }
        [Ignored]
        public abstract ObservableCollection<ViewCard>? BoardViewCards { get; }
        [Ignored]
        public abstract ViewCard? ViewCard1 { get; }
        [Ignored]
        public abstract ViewCard? ViewCard2 { get; }

        #endregion

        #region public methods

        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void AddSnapShotListener();
        public abstract void RemoveSnapShotListener();
        public abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void PickedFold();
        public abstract void UpdateGuestUser(Action<Task> OnComplete);
        public abstract void CallFunction();
        public abstract void BetFunction(object obj);
        public abstract int PreviousPlayerIndex();

        #endregion

        #region protected methods

        protected abstract void OnComplete(Task task);
        protected abstract void UpdateFireBaseJoinGame(Action<Task> OnComplete);
        protected abstract void FillBoard(int round);
        protected abstract void OnChange(IDocumentSnapshot? snapshot, Exception? error);
        protected abstract void FillArrayAndAddCards(bool upDateFB, Action<Task> OnComplete);
        protected abstract void UpdatePlayersArray(Action<Task> OnComplete);
        protected abstract bool IsOneStaying();
        protected abstract void ChangeIsFoldedToFalse();
        protected abstract void EndHand();
        protected abstract void OnMessageReceived(long timeLeft);
        protected abstract void RegisterTimer();
        protected abstract void UpdateFirebaseIfNeeded(bool endedRound, bool skippedTurn, int round, bool isEndOfHand);
        protected abstract void UpdateCheckOrCallUI();
        protected abstract void SyncGameState(Game updatedGame);
        protected abstract bool HasGameJustStarted(Game updatedGame);
        protected abstract bool ShouldEndRound(bool isEndOfRound, bool isHandEnded);
        protected abstract Player[] HandleLastPlayerWins();
        protected abstract bool EveryOneAreEqual();
        protected abstract void EndOfRound(int round);
        protected abstract int CalculateMinBet();
        protected abstract bool AllBetsZero();
        protected abstract void ProcessRoundAndTurnUpdates(bool isEndOfRound, bool isHandEnded, bool changedToFull, int nextRound);
        protected abstract void DistributePotToWinners(Player[] sortedPlayers, Dictionary<Player, HandRank> Dict);
        protected abstract bool HasGameBecomeFull(Game updatedGame);
        protected abstract Dictionary<Player, HandRank> EvaluatePlayerHands();
        protected abstract bool AnyOneIsAllIn();
        protected abstract Player[] SortPlayersByHandRank(Dictionary<Player, HandRank> ranks);
        protected abstract bool AmIWinner();
        protected abstract bool IsRoundEnding(Game updatedGame);
        protected abstract bool IsHandOver();
        protected abstract bool CheckForGameOver();
        protected abstract Player[] HandleShowdown();
        protected abstract void EnsureTimerRegistered();
        protected abstract Player[] HandleHandEnd();
        protected abstract bool FinalizeHandIfHost();
        protected abstract int HandleAllInScenarios();
        public abstract void DisplayOponnentsNames(List<Label> lstOponnentsLabels);
        public abstract void UpdateMoney(List<Label> lstOponnentsLabels, List<Label> lstOponnentsMoneyLabels, string winnerName);
        protected bool ShouldSkipCurrentPlayerTurn() => CurrentPlayer != null && IsMyTurn && (CurrentPlayer.IsFolded);
        #endregion
    }
}
