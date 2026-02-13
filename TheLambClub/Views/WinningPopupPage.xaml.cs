using CommunityToolkit.Maui.Views;
using TheLambClub.ModelsLogic;
using TheLambClub.ViewModel;

namespace TheLambClub.Views;

public partial class WinningPopupPage : Popup
{
    private readonly WinningPopupVM winningPopupVM;
    public WinningPopupPage(Player[] Players, Dictionary<Player, HandRank> ranks, int numUpWinners)
    {
        InitializeComponent();
        winningPopupVM = new WinningPopupVM(Players,ranks, numUpWinners);
        BindingContext = winningPopupVM;
        winningPopupVM.RequestClose += OnRequestClose;
    }
    private void OnRequestClose()
    {
        Close();
    }
}