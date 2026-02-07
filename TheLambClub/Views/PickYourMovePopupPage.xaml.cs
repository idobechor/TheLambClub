
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using TheLambClub.ModelsLogic;
using TheLambClub.Services;
using TheLambClub.ViewModel;

namespace TheLambClub.Views;

public partial class PickYourMovePopupPage  : Popup
{
    private readonly PickYourMovePromptPageVM PromptYourMoveVM;
    public PickYourMovePopupPage(Game game, IPokerSuggestionService? suggestionService)
	{
		InitializeComponent();   
        PromptYourMoveVM = new PickYourMovePromptPageVM(game, TimeLeft, suggestionService);
        BindingContext= PromptYourMoveVM;
        PromptYourMoveVM.RequestClose += OnRequestClose;
    }
    private void OnRequestClose()
    {
        PromptYourMoveVM.Close();
        Close(); 
    }
}