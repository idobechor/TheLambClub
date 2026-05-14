using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Plugin.CloudFirestore;
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    /// <summary>
    /// Manages the collection of games, handling the creation of new game rooms 
    /// and monitoring the lobby for available (non-full) games.
    /// </summary>
    public class Games : GamesModel
    {
        #region constructors

        public Games()
        {
        }

        #endregion

        #region public methods

        /// <summary>
        /// Creates a new game instance, initializes the host player, 
        /// and uploads the game document to Firestore.
        /// </summary>
        public override void AddGame()
        {
            IsBusy = true;
            CurrentGame = new(SelectedNumberOfPlayers);
            if (currentGame != null)
            {
                // Initialize the players array and set the current user as the host.
                currentGame.Players = new Player[SelectedNumberOfPlayers];
                currentGame.Players[0] = new Player((new User()).UserName, fbd.UserId);
                currentGame.HostId = fbd.UserId;
                
                // Subscribe to deletion events and save the document.
                currentGame.OnGameDeleted += OnGameDeleted;
                CurrentGame.SetDocument(OnComplete);
            }
        }

        /// <summary>
        /// Registers a listener to the games collection in Firestore to receive real-time updates.
        /// </summary>
        public override void AddSnapshotListener()
        {
            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, OnChange!);
        }

        /// <summary>
        /// Removes the active snapshot listener to stop receiving updates.
        /// </summary>
        public override void RemoveSnapshotListener()
        {
            ilr?.Remove();
        }

        #endregion

        #region protected methods

        /// <summary>
        /// Displays a toast message on the UI thread when a game is deleted.
        /// </summary>
        protected override void OnGameDeleted(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.GameDeleted, ToastDuration.Long).Show();
            });
        }

        /// <summary>
        /// Handles the completion of the document creation process.
        /// </summary>
        protected override void OnComplete(Task task)
        {
            IsBusy = false;
            OnGameAdded?.Invoke(this, CurrentGame!);
        }

        /// <summary>
        /// Triggered when the collection changes. It queries Firestore for games 
        /// that are not yet full to update the lobby list.
        /// </summary>
        protected override void OnChange(IQuerySnapshot snapshot, Exception error)
        {
            fbd.GetDocumentsWhereEqualTo(Keys.GamesCollection, nameof(GameModel.IsFull), false, OnComplete);
        }

        /// <summary>
        /// Processes the query results of available games, populating the GamesList 
        /// and notifying the UI of changes.
        /// </summary>
        /// <param name="qs">The query snapshot containing available games.</param>
        protected override void OnComplete(IQuerySnapshot qs)
        {
            GamesList!.Clear();
            foreach (IDocumentSnapshot ds in qs.Documents)
            {
                Game? game = ds.ToObject<Game>();
                if (game != null)
                {
                    game.Id = ds.Id;
                    GamesList.Add(game);
                }
            }
            OnGamesChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}