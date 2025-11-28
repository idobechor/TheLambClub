using TheLambClub.ViewModel;

namespace TheLambClub.Views;

public partial class PickRegisterOrLoginPage : ContentPage
{
	public PickRegisterOrLoginPage()
	{
		InitializeComponent();
        BindingContext = new PickRegisterOrLoginPageVM();
    }
}