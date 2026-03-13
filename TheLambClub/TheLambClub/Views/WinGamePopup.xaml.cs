using CommunityToolkit.Maui.Views;
using TheLambClub.ViewModel;
namespace TheLambClub.Views;
public partial class WinGamePopup : Popup
{
    #region fields

    private readonly WinGamePopupVM winGamePopupVM;

    #endregion

    #region constructors

    public WinGamePopup(string WinningText)
    {
        InitializeComponent();
        winGamePopupVM = new WinGamePopupVM(WinningText);
        BindingContext = winGamePopupVM;
        winGamePopupVM.RequestClose += OnRequestClose;
    }

    #endregion

    #region private methods

    private void OnRequestClose()
    {
        Close();
    }

    #endregion
}
