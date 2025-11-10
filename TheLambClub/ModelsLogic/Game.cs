using Plugin.CloudFirestore;
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    public class Game : GameModel
    {
        public Game()
        {
            HostName = new User().UserName;
            Created = DateTime.Now;
            MaxNumOfPlayersStr = $" max of players: {MaxNumOfPlayers} in a game";
            CurrentNumOfPlayersStr = $" current players: {CurrentNumOfPlayers}";
            IsFull = false;
            CurrentNumOfPlayers = 1;
        }

        public override string OpponentName => IsHostUser? GuestName: HostName;


        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }


        public override void AddSnapShotListener()
        {
            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, Id, OnChange);
        }

        private void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
             IsFull= updatedGame.IsFull;
             GuestName= updatedGame.GuestName;
             OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public override void RemoveSnapShotListener()
        {
            ilr?.Remove();
        }
        public void UpdateGuestUser(Action<Task> OnComplete)
        {
            IsFull = true;
            GuestName=MyName;
            UpdateFireBaseJoinGame(OnComplete);
        }

        private void UpdateFireBaseJoinGame(Action<Task> OnComplete)
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(GuestName), GuestName },
                { nameof(IsFull), IsFull }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }
    }
}
