using TheLambClub.ViewModel;

namespace TheLambClub.Views;

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
