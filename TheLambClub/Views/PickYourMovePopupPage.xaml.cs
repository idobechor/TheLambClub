
using CommunityToolkit.Maui.Views;
using TheLambClub.ModelsLogic;
using TheLambClub.ViewModel;

namespace TheLambClub.Views;

public partial class PickYourMovePopupPage  : Popup
{
    private readonly PickYourMovePromptPageVM PromptYourMoveVM;
    public PickYourMovePopupPage(Game game)
	{
		InitializeComponent();
       
        PromptYourMoveVM = new PickYourMovePromptPageVM(game);
        BindingContext= PromptYourMoveVM;
        PromptYourMoveVM.RequestClose += OnRequestClose;
    }

    private void OnRequestClose()
    {
        PromptYourMoveVM.Close();
        Close(); // closes the popup
       // ((Command))(new GamePageVM()).ShowPickYourMovePrompt.;
    }
}