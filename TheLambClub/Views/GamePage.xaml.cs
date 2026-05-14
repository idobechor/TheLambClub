using TheLambClub.ModelsLogic;
using TheLambClub.ViewModel;

namespace TheLambClub.Views;

/// <summary>
/// The view code-behind for the Game Page. Manages the lifecycle of the page 
/// and links the UI components to the GamePageVM.
/// </summary>
public partial class GamePage : ContentPage
{
    #region fields

    private readonly GamePageVM gpVM;

    #endregion

    #region constructors

    /// <summary>
    /// Initializes the GamePage, setting up the ViewModel and binding it to the page context.
    /// </summary>
    /// <param name="game">The current game session context.</param>
    public GamePage(Game game)
    {
        InitializeComponent();
        // Initializes the ViewModel with the game instance and the opponent grid from XAML
        gpVM = new GamePageVM(game, grdOponnents);
        BindingContext = gpVM;
    }

    #endregion

    #region protected methods

    /// <summary>
    /// Called when the page appears; initiates the real-time snapshot listener.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        gpVM.AddSnapshotListener();
    }

    /// <summary>
    /// Called when the page disappears; removes the snapshot listener to prevent memory leaks 
    /// and unnecessary network calls.
    /// </summary>
    protected override void OnDisappearing()
    {
        gpVM.RemoveSnapshotListener();
        base.OnDisappearing();
    }

    #endregion
}