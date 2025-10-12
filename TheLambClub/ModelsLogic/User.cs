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
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    SaveToPreferences();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Registration Failed",
                        "An error occurred while creating your account. Please try again.",
                        "OK"
                    );
                }
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
