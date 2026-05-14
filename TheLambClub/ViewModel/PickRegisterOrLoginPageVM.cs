using System.Windows.Input;
using TheLambClub.Views;

namespace TheLambClub.ViewModel
{
    /// <summary>
    /// ViewModel for the entry selection page. Handles navigation between the 
    /// Login and Registration pages.
    /// </summary>
    public class PickRegisterOrLoginPageVM
    {
        #region commands

        /// <summary>
        /// Command to navigate to the Login page.
        /// </summary>
        public ICommand LoginCommand { get; }

        /// <summary>
        /// Command to navigate to the Registration page.
        /// </summary>
        public ICommand RegisterCommand { get; }

        #endregion

        #region constructors

        /// <summary>
        /// Initializes the ViewModel and binds the navigation commands.
        /// </summary>
        public PickRegisterOrLoginPageVM()
        {
            LoginCommand = new Command(MoveToLoginPage);
            RegisterCommand = new Command(MoveToRegisterPage);
        }

        #endregion

        #region private methods

        /// <summary>
        /// Navigates the application to the RegisterPage on the main thread.
        /// </summary>
        /// <param name="obj">The command parameter (unused).</param>
        private void MoveToRegisterPage(object obj)
        {
            if (Application.Current != null)
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Application.Current!.MainPage = new RegisterPage();
                });
        }

        /// <summary>
        /// Navigates the application to the LoginPage on the main thread.
        /// </summary>
        /// <param name="obj">The command parameter (unused).</param>
        private void MoveToLoginPage(object obj)
        {
            if (Application.Current != null)
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Application.Current!.MainPage = new LoginPage();
                });
        }

        #endregion
    }
}