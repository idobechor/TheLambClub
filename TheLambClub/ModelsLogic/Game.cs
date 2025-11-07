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

        public override string OpponentName => IsHost? GuestName: HostName;
        public override void SetDocument(Action<System.Threading.Tasks.Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }
    }
}
