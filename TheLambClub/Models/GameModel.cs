
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public abstract class GameModel
    {
        [Ignored]
        public EventHandler? TimeLeftChanged;
        [Ignored]
        public EventHandler? TimeLeftFinished;
        [Ignored]
        protected IListenerRegistration? ilr;
        [Ignored]      
        protected FbData fbd = new();
        [Ignored]
        public EventHandler? OnGameDeleted;
        [Ignored]
        public EventHandler? OnGameChanged;
        [Ignored]
        public EventHandler? OnCheckOrCallChanged;
        [Ignored]
        public  abstract Player ?CurrentPlayer { get; }
        [Ignored]
        public abstract string CurrentStatus { get;}
        [Ignored]
        protected  SetOfCards setOfCards { get; }= new SetOfCards();
        [Ignored]
        public TimerSettings timerSettings = new(Keys.TimerTotalTime, Keys.TimerInterval);     
        [Ignored]
        public string TimeLeft { get; protected set; } = string.Empty;
        public FBCard[]BoardCards { get; set; }=new FBCard[5];
        public int RoundNumber{get;set;}
        public int CurrentPlayerIndex { get; set; }
        public string HostName { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public int MaxNumOfPlayers { get; set; }
        public int CurrentNumOfPlayers { get; set; }=1;
        public abstract bool IsFull { get; set; }
        [Ignored]
        public string Id { get; set; } = string.Empty;
        [Ignored]
        public string MyName { get; set; } = new User().UserName;
        public string? HostId { get; set; }=string.Empty;
        [Ignored]
        public abstract bool IsHost { get; } 
        [Ignored]
        public string NumOfPlayersName => $"{MaxNumOfPlayers }";
        [Ignored]
        public NumberOfPlayers? NumberOfPlayers { get; set; }
        [Ignored]
        public abstract bool IsMyTurn { get; set; }
        [Ignored]
        public bool CanICheck { get; set; }=true;
        [Ignored]
        public string CheckOrCall { get; set; } = "Check";
        [Ignored]
        protected bool EndOfHand = false;
        [Ignored]
        protected bool TimerCreated = false;
        public Player[]? Players { get; set; }
        protected const int HandComplete = 4;
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void AddSnapShotListener();
        public abstract void RemoveSnapShotListener();
        public abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void NextTurn();
        public abstract void PickedFold();
        protected abstract void FillDummes();
        protected abstract void OnComplete(Task task);
        public abstract void UpdateGuestUser(Action<Task> OnComplete);
        protected abstract void UpdateFireBaseJoinGame(Action<Task> OnComplete);
        protected abstract void UpdateFBTurnUpdate(Action<Task> OnComplete);
        protected abstract void UpdateBoard(Action<Task> OnComplete);
        protected abstract void FillBoard();
        protected abstract void OnChange(IDocumentSnapshot? snapshot, Exception? error);
        protected abstract void FillArrayAndAddCards(bool upDateFB,Action<Task> OnComplete);
        protected abstract void UpdatePlayersArray(Action<Task> OnComplete);
        protected abstract bool IsOneStaying();
        protected abstract void ChangeIsFoldedToFalse();
        protected abstract int BeforeCurrentPlayerIndex();
        protected abstract void CallFunction();
        protected abstract bool EveryOneIsNotRerazeing();
        protected abstract void EndHand();
        protected abstract void BetFunction(object obj);
        protected abstract int FirstPlayerWhichIsNotFold();
        protected abstract void OnMessageReceived(long timeLeft);
        protected abstract void RegisterTimer();
    }
}
