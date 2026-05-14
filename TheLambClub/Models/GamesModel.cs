using Plugin.CloudFirestore;
using System.Collections.ObjectModel;
using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    /// <summary>
    /// Represents an abstract base model for managing multiple game sessions and collection-level operations in Firebase.
    /// </summary>
    public abstract class GamesModel
    {
        #region fields

        /// <summary>
        /// Registration for the Firestore collection snapshot listener.
        /// </summary>
        protected IListenerRegistration? ilr;

        /// <summary>
        /// Instance of the Firebase data handler for database interactions.
        /// </summary>
        protected FbData fbd = new();

        /// <summary>
        /// The currently selected or active game instance.
        /// </summary>
        protected Game? currentGame;

        #endregion

        #region events

        /// <summary>
        /// Occurs when a new game is successfully added to the collection.
        /// </summary>
        public EventHandler<Game>? OnGameAdded;

        /// <summary>
        /// Occurs when there is a change in the list of available games.
        /// </summary>
        public EventHandler? OnGamesChanged;

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets a value indicating whether an asynchronous operation is currently in progress.
        /// </summary>
        public bool IsBusy { get; set; }

        /// <summary>
        /// Gets or sets the collection of available game sessions.
        /// </summary>
        public ObservableCollection<Game> GamesList { get; set; } = [];

        /// <summary>
        /// Gets or sets the list of possible player counts for a new game.
        /// </summary>
        public ObservableCollection<int> NumberOfPlayersList { get; set; } = [3, 4, 5];

        /// <summary>
        /// Gets a string representation of the selected number of players.
        /// </summary>
        public string DisplayName => $"{SelectedNumberOfPlayers}";

        /// <summary>
        /// Gets or sets the number of players selected for a new game session.
        /// </summary>
        public int SelectedNumberOfPlayers { get; set; } = 0;

        /// <summary>
        /// Gets or sets the current game instance.
        /// </summary>
        public Game? CurrentGame { get => currentGame; set => currentGame = value; }

        #endregion

        #region public methods

        /// <summary>
        /// Attaches a real-time listener to the games collection in the database.
        /// </summary>
        public abstract void AddSnapshotListener();

        /// <summary>
        /// Detaches the real-time listener from the games collection.
        /// </summary>
        public abstract void RemoveSnapshotListener();

        /// <summary>
        /// Initiates the process of adding a new game session to the database.
        /// </summary>
        public abstract void AddGame();

        #endregion

        #region protected methods

        /// <summary>
        /// Handles the completion logic for general asynchronous tasks.
        /// </summary>
        /// <param name="task">The completed task.</param>
        protected abstract void OnComplete(Task task);

        /// <summary>
        /// Handles the logic when a game session is deleted.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data.</param>
        protected abstract void OnGameDeleted(object? sender, EventArgs e);

        /// <summary>
        /// Processes changes received from the Firestore collection snapshot.
        /// </summary>
        /// <param name="snapshot">The updated query snapshot.</param>
        /// <param name="error">Any exception encountered during the sync.</param>
        protected abstract void OnChange(IQuerySnapshot snapshot, Exception error);

        /// <summary>
        /// Processes the result of a query snapshot once it has been retrieved.
        /// </summary>
        /// <param name="qs">The retrieved query snapshot.</param>
        protected abstract void OnComplete(IQuerySnapshot qs);

        #endregion
    }
}