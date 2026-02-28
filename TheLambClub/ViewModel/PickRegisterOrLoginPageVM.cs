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
        #region commands

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        #endregion

        #region constructors

        public PickRegisterOrLoginPageVM()
        {
            LoginCommand = new Command(MoveToLoginPage);
            RegisterCommand = new Command(MoveToRegisterPage);
        }

        #endregion

        #region private methods

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

        #endregion
    }
}
