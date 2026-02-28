using CommunityToolkit.Maui.Views;
using TheLambClub.ViewModel;

namespace TheLambClub.Views;

public partial class WinGamePopupPage : Popup
{
    #region fields

    private readonly WinGamePopupVM winGamePopupVM;

    #endregion

    #region constructors

    public WinGamePopupPage(string WinningText)
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
