
using TheLambClub.ModelsLogic;
using TheLambClub.NewFolder;
using TheLambClub.Views;
namespace TheLambClub
{
    public partial class App : Application
    {
        public App()
        {
          InitializeComponent(); 
            User user = new();
            Page page = user.IsRegistered ? new LoginPage() : new RegisterPage();
            MainPage = new HomePageView();
        }      
    }
}
