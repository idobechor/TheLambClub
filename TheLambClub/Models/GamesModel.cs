using Plugin.CloudFirestore;
using System.Collections.ObjectModel;
using TheLambClub.ModelsLogic;
namespace TheLambClub.Models
{
    public abstract class GamesModel
    {
        #region fields

        protected IListenerRegistration? ilr;
        protected FbData fbd = new();
        protected Game? currentGame;

        #endregion

        #region events

        public EventHandler<Game>? OnGameAdded;
        public EventHandler? OnGamesChanged;

        #endregion

        #region properties

        public bool IsBusy { get; set; }
        public ObservableCollection<Game> GamesList { get; set; } = [];
        public ObservableCollection<int> NumberOfPlayersList { get; set; } = [3, 4, 5];
        public string DisplayName => $"{SelectedNumberOfPlayers}";
        public int SelectedNumberOfPlayers { get; set; } = 0;
        public Game? CurrentGame { get => currentGame; set => currentGame = value; }

        #endregion

        #region public methods

        public abstract void AddSnapshotListener();
        public abstract void RemoveSnapshotListener();
        public abstract void AddGame();

        #endregion

        #region protected methods

        protected abstract void OnComplete(Task task);
        protected abstract void OnGameDeleted(object? sender, EventArgs e);
        protected abstract void OnChange(IQuerySnapshot snapshot, Exception error);
        protected abstract void OnComplete(IQuerySnapshot qs);

        #endregion
    }
}
