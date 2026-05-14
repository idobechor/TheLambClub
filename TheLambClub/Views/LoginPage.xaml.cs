using TheLambClub.ViewModel;

namespace TheLambClub.Views;

/// <summary>
/// The view code-behind for the Login Page. 
/// Orchestrates the initialization of the UI and binds it to the LoginPageVM.
/// </summary>
public partial class LoginPage : ContentPage
{
    #region constructors

    /// <summary>
    /// Initializes the Login Page UI and sets the ViewModel as the BindingContext.
    /// </summary>
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginPageVM();
    }

    #endregion
}