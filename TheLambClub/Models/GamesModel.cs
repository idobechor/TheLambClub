
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
        public ObservableCollection<Game>? GamesList { get; set; } = [];
        public ObservableCollection<NumberOfPlayers>? NumberOfPlayersList { get; set; } = [new NumberOfPlayers(4), new NumberOfPlayers(5), new NumberOfPlayers(6)];
        public NumberOfPlayers SelectedNumberOfPlayers { get; set; } = new NumberOfPlayers();
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
