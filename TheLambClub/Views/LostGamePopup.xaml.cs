using CommunityToolkit.Maui.Views;
using TheLambClub.ViewModel;

namespace TheLambClub.Views;

/// <summary>
/// The view code-behind for the 'Lost Game' popup. 
/// Initializes the UI components and binds them to the LostGamePopupVM.
/// </summary>
public partial class LostGamePopup : Popup
{
    #region constructors

    /// <summary>
    /// Initializes the popup, setting the BindingContext with the losing game text.
    /// </summary>
    /// <param name="losingText">The message indicating the game loss to be displayed.</param>
    public LostGamePopup(string losingText)
    {
        BindingContext = new LostGamePopupVM(losingText);
        InitializeComponent();
    }

    #endregion
}