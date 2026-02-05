using CommunityToolkit.Maui.Views;
using TheLambClub.ViewModel;

namespace TheLambClub.Views;

public partial class LostGamePopup : Popup
{
    public LostGamePopup(string losingText)
    {
        BindingContext = new LostGamePopupVM(losingText);
        InitializeComponent();
    }
}