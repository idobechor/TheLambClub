using CommunityToolkit.Maui.Views;
using TheLambClub.ViewModel;

namespace TheLambClub.Views;

public partial class LostGamePopupPage : Popup
{
    #region constructors

    public LostGamePopupPage(string losingText)
    {
        BindingContext = new LostGamePopupVM(losingText);
        InitializeComponent();
    }

    #endregion
}
