using TheLambClub.ViewModel;

namespace TheLambClub.Views;

/// <summary>
/// The view code-behind for the selection page between Login and Registration.
/// Orchestrates the initialization of the UI and binds it to the PickRegisterOrLoginPageVM.
/// </summary>
public partial class PickRegisterOrLoginPage : ContentPage
{
    #region constructors

    /// <summary>
    /// Initializes the selection page UI and sets the ViewModel as the BindingContext.
    /// </summary>
    public PickRegisterOrLoginPage()
    {
        InitializeComponent();
        BindingContext = new PickRegisterOrLoginPageVM();
    }

    #endregion
}