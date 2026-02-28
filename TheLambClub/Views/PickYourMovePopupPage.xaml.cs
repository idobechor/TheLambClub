using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using TheLambClub.ModelsLogic;
using TheLambClub.Services;
using TheLambClub.ViewModel;

namespace TheLambClub.Views;

public partial class PickYourMovePopupPage : Popup
{
    #region fields

    private readonly PickYourMovePopupPageVM PromptYourMoveVM;

    #endregion

    #region constructors

    public PickYourMovePopupPage(Game game, IPokerSuggestionService? suggestionService)
    {
        InitializeComponent();
        PromptYourMoveVM = new PickYourMovePopupPageVM(game, TimeLeft, suggestionService);
        BindingContext = PromptYourMoveVM;
        PromptYourMoveVM.RequestClose += OnRequestClose;
    }

    #endregion

    #region private methods

    private void OnRequestClose()
    {
        PromptYourMoveVM.Close();
        Close();
    }

    #endregion
}
