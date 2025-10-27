using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    internal class HomePageVM
    {
        public ICommand StartGameCommand { get => new Command(MoveToGamePage); }
        private readonly HomePage homePage = new();
        private void MoveToGamePage()
        {
            homePage.MoveToGamePage();
        }
    }
}
