using System.Collections.ObjectModel;
using System.Windows.Input;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;
using TheLambClub.Views;

namespace TheLambClub.ViewModel
{ 
public class MainPageVM : ObservableObject
{
    private readonly Games games = new();
    private readonly User user = new();
    public ObservableCollection<int>? NumberOfPlayersList { get => games.NumberOfPlayersList; set => games.NumberOfPlayersList = value; }
    public int SelectedNumberOfPlayers { get => games.SelectedNumberOfPlayers; 
            set { games.SelectedNumberOfPlayers = value; (AddGameCommand as Command)?.ChangeCanExecute(); } }
        private bool CanAddGame()
        {
            return SelectedNumberOfPlayers !=0;
        }
        public ICommand InstructionsCommand { get; private set; }
    public ICommand AddGameCommand {get;}
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
    private void AddGame()
    {
        games.AddGame();
        OnPropertyChanged(nameof(IsBusy));
    }
    public MainPageVM()
    {
        AddGameCommand = new Command(AddGame, CanAddGame);
        InstructionsCommand = new Command(ShowInstructionsPrompt);
        games.OnGameAdded += OnGameAdded;
        games.OnGamesChanged += OnGamesChanged;
        }
    public void ShowInstructionsPrompt(object obj)
    {
      Application.Current!.MainPage!.DisplayAlert(Strings.InsructionsTxtTitle, Strings.InsructionsTxt, Strings.Ok);
    }

    private void OnGameAdded(object? sender, Game game)
    {
        OnPropertyChanged(nameof(IsBusy));
        MainThread.InvokeOnMainThreadAsync(() =>
        {
            Shell.Current.Navigation.PushAsync(new GamePage(game), true);
        });
    }
    public void AddSnapshotListener()
    {
        games.AddSnapshotListener();
    }

    public void RemoveSnapshotListener()
    {
        games.RemoveSnapshotListener();
    }
    private void OnGamesChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(GamesList));
    }
  }
}
