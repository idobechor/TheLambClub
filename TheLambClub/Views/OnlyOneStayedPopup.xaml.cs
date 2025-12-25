

using CommunityToolkit.Maui.Views;
using TheLambClub.ViewModel;

namespace TheLambClub.Views;

public partial class OnlyOneStayedPopup : Popup
{
    private readonly WinnerOfOnlyOneStaiedPopupVM WinnerOfOnlyOneStaiedPopupVM;
    public OnlyOneStayedPopup(string name)
    {
        InitializeComponent();
        WinnerOfOnlyOneStaiedPopupVM = new WinnerOfOnlyOneStaiedPopupVM(name);
        BindingContext = WinnerOfOnlyOneStaiedPopupVM;
    }
}