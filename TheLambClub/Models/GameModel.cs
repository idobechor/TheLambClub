
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System.Collections.ObjectModel;
using TheLambClub.ModelsLogic;
using TheLambClub.ViewModel;
using static TheLambClub.Models.CardModel;

namespace TheLambClub.Models
{
    public abstract class GameModel
    {
        [Ignored]
        protected IListenerRegistration? ilr;
        [Ignored]      
        protected FbData fbd = new();
        [Ignored]
        protected Board GameBoard=new();
        [Ignored]
        public EventHandler? OnGameDeleted;
        [Ignored]
        public EventHandler? OnGameChanged;
        [Ignored]
        public Player CurrentPlayer { get; set; }
        [Ignored]
        public abstract string CurrentStatus { get; set; }
        public int CurrentPlayerIndex { get; set; }
        public string HostName { get; set; } = string.Empty;
        public string[]? PlayersNames { get; set; }
        public string[]? PlayersIds { get; set; }
        public DateTime Created { get; set; }
        public int MaxNumOfPlayers { get; set; }
        public int CurrentNumOfPlayers { get; set; }=1;
        public abstract bool IsFull { get; set; }
        [Ignored]
        public string Id { get; set; } = string.Empty;
        [Ignored]
        public string MyName { get; set; } = new User().UserName;
        [Ignored]
        public string HostId { get; set; } 
        [Ignored]
        public bool IsHostUser { get; set; }
        [Ignored]
        public string NumOfPlayersName => $"{MaxNumOfPlayers }";
        [Ignored]
        public NumberOfPlayers? NumberOfPlayers { get; set; }
        [Ignored]
        public abstract bool IsMyTurn { get; }
        [Ignored]
        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void AddSnapShotListener();
        public abstract void RemoveSnapShotListener();
        public abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void NextTurn();
    }
}
