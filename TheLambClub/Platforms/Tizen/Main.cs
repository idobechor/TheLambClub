using System;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace TheLambClub
{
    public class Program : MauiApplication
    {
        #region protected methods

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        #endregion

        #region private methods

        static void Main(string[] args)
        {
            Program app = new Program();
            app.Run(args);
        }

        #endregion
    }
}
