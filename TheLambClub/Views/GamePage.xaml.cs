using TheLambClub.ModelsLogic;
using TheLambClub.ViewModel;

namespace TheLambClub.Views;

public partial class GamePage : ContentPage
{
    #region fields

    private readonly GamePageVM gpVM;

    #endregion

    #region constructors

    public GamePage(Game game)
    {
        InitializeComponent();
        gpVM = new GamePageVM(game, grdOponnents);
        BindingContext = gpVM;
    }

    #endregion

    #region protected methods

    protected override void OnAppearing()
    {
        base.OnAppearing();
        gpVM.AddSnapshotListener();
    }

    protected override void OnDisappearing()
    {
        gpVM.RemoveSnapshotListener();
        base.OnDisappearing();
    }

    #endregion
}
