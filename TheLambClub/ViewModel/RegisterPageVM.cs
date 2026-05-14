using TheLambClub.Models;
using TheLambClub.ModelsLogic;
using TheLambClub.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Windows.Input;

namespace TheLambClub.ViewModel
{
    /// <summary>
    /// ViewModel for the Registration Page. Manages user registration input, 
    /// validation commands, and UI state feedback for the registration process.
    /// </summary>
    public class RegisterPageVM : ObservableObject
    {
        #region fields

        private readonly User user = new();
        private bool isBusy;

        #endregion

        #region commands

        /// <summary>
        /// Command to initiate the user registration process.
        /// </summary>
        public ICommand RegisterCommand { get; }

        /// <summary>
        /// Command to toggle the visibility of the password field.
        /// </summary>
        public ICommand ToggleIsPasswordCommand { get; }

        #endregion

        #region properties

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }

        public bool IsPassword { get; set; } = true;

        public string UserName
        {
            get => user.UserName;
            set
            {
                user.UserName = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }

        public string Password
        {
            get => user.Password;
            set
            {
                user.Password = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }

        public string Email
        {
            get => user.Email;
            set
            {
                user.Email = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }

        public string Age
        {
            get => user.Age;
            set
            {
                user.Age = value;
                (RegisterCommand as Command)?.ChangeCanExecute();
            }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Initializes the RegisterPageVM, binds command logic, and subscribes to authentication events.
        /// </summary>
        public RegisterPageVM()
        {
            RegisterCommand = new Command(Register, CanRegister);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            user.OnAuthComplete += OnAuthComplete;
            user.ShowToastAlert += ShowToastAlert;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Determines if the registration command can be executed based on field validation and busy state.
        /// </summary>
        public bool CanRegister()
        {
            return !IsBusy && user.CanRegister();
        }

        #endregion

        #region private methods

        /// <summary>
        /// Displays an error message toast when registration fails.
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
        /// Handles successful registration by switching the application main page to AppShell.
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
        /// Triggers the user registration process if the system is not currently busy.
        /// </summary>
        private void Register()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                user.Register();
            }
        }

        #endregion
    }
}