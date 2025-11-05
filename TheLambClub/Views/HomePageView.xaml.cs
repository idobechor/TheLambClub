using TheLambClub.ViewModel;

namespace TheLambClub.Views;

public partial class HomePageView : ContentPage
{
    private readonly HomePageVM hpVM = new();
    public HomePageView()
	{
		InitializeComponent();
        BindingContext = hpVM;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        hpVM.AddSnapshotListener();
    }

    protected override void OnDisappearing()
    {
        hpVM.RemoveSnapshotListener();
        base.OnDisappearing();
    }
}