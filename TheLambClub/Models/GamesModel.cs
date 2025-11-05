using Microsoft.Maui.Controls;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    internal class GamesModel
    {
        protected IListenerRegistration? ilr;
        protected FbData fbd = new();
        public bool IsBusy { get; set; }
        public EventHandler<bool>? OnGameAdded;
        public ObservableCollection<Game>? GamesList { get; set; } = [];
        public EventHandler? OnGamesChanged;
    }
}
