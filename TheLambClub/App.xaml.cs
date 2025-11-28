
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
          MainPage = new PickRegisterOrLoginPage();
        }      
    }
}
