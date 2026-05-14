using CommunityToolkit.Maui.Core;
using System.Windows.Input;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    /// <summary>
    /// ViewModel for the 'Win Game' popup. Handles the display of the winning result 
    /// and provides navigation logic to return home.
    /// </summary>
    public class WinGamePopupVM
    {

        #region commands

        /// <summary>
        /// Command to navigate the user back to the home screen.
        /// </summary>
        public ICommand? MoveToHome { get; }

        #endregion

        #region properties

        /// <summary>
        /// Gets the winning result message to be displayed on the popup.
        /// </summary>
        public string? ResultMessage {  get; }

        #endregion

        #region constructors

        /// <summary>
        /// Initializes the ViewModel with the provided winning game text and sets up the home navigation command.
        /// </summary>
        /// <param name="winningText">The message indicating the game win.</param>
        public WinGamePopupVM(string winningText)
        {
            ResultMessage = winningText;
            MoveToHome = new Command(MoveToHomeFunction);
        }

        /// <summary>
        /// Default constructor for the WinGamePopupVM.
        /// </summary>
        public WinGamePopupVM()
        {
        }

        #endregion

        #region private methods

        /// <summary>
        /// Navigates the application back to the previous screen (home) on the main thread.
        /// </summary>
        /// <param name="obj">The command parameter (unused).</param>
        private void MoveToHomeFunction(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PopAsync();
            });
        }

        #endregion
    }
}