using TheLambClub.ModelsLogic;
using TheLambClub.NewFolder;
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
