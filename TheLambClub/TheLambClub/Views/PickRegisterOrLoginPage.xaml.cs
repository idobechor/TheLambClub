using TheLambClub.ViewModel;

namespace TheLambClub.Views;

public partial class PickRegisterOrLoginPage : ContentPage
{
    #region constructors

    public PickRegisterOrLoginPage()
    {
        InitializeComponent();
        BindingContext = new PickRegisterOrLoginPageVM();
    }

    #endregion
}
