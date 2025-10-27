using System.Windows.Input;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    internal class HomePageVM
    {
        private readonly HomePage homePage = new();
        public ICommand ShowNumericPromptCommand { get; private set; }
        public ICommand InstructionsCommand { get; private set; }
        public ICommand StartGameCommand { get => new Command(MoveToGamePage); }
        public HomePageVM()
        {
            ShowNumericPromptCommand = new Command(ShowNumericPromptCasting);
            InstructionsCommand = new Command(ShowInstructionsPrompt);
        }       
        private void MoveToGamePage()
        {
            homePage.MoveToGamePage();
        }
        public void ShowNumericPromptCasting(object obj)
        {
            homePage.ShowNumericPromptCasting(obj);
        }
        public void ShowInstructionsPrompt(object obj)
        {
            homePage.ShowInstructionsPrompt(obj);
        }
    }
}
