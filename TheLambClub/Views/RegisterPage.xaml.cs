using TheLambClub.ViewModel;

namespace TheLambClub.Views;

/// <summary>
/// The view code-behind for the Registration Page. 
/// Orchestrates the initialization of the UI and binds it to the RegisterPageVM.
/// </summary>
public partial class RegisterPage : ContentPage
{
    #region constructors

    /// <summary>
    /// Initializes the Registration Page UI and sets the ViewModel as the BindingContext.
    /// </summary>
    public RegisterPage()
    {
        InitializeComponent();
        BindingContext = new RegisterPageVM();
    }

    #endregion
}