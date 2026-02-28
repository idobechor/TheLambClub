using TheLambClub.ModelsLogic;
using TheLambClub.Views;
namespace TheLambClub
{
    public partial class App : Application
    {
        #region constructors

        public App()
        {
            InitializeComponent();
            MainPage = new PickRegisterOrLoginPage();
        }

        #endregion
    }
}
