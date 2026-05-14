using TheLambClub.Models;
using TheLambClub.ModelsLogic;
using TheLambClub.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Windows.Input;

namespace TheLambClub.ViewModel
{
    /// <summary>
    /// ViewModel for the Login Page. Manages user input, authentication commands, 
    /// and UI state feedback for the login process.
    /// </summary>
    public partial class LoginPageVM : ObservableObject
    {
        #region fields

        private readonly User user = new();
        private bool isBusy;

        #endregion

        #region commands

        /// <summary>
        /// Command to execute the user login process.
        /// </summary>
        public ICommand LoginCommand { get; }

        /// <summary>
        /// Command to toggle the visibility of the password field.
        /// </summary>
        public ICommand ToggleIsPasswordCommand { get; }

        #endregion

        #region properties

        public bool IsChecked
        {
            get => user.IsChecked;
            set => user.IsChecked = value;
        }
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
                (LoginCommand as Command)?.ChangeCanExecute();
            }
        }

        public bool IsPassword { get; set; } = true;

        public string Password
        {
            get => user.Password;
            set
            {
                user.Password = value;
                (LoginCommand as Command)?.ChangeCanExecute();
            }
        }

        public string UserName
        {
            get => user.UserName;
            set
            {
                user.UserName = value;
                (LoginCommand as Command)?.ChangeCanExecute();
            }
        }

        public string Email
        {
            get => user.Email;
            set
            {
                user.Email = value;
                (LoginCommand as Command)?.ChangeCanExecute();
            }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Initializes the LoginPageVM, sets up command logic, and subscribes to authentication events.
        /// </summary>
        public LoginPageVM()
        {
            LoginCommand = new Command(Login, CanLogin);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            user.OnAuthComplete += OnAuthComplete;
            user.ShowToastAlert += ShowToastAlert;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Determines if the login command can be executed based on field validation and busy state.
        /// </summary>
        public bool CanLogin()
        {
            return user.CanLogin() && !isBusy;
        }

        #endregion

        #region private methods

        /// <summary>
        /// Displays an error message toast when authentication fails.
        /// </summary>
        private void ShowToastAlert(object? sender, string msg)
        {
            isBusy = false;
            OnPropertyChanged(nameof(isBusy));
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(msg, ToastDuration.Long).Show();
            });
        }

        /// <summary>
        /// Handles successful authentication by switching the application main page to AppShell.
        /// </summary>
        private void OnAuthComplete(object? sender, EventArgs e)
        {
            if (Application.Current != null)
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Application.Current.MainPage = new AppShell();
                });
            }
        }

        /// <summary>
        /// Switches the password field between masked and visible states.
        /// </summary>
        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }

        /// <summary>
        /// Triggers the user login process if the system is not currently busy.
        /// </summary>
        private void Login()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                user.Login();
            }
        }

        #endregion
    }
}