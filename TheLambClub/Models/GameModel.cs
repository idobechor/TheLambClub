
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public abstract class GameModel
    {
        protected IListenerRegistration? ilr;
        protected const int MaxNumOfPlayers = 6;
        protected FbData fbd = new();
        [Ignored]
        public EventHandler? OnGameChanged;
        [Ignored]
        public EventHandler? OnGameDeleted;
        public string HostName { get; set; } = string.Empty;
        public string[] Players { get; set; }= new string[MaxNumOfPlayers];
        public DateTime Created { get; set; }
        public bool IsFull { get; set; }
        public int CurrentNumOfPlayers { get; set; }=1;
        [Ignored]
        public string Id { get; set; } = string.Empty;
        [Ignored]
        public  string MaxNumOfPlayersStr  { get; set; } = string.Empty;
        [Ignored]
        public  string CurrentNumOfPlayersStr { get; set; } = string.Empty;
        [Ignored]
        public string MyName { get; set; } = new User().UserName;
        [Ignored]
        public abstract string OpponentsNames { get;  }
        [Ignored]
        public bool IsHostUser { get; set; } 
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void AddSnapShotListener();
        public abstract void RemoveSnapShotListener();
        public abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete);
    }
}
