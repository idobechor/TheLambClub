using TheLambClub.ModelsLogic;
using TheLambClub.ViewModel;

namespace TheLambClub.Views;

public partial class GamePage : ContentPage
{
    public GamePage(Game game )
	{
		InitializeComponent();
		BindingContext = new GamePageVM(game);
    }
}