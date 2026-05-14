using CommunityToolkit.Maui.Views;
using TheLambClub.ModelsLogic;
using TheLambClub.ViewModel;

namespace TheLambClub.Views;

/// <summary>
/// The view code-behind for the 'Winning' popup. 
/// Orchestrates the initialization of the UI, binds it to the WinningPopupVM,
/// and handles the lifecycle event for closing the popup.
/// </summary>
public partial class WinningPopupPage : Popup
{
    #region fields

    private readonly WinningPopupVM winningPopupVM;

    #endregion

    #region constructors

    /// <summary>
    /// Initializes the popup, sets the ViewModel as the BindingContext, 
    /// and subscribes to the request to close the popup.
    /// </summary>
    /// <param name="Players">Array of players involved in the showdown.</param>
    /// <param name="ranks">Dictionary mapping players to their hand rank.</param>
    /// <param name="numUpWinners">Number of winners to be displayed.</param>
    public WinningPopupPage(Player[] Players, Dictionary<Player, HandRank> ranks, int numUpWinners)
    {
        InitializeComponent();
        // Initializes the ViewModel with the showdown details
        winningPopupVM = new WinningPopupVM(Players, ranks, numUpWinners);
        BindingContext = winningPopupVM;
        winningPopupVM.RequestClose += OnRequestClose;
    }

    #endregion

    #region private methods

    /// <summary>
    /// Handles the request to close the popup by triggering the Close() method.
    /// </summary>
    private void OnRequestClose()
    {
        Close();
    }

    #endregion
}