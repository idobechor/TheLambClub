using TheLambClub.ViewModel;

namespace TheLambClub.NewFolder;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        BindingContext = new LoginPageVM();
    }
}