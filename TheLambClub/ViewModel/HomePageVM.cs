using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    internal class HomePageVM: ObservableObject
    {
        private Games games = new();
        private User user = new();
        private readonly HomePage homePage = new();
        public ICommand ShowNumericPromptCommand { get; private set; }
        public ICommand InstructionsCommand { get; private set; }
        public ICommand AddGameCommand => new Command(AddGame);
        public ObservableCollection<Game>? GamesList => games.GamesList;
        public string UserName => user.UserName;
        public bool IsBusy => games.IsBusy;
        private void AddGame()
        {
            games.AddGame();
            OnPropertyChanged(nameof(IsBusy));
        }
        public HomePageVM()
        {
            ShowNumericPromptCommand = new Command(ShowNumericPromptCasting);
            InstructionsCommand = new Command(ShowInstructionsPrompt);
            games.OnGameAdded += OnGameAdded;
            games.OnGamesChanged += OnGamesChanged;
        }           
        public void ShowNumericPromptCasting(object obj)
        {
            homePage.ShowNumericPromptCasting(obj);
        }
        public void ShowInstructionsPrompt(object obj)
        {
            homePage.ShowInstructionsPrompt(obj);
        }

        private void OnGameAdded(object? sender, bool e)
        {
            OnPropertyChanged(nameof(IsBusy));
        }
        internal void AddSnapshotListener()
        {
            games.AddSnapshotListener();
        }

        internal void RemoveSnapshotListener()
        {
            games.RemoveSnapshotListener();
        }
        private void OnGamesChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(GamesList));
        }

    }
}
