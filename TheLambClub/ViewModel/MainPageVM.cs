using System.Collections.ObjectModel;
using System.Windows.Input;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;
using TheLambClub.Views;

namespace TheLambClub.ViewModel
{
    /// <summary>
    /// ViewModel for the Main Page. Manages the list of available games, 
    /// player selection, and navigation to specific game instances.
    /// </summary>
    public class MainPageVM : ObservableObject
    {
        #region fields

        private readonly Games games = new();
        private readonly User user = new();

        #endregion

        #region commands

        /// <summary>
        /// Command to display application instructions to the user.
        /// </summary>
        public ICommand InstructionsCommand { get; private set; }

        /// <summary>
        /// Command to initiate the creation of a new game.
        /// </summary>
        public ICommand AddGameCommand { get; }

        #endregion

        #region properties

        public ObservableCollection<int>? NumberOfPlayersList { get => games.NumberOfPlayersList; set => games.NumberOfPlayersList = value!; }

        public int SelectedNumberOfPlayers
        {
            get => games.SelectedNumberOfPlayers;
            set { games.SelectedNumberOfPlayers = value; (AddGameCommand as Command)?.ChangeCanExecute(); }
        }

        public ObservableCollection<Game>? GamesList => games.GamesList;
        public string UserName => user.UserName;
        public bool IsBusy => games.IsBusy;

        /// <summary>
        /// Gets or sets the currently selected game from the list. 
        /// Navigates to the GamePage upon selection.
        /// </summary>
        public Game? SelectedItem
        {
            get => games.CurrentGame;
            set
            {
                if (value != null)
                {
                    games.CurrentGame = value;
                    games.CurrentGame.UpdateGuestUser((t) => { });
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.Navigation.PushAsync(new GamePage(value), true);
                    });
                }
            }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Initializes the MainPageVM, configures commands, and subscribes to game-related events.
        /// </summary>
        public MainPageVM()
        {
            AddGameCommand = new Command(AddGame, CanAddGame);
            InstructionsCommand = new Command(ShowInstructionsPrompt);
            games.OnGameAdded += OnGameAdded;
            games.OnGamesChanged += OnGamesChanged;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Starts listening for real-time Firebase database updates for the games list.
        /// </summary>
        public void AddSnapshotListener()
        {
            games.AddSnapshotListener();
        }

        /// <summary>
        /// Stops listening for real-time Firebase database updates.
        /// </summary>
        public void RemoveSnapshotListener()
        {
            games.RemoveSnapshotListener();
        }

        #endregion

        #region private methods

        /// <summary>
        /// Validates that a valid number of players has been selected before allowing game creation.
        /// </summary>
        private bool CanAddGame()
        {
            return SelectedNumberOfPlayers != 0;
        }

        /// <summary>
        /// Triggers the logic to add a new game and updates the busy state.
        /// </summary>
        private void AddGame()
        {
            games.AddGame();
            OnPropertyChanged(nameof(IsBusy));
        }

        /// <summary>
        /// Handles the event when a new game is successfully added, navigating to its page.
        /// </summary>
        private void OnGameAdded(object? sender, Game game)
        {
            OnPropertyChanged(nameof(IsBusy));
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PushAsync(new GamePage(game), true);
            });
        }

        /// <summary>
        /// Refreshes the GamesList property in the UI when the underlying collection changes.
        /// </summary>
        private void OnGamesChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(GamesList));
        }

        #endregion
    }
}