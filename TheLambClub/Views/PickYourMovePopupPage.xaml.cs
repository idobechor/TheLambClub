using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using TheLambClub.ModelsLogic;
using TheLambClub.Services;
using TheLambClub.ViewModel;

namespace TheLambClub.Views;

/// <summary>
/// The view code-behind for the 'Pick Your Move' popup. 
/// Orchestrates the initialization of the UI, binds it to the PickYourMovePopupPageVM,
/// and handles the lifecycle event for closing the popup.
/// </summary>
public partial class PickYourMovePopupPage : Popup
{
    #region fields

    private readonly PickYourMovePopupPageVM PromptYourMoveVM;

    #endregion

    #region constructors

    /// <summary>
    /// Initializes the popup, sets the ViewModel as the BindingContext, 
    /// and subscribes to the request to close the popup.
    /// </summary>
    /// <param name="game">The current game session.</param>
    /// <param name="suggestionService">The optional AI suggestion service.</param>
    public PickYourMovePopupPage(Game game, IPokerSuggestionService? suggestionService)
    {
        InitializeComponent();
        // Initializes the ViewModel with the game instance, the UI label for time, and the suggestion service
        PromptYourMoveVM = new PickYourMovePopupPageVM(game, TimeLeft, suggestionService);
        BindingContext = PromptYourMoveVM;
        PromptYourMoveVM.RequestClose += OnRequestClose;
    }

    #endregion

    #region private methods

    /// <summary>
    /// Handles the request to close the popup by triggering cleanup in the ViewModel 
    /// and closing the popup view.
    /// </summary>
    private void OnRequestClose()
    {
        PromptYourMoveVM.Close();
        Close();
    }

    #endregion
}