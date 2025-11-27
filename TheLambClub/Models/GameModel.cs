
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System.Collections.ObjectModel;
using TheLambClub.ModelsLogic;
using static TheLambClub.Models.CardModel;

namespace TheLambClub.Models
{
    public abstract class GameModel
    {
      

        protected IListenerRegistration? ilr;
        private readonly Games Games = new();       
        protected FbData fbd = new();
        [Ignored]
        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();
        protected Board GameBoard=new();
        [Ignored]
        public EventHandler? OnGameChanged;
        [Ignored]
        public EventHandler? OnGameDeleted;
        public string HostName { get; set; } = string.Empty;
        public string[]? PlayersNames { get; set; }
        public DateTime Created { get; set; }
        public int MaxNumOfPlayers { get; set; }
        public bool IsFull { get; set; }
        public int CurrentNumOfPlayers { get; set; }=1;
        [Ignored]
        public string Id { get; set; } = string.Empty;
        [Ignored]
        public string MyName { get; set; } = new User().UserName;
        [Ignored]
        public bool IsHostUser { get; set; }
        [Ignored]
        public string NumOfPlayersName => $"{MaxNumOfPlayers }";
        [Ignored]
        public NumberOfPlayers? NumberOfPlayers { get; set; }
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void AddSnapShotListener();
        public abstract void RemoveSnapShotListener();
        public abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete);
        protected abstract void createPlayers();
        public abstract void Init();
    }
}
