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
        protected FbData fbd = new();
        [Ignored]
        public string Id { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public bool IsFull { get; set; }
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
    }
}
