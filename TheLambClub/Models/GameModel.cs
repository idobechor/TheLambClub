
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System.Collections.ObjectModel;
using TheLambClub.ModelsLogic;
namespace TheLambClub.Models
{
    public abstract class GameModel
    {
        [Ignored]
        public EventHandler<WinningPopupEvent>? OnwinnerSelected;
        [Ignored]
        public EventHandler? TimeLeftChanged;
        [Ignored]
        public EventHandler? TimeLeftFinished;
        [Ignored]
        public EventHandler? OpenMyTurnPopUp;
        [Ignored]
        protected IListenerRegistration? ilr;
        [Ignored]      
        protected FbData fbd = new();
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
        public EventHandler? OnMyMoneyChanged;
        [Ignored]
        public  abstract Player ?CurrentPlayer { get; }
        [Ignored]
        public abstract string CurrentStatus { get;}
        [Ignored]
        protected  SetOfCards SetOfCards { get; }= new SetOfCards();
        [Ignored]
        public TimerSettings timerSettings = new(Keys.TimerTotalTime, Keys.TimerInterval);     
        [Ignored]
        public string TimeLeft { get; protected set; } = string.Empty;     
        public FBCard[]BoardCards { get; set; }=new FBCard[5];
        public int RoundNumber{get;set;}
        public int beforeCurrentPlayerIndex { get; set; } 
        protected int _currentPlayerIndex;
        public abstract int CurrentPlayerIndex { get; set; }
        public string HostName { get; set; } = string.Empty;
        public double[] Pot = new double[5];
        public DateTime Created { get; set; }
        public int MaxNumOfPlayers { get; set; }
        public int CurrentNumOfPlayers { get; set; }=1;
        public string PlayerBeforeId=string.Empty;
        [Ignored]
        public int MinBet { get; set; }
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
        public int? NumberOfPlayers { get; set; }
        [Ignored]
        public abstract bool IsMyTurn { get; }
        [Ignored]
        public bool CanICheck { get; set; }=true;
        [Ignored]
        public string CheckOrCall { get; set; } = Strings.Check;
        [Ignored]
        protected bool TimerCreated = false;
        public Player[]? Players { get; set; }
        protected const int HandComplete = 4;       
        [Ignored]
        public bool IsPopupOpen { get; set; }= false;
        [Ignored]
        public abstract ObservableCollection<ViewCard> ?BoardViewCards { get; }
        [Ignored]
        public abstract ViewCard ?ViewCard1 { get; }
        [Ignored]
        public abstract ViewCard ?ViewCard2 { get; }
        [Ignored]
        public bool IsHappened=false;
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void AddSnapShotListener();
        public abstract void RemoveSnapShotListener();
        public abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void NextTurn(bool UpDateFB);
        public abstract void PickedFold();
        protected abstract void OnComplete(Task task);
        public abstract void UpdateGuestUser(Action<Task> OnComplete);
        protected abstract void UpdateFireBaseJoinGame(Action<Task> OnComplete);
        protected abstract void UpdateFBTurnUpdate(Action<Task> OnComplete);
      //  protected abstract void UpdateBoard( Action<Task> OnComplete);
        protected abstract void FillBoard(int round);
        protected abstract void OnChange(IDocumentSnapshot? snapshot, Exception? error);
        protected abstract void FillArrayAndAddCards(bool upDateFB,Action<Task> OnComplete);
        protected abstract void UpdatePlayersArray(Action<Task> OnComplete);
        protected abstract bool IsOneStaying();
        protected abstract void ChangeIsFoldedToFalse();
        //protected abstract int BeforeCurrentPlayerIndex();
        public abstract void CallFunction();
        protected abstract bool EveryOneIsNotRerazeing();
        protected abstract void EndHand();
        public abstract void BetFunction(object obj);
        protected abstract int FirstPlayerWhichIsNotFold();
        protected abstract void OnMessageReceived(long timeLeft);
        protected abstract void RegisterTimer();
    }
}
