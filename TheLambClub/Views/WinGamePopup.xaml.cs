using CommunityToolkit.Maui.Views;
using TheLambClub.ViewModel;
namespace TheLambClub.Views;
public partial class WinGamePopup : Popup
{
	public WinGamePopup( string WinningText)
	{
        InitializeComponent();
        BindingContext = new WinGamePopupVM(WinningText);        
    }
}