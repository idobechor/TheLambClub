
using CommunityToolkit.Maui.Views;
using TheLambClub.ViewModel;

namespace TheLambClub.Views;

public partial class PickYourMovePopupPage  : Popup
{
	public PickYourMovePopupPage()
	{
		InitializeComponent();
        BindingContext = new PickYourMovePopupVM();
    }
}