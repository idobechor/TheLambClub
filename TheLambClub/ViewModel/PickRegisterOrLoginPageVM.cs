using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TheLambClub.NewFolder;
using TheLambClub.Views;

namespace TheLambClub.ViewModel
{
    public class PickRegisterOrLoginPageVM
    {
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public PickRegisterOrLoginPageVM()
        {
            LoginCommand = new Command(MoveToLoginPage);
            RegisterCommand = new Command(MoveToRegisterPage);
        }

        private void MoveToRegisterPage(object obj)
        {
            if (Application.Current != null)
            {
                if (Application.Current != null)
                {
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Application.Current.MainPage = new RegisterPage();
                    });
                }
            }
        }

        private void MoveToLoginPage(object obj)
        {
            if (Application.Current != null)
            {
                if (Application.Current != null)
                {
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Application.Current.MainPage = new LoginPage();
                    });
                }
            }
        }
    }
}
