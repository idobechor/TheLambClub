using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    internal class User : UserModels
    {
        public override void Register()
        {
            fbd.CreateUserWithEmailAndPasswordAsync(Email, UserName, Password, OnComplete);              
        }

        private void OnComplete(Task task)
        {            
                if (task.IsCompletedSuccessfully)
                {
                    SaveToPreferences();
                 OnAuthComplete?.Invoke(this, EventArgs.Empty);
                }
                else if (task.Exception != null)
                {
                string msg = task.Exception.Message;
                ShowAlert(GetFirebaseErrorMessage(msg));                   
                }
                else
                ShowAlert(Strings.UnknownRegistrationFailedError);                         
        }
        public override  string GetFirebaseErrorMessage(string msg)
        {
            if (msg.Contains(Strings.ErrMessageReason))
            {
                if (msg.Contains(Strings.EmailExists))
                    return Strings.EmailExistsErrMsg;
                if (msg.Contains(Strings.InvalidEmailAddress))
                    return Strings.InvalidEmailErrMessage;
                if (msg.Contains(Strings.WeakPassword))
                    return Strings.WeakPasswordErrMessage;
            }
            return Strings.UnknownErrorMessage;
        }

        private static void ShowAlert(string msg)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(msg, ToastDuration.Long).Show();
            });
        }

        private void SaveToPreferences()
        {
            Preferences.Set(Keys.UserNameKey, UserName);
            Preferences.Set(Keys.PasswordNameKey, Password);
            Preferences.Set(Keys.EmailNameKey, Email);
            Preferences.Set(Keys.AgeNameKey, Age);
        }

        public override bool CanRegister()
        {
            return (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Age));
        }
        public override void Login()
        {
            Preferences.Set(Keys.UserNameKey, UserName);
            Preferences.Set(Keys.PasswordNameKey, Password);
            Preferences.Set(Keys.EmailNameKey, Email);
        }
      
        public override bool CanLogin()
        {
            return (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password)&&!string.IsNullOrWhiteSpace(Email));
        }
  
        public User()
        {
            UserName = Preferences.Get(Keys.UserNameKey, string.Empty);
            Password = Preferences.Get(Keys.PasswordNameKey, string.Empty);
            Email = Preferences.Get(Keys.EmailNameKey, string.Empty);
            Age = Preferences.Get(Keys.AgeNameKey, string.Empty);
        }
    }
}
