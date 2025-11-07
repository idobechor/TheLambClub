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
    public class GamesModel
    {
        protected IListenerRegistration? ilr;
        protected FbData fbd = new();
        protected Game? currentGame = new();
        public bool IsBusy { get; set; }
        public EventHandler<Game>? OnGameAdded;
        public ObservableCollection<Game>? GamesList { get; set; } = [];
        public Game? CurrentGame { get => currentGame; set => currentGame = value; }
        public EventHandler? OnGamesChanged;
    }
}
