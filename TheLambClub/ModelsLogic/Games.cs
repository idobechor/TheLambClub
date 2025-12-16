
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Plugin.CloudFirestore;
using TheLambClub.Models;
namespace TheLambClub.ModelsLogic
{
    public class Games : GamesModel
    {
        public override void AddGame()
        {
            IsBusy = true;
            CurrentGame = new(SelectedNumberOfPlayers);
            currentGame?.Players = new Player[SelectedNumberOfPlayers.NumPlayers];
            currentGame?.Players?[0] = new Player((new User()).UserName, fbd.UserId);
            currentGame?.HostId = fbd.UserId;//לhא בדקתhי
            currentGame?.OnGameDeleted += OnGameDeleted;           
            CurrentGame.SetDocument(OnComplete);
        }

        protected override void OnGameDeleted(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.GameDeleted, ToastDuration.Long).Show();
            });
        }
        public Games()
        {

        }
        protected override void OnComplete(Task task)
        {
            IsBusy = false;
            OnGameAdded?.Invoke(this, CurrentGame!);
        }
        public override void AddSnapshotListener()
        {
            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, OnChange!);
        }
        public override void RemoveSnapshotListener()
        {
            ilr?.Remove();
        }
        protected override void OnChange(IQuerySnapshot snapshot, Exception error)
        {
            fbd.GetDocumentsWhereEqualTo(Keys.GamesCollection, nameof(GameModel.IsFull), false, OnComplete);
        }
        protected override void OnComplete(IQuerySnapshot qs) 
        {
            GamesList!.Clear(); // clean list
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
    }
}
