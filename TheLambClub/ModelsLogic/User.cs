using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    internal class User : UserModels
    {
        public override void Register()
        {
            fbd.CreateUserWithEmailAndPasswordAsync(Email, Password , UserName, OnComplete);              
        }

        private void OnComplete(Task task)
        {            
                if (task.IsCompletedSuccessfully)
                {
                     SaveToPreferences();
                     OnAuthComplete?.Invoke(this, EventArgs.Empty);
                }
                else if (task.Exception != null)
                    ShowAlert(GetFirebaseErrorMessage(task.Exception.Message));                   
                else
                    ShowAlert(Strings.UnknownRegistrationFailedError);                         
        }
        public override  string GetFirebaseErrorMessage(string msg)
        {
            string result = string.Empty;
            if (msg.Contains(Strings.ErrMessageReason))
            {
                if (msg.Contains(Strings.EmailExists))
                    result= Strings.EmailExistsErrMsg;
                if (msg.Contains(Strings.InvalidEmailAddress))
                    result= Strings.InvalidEmailErrMessage;
                if (msg.Contains(Strings.WeakPassword))
                    result= Strings.WeakPasswordErrMessage;
                if(msg.Contains(Strings.UserNotFound))
                    result= Strings.UserNotFoundmsg;
            }
            return result;
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
            return (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Age)) && !string.IsNullOrWhiteSpace(Email) && double.TryParse(Age, out double ageNumber);
        }
        public override void Login(bool IsChecked)
        {
            if (IsChecked)
            {           
             Preferences.Set(Keys.UserNameKey, UserName);
            Preferences.Set(Keys.PasswordNameKey, Password);
            Preferences.Set(Keys.EmailNameKey, Email);
            }
            else
             Preferences.Clear();

            fbd.SignInWithEmailAndPasswordAsync(Email, Password, OnComplete);
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
