using CommunityToolkit.Maui.Views;
using TheLambClub.ModelsLogic;
using TheLambClub.ViewModel;

namespace TheLambClub.Views;

public partial class WinningPopupPage : Popup
{
    #region fields

    private readonly WinningPopupVM winningPopupVM;

    #endregion

    #region constructors

    public WinningPopupPage(Player[] Players, Dictionary<Player, HandRank> ranks, int numUpWinners)
    {
        InitializeComponent();
        winningPopupVM = new WinningPopupVM(Players, ranks, numUpWinners);
        BindingContext = winningPopupVM;
        winningPopupVM.RequestClose += OnRequestClose;
    }

    #endregion

    #region private methods

    private void OnRequestClose()
    {
        Close();
    }

    #endregion
}
