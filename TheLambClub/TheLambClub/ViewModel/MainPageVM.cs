using System.Collections.ObjectModel;
using System.Windows.Input;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;
using TheLambClub.Views;

namespace TheLambClub.ViewModel
{
    public class MainPageVM : ObservableObject
    {
        #region fields

        private readonly Games games = new();
        private readonly User user = new();

        #endregion

        #region commands

        public ICommand InstructionsCommand { get; private set; }
        public ICommand AddGameCommand { get; }

        #endregion

        #region properties

        public ObservableCollection<int>? NumberOfPlayersList { get => games.NumberOfPlayersList; set => games.NumberOfPlayersList = value!; }
        public int SelectedNumberOfPlayers { get => games.SelectedNumberOfPlayers;
            set { games.SelectedNumberOfPlayers = value; (AddGameCommand as Command)?.ChangeCanExecute(); } }
        public ObservableCollection<Game>? GamesList => games.GamesList;
        public string UserName => user.UserName;
        public bool IsBusy => games.IsBusy;
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

        public MainPageVM()
        {
            AddGameCommand = new Command(AddGame, CanAddGame);
            InstructionsCommand = new Command(ShowInstructionsPrompt);
            games.OnGameAdded += OnGameAdded;
            games.OnGamesChanged += OnGamesChanged;
        }

        #endregion
        #region public methods
        public void AddSnapshotListener()
        {
            games.AddSnapshotListener();
        }
        public void RemoveSnapshotListener()
        {
            games.RemoveSnapshotListener();
        }

        #endregion

        #region private methods

        private bool CanAddGame()
        {
            return SelectedNumberOfPlayers != 0;
        }
        private void AddGame()
        {
            games.AddGame();
            OnPropertyChanged(nameof(IsBusy));
        }
        private void OnGameAdded(object? sender, Game game)
        {
            OnPropertyChanged(nameof(IsBusy));
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PushAsync(new GamePage(game), true);
            });
        }
        private void OnGamesChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(GamesList));
        }

        #endregion
    }
}
