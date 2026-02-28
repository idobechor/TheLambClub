using TheLambClub.ViewModel;

namespace TheLambClub.Views;

public partial class RegisterPage : ContentPage
{
    #region constructors

    public RegisterPage()
    {
        InitializeComponent();
        BindingContext = new RegisterPageVM();
    }

    #endregion
}
