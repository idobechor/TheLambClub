using TheLambClub.ViewModel;

namespace TheLambClub.Views;

public partial class HomePageView : ContentPage
{
	public HomePageView()
	{
		InitializeComponent();
        BindingContext = new HomePageVM();
    }
}