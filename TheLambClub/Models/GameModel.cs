using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    internal abstract class GameModel
    {
        protected const int MaxNumOfPlayers = 6;
        protected FbData fbd = new();
        [Ignored]
        public string Id { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public bool IsFull { get; set; }
        public int CurrentNumOfPlayers { get; set; }=1;
        [Ignored]
        public static string MaxNumOfPlayersStr  { get; set; }= $" max of players:{MaxNumOfPlayers} in a game";
        [Ignored]
        public static string CurrentNumOfPlayersStr { get; set; } = $" current players{MaxNumOfPlayers}";
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
    }
}
