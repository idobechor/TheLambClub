using System.Windows.Input;
using TheLambClub.ModelsLogic;
using TheLambClub.Models;

namespace TheLambClub.ViewModel
{
    internal partial class LoginPageVM : ObservableObject
    {
        public ICommand LoginCommand { get; }
        public ICommand ToggleIsPasswordCommand { get; }
        private bool IsCheckedValue;

        public bool IsChecked
        {
            get => IsCheckedValue;
            set => IsCheckedValue = value;
        }
        public LoginPageVM() 
        {
            LoginCommand=new Command(Login,CanLogin);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
        }
        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }
        private void Login()
        {
            user.Login(IsCheckedValue);
        }
        public bool CanLogin()
        {
           return user.CanLogin();           
        }
        public bool IsPassword { get; set; } = true;
        private readonly User user = new();
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
