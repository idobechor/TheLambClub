using CommunityToolkit.Maui.Views;
using TheLambClub.ViewModel;

namespace TheLambClub.Views;

/// <summary>
/// The view code-behind for the 'Win Game' popup. 
/// Initializes the UI components and binds them to the WinGamePopupVM.
/// </summary>
public partial class WinGamePopup : Popup
{
    #region fields

    private readonly WinGamePopupVM winGamePopupVM;

    #endregion

    #region constructors

    /// <summary>
    /// Initializes the popup, setting the BindingContext with the winning game text.
    /// </summary>
    /// <param name="WinningText">The message indicating the game win to be displayed.</param>
    public WinGamePopup(string WinningText)
    {
        InitializeComponent();
        winGamePopupVM = new WinGamePopupVM(WinningText);
        BindingContext = winGamePopupVM;
    }

    #endregion
}