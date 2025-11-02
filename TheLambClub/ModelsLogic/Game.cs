using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    internal class Game : GameModel
    {
        internal Game()
        {
            HostName = new User().UserName;
            Created = DateTime.Now;
        }

        public override void SetDocument(Action<System.Threading.Tasks.Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }
    }
}
