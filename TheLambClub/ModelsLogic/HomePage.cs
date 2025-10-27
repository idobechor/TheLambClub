using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLambClub.Models;
using TheLambClub.Views;

namespace TheLambClub.ModelsLogic
{
    internal class HomePage:HomePageModel
    {
        public override void MoveToGamePage()
        {
            if (Application.Current != null)
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Application.Current.MainPage = new RuningGameView();
                });
            }
        }
    }
}
