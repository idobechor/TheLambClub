using TheLambClub.ViewModel;

namespace TheLambClub;

/// <summary>
/// The view code-behind for the Main Page. Manages the page lifecycle, 
/// binding the UI to the MainPageVM and handling real-time data synchronization.
/// </summary>
public partial class MainPage : ContentPage
{
    #region fields

    private readonly MainPageVM mpVM = new();

    #endregion

    #region constructors

    /// <summary>
    /// Initializes the Main Page UI and sets the ViewModel as the BindingContext.
    /// </summary>
    public MainPage()
    {
        InitializeComponent();
        BindingContext = mpVM;
    }

    #endregion

    #region protected methods

    /// <summary>
    /// Called when the page appears; initiates the real-time snapshot listener 
    /// for game updates.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        mpVM.AddSnapshotListener();
    }

    /// <summary>
    /// Called when the page disappears; removes the snapshot listener to ensure
    /// efficient resource management.
    /// </summary>
    protected override void OnDisappearing()
    {
        mpVM.RemoveSnapshotListener();
        base.OnDisappearing();
    }

    #endregion
}