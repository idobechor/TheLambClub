 using TheLambClub.ModelsLogic;
using System.Windows.Input;
using TheLambClub.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using TheLambClub.Views;

namespace TheLambClub.ViewModel
{
    public class RegisterPageVM: ObservableObject
    {
        public ICommand RegisterCommand { get; }
        public ICommand ToggleIsPasswordCommand { get; }
        private readonly User user = new();
        public bool IsPassword { get; set; } = true;
        public bool CanRegister()
        {
            return user.CanRegister();
        }
        public RegisterPageVM()
        {
            RegisterCommand=new Command(Register, CanRegister);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            user.OnAuthComplete += OnAuthComplete;
            
        }

        private void OnAuthComplete(object? sender, EventArgs e)
        {
            if (Application.Current != null)
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Application.Current.MainPage = new HomePageView();
                });
            }
        }
        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }
        private void Register()
        {
            user.Register();
        }        
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
        
    }
}
