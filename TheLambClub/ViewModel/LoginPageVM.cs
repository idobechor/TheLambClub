using TheLambClub.Models;
using TheLambClub.ModelsLogic;
using TheLambClub.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Windows.Input;

namespace TheLambClub.ViewModel
{
    public partial class LoginPageVM : ObservableObject
    {
        #region fields

        private readonly User user = new();
        private bool isBusy;

        #endregion

        #region commands

        public ICommand LoginCommand { get; }
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

        public LoginPageVM()
        {
            LoginCommand = new Command(Login, CanLogin);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            user.OnAuthComplete += OnAuthComplete;
            user.ShowToastAlert += ShowToastAlert;
        }

        #endregion

        #region public methods

        public bool CanLogin()
        {
            return user.CanLogin() && !isBusy;
        }

        #endregion

        #region private methods

        private void ShowToastAlert(object? sender, string msg)
        {
            isBusy = false;
            OnPropertyChanged(nameof(isBusy));
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(msg, ToastDuration.Long).Show();
            });
        }
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
        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }
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
