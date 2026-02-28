using TheLambClub.ViewModel;

namespace TheLambClub.NewFolder;

public partial class LoginPage : ContentPage
{
    #region constructors

    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginPageVM();
    }

    #endregion
}
