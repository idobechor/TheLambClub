using System.Windows.Input;
using TheLambClub.ModelsLogic;
using TheLambClub.Models;
using TheLambClub.Views;

namespace TheLambClub.ViewModel
{
    public partial class LoginPageVM : ObservableObject
    {
        private readonly User user = new();
        public ICommand LoginCommand { get; }
        public ICommand ToggleIsPasswordCommand { get; }
        public bool IsChecked
        {
            get => user.IsChecked;
            set => user.IsChecked = value;
        }
        public LoginPageVM() 
        {
            LoginCommand=new Command(Login,CanLogin);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            user.OnAuthComplete += OnAuthComplete;
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
            user.Login();
        }
        public bool CanLogin()
        {
           return user.CanLogin();           
        }
        public bool IsPassword { get; set; } = true;
        public string Password 
        { get=> user.Password;
            set
            {
                user.Password = value;
                (LoginCommand as Command)?.ChangeCanExecute();
            }        
        }
        public string UserName 
        { get=> user.UserName;
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
    }
}
