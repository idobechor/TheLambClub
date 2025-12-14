
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public abstract class GameModel
    {
        [Ignored]
        protected IListenerRegistration? ilr;
        [Ignored]      
        protected FbData fbd = new();
        [Ignored]
        public EventHandler? OnGameDeleted;
        [Ignored]
        public EventHandler? OnGameChanged;
        [Ignored]
        public  abstract Player ?CurrentPlayer { get; }
        [Ignored]
        public abstract string CurrentStatus { get;}
        [Ignored]
        protected  SetOfCards setOfCards { get; }= new SetOfCards();
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
        public abstract bool IsMyTurn { get; }
        //[Ignored]
        //public ObservableCollection<Player> Players { get; set; } = [];
        public Player[]? Players { get; set; }
        protected const int HandComplete = 5;
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
        protected abstract void FillArrayAndAddCards(Action<Task> OnComplete);
        protected abstract void UpdatePlayersArray(Action<Task> OnComplete);
        protected abstract bool IsOneStaying();
        protected abstract void ChangeIsFoldedToFalse();
    }
}
