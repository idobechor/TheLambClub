using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    internal class Games : GamesModel
    {
        internal void AddGame()
        {
            IsBusy = true;
            Game game = new Game();
            game.SetDocument(OnComplete);
        }
        private void OnComplete(Task task)
        {
            IsBusy = false;
            OnGameAdded?.Invoke(this, task.IsCompletedSuccessfully);
        }
    }
}
