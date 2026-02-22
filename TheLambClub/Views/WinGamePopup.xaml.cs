using CommunityToolkit.Maui.Views;
using TheLambClub.ViewModel;
namespace TheLambClub.Views;
public partial class WinGamePopup : Popup
{
    private readonly WinGamePopupVM winGamePopupVM;
    public WinGamePopup( string WinningText)
	{
        InitializeComponent();
        winGamePopupVM= new WinGamePopupVM(WinningText);
        BindingContext = winGamePopupVM;
        winGamePopupVM.RequestClose += OnRequestClose;
    }
    private void OnRequestClose()
    {
        Close();
    }
}