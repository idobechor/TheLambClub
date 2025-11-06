using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    internal class Game : GameModel
    {
        internal Game()
        {
            HostName = new User().UserName;
            Created = DateTime.Now;
            MaxNumOfPlayersStr = $" max of players: {MaxNumOfPlayers} in a game";
            CurrentNumOfPlayersStr = $" current players: {CurrentNumOfPlayers}";
            IsFull = false;
            CurrentNumOfPlayers = 1;
        }
        public override void SetDocument(Action<System.Threading.Tasks.Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }
    }
}
