
using Plugin.CloudFirestore;
using System.Collections.ObjectModel;
using TheLambClub.ModelsLogic;
namespace TheLambClub.Models
{
    public abstract class GamesModel
    {
        protected IListenerRegistration? ilr;
        protected FbData fbd = new();
        protected Game? currentGame;
        public bool IsBusy { get; set; }
        public EventHandler<Game>? OnGameAdded;
        public ObservableCollection<Game>? GamesList { get; set; } = [];//רשימת השחקנים שמציגים שחסר בהם משתתפים
        public ObservableCollection<int>? NumberOfPlayersList { get; set; } = [4, 5, 6];
        public string DisplayName => $"{SelectedNumberOfPlayers}";
        public int SelectedNumberOfPlayers { get; set; } 
        public Game? CurrentGame { get => currentGame; set => currentGame = value; }
        public EventHandler? OnGamesChanged;
        public abstract void AddSnapshotListener();
        public abstract void RemoveSnapshotListener();
        public abstract void AddGame();
        protected abstract void OnComplete(Task task);
        protected abstract void OnGameDeleted(object? sender, EventArgs e);
        protected abstract void OnChange(IQuerySnapshot snapshot, Exception error);
        protected abstract void OnComplete(IQuerySnapshot qs);


    }
}
