using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    /// <summary>
    /// Handles user authentication, registration, and local preference management.
    /// </summary>
    public class User : UserModels
    {
        #region constructors
        /// <summary>
        /// Initializes a new instance of the User class and retrieves saved credentials from local preferences.
        /// </summary>
        public User()
        {
            UserName = Preferences.Get(Keys.UserNameKey, string.Empty);
            Password = Preferences.Get(Keys.PasswordNameKey, string.Empty);
            Email = Preferences.Get(Keys.EmailNameKey, string.Empty);
            Age = Preferences.Get(Keys.AgeNameKey, string.Empty);
        }
        #endregion
        #region public methods
        /// <summary>
        /// Initiates the registration process with Firebase using email, password, and username.
        /// </summary>
        public override void Register()
        {
            fbd.CreateUserWithEmailAndPasswordAsync(Email, Password, UserName, OnComplete);
        }
        /// <summary>
        /// Parses Firebase exception messages and maps them to localized user-friendly error strings.
        /// </summary>
        /// <param name="msg">The raw error message from the Firebase exception.</param>
        /// <returns>A formatted error string for the UI.</returns>
        public override string GetFirebaseErrorMessage(string msg)
        {
            string result = string.Empty;
            if (msg.Contains(Strings.ErrMessageReason))
            {
                if (msg.Contains(Strings.EmailExists))
                    result = Strings.EmailExistsErrMsg;
                if (msg.Contains(Strings.InvalidEmailAddress))
                    result = Strings.InvalidEmailErrMessage;
                if (msg.Contains(Strings.WeakPassword))
                    result = Strings.WeakPasswordErrMessage;
                if (msg.Contains(Strings.UserNotFound))
                    result = Strings.UserNotFoundmsg;
            }
            return result;
        }
        /// <summary>
        /// Validates that all required registration fields are filled correctly and that the age is a valid number.
        /// </summary>
        /// <returns>True if the user can proceed with registration; otherwise, false.</returns>
        public override bool CanRegister()
        {
            return (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Age)) && !string.IsNullOrWhiteSpace(Email) && double.TryParse(Age, out _);
        }
        /// <summary>
        /// Handles the login process. Saves or clears local preferences based on the "Remember Me" status.
        /// </summary>
        public override void Login()
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
        /// <summary>
        /// Checks if the minimum required fields for login (Username, Password, Email) are populated.
        /// </summary>
        /// <returns>True if the fields are valid; otherwise, false.</returns>
        public override bool CanLogin()
        {
            return (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email));
        }
        #endregion

        #region protected methods

        /// <summary>
        /// Triggers the alert event to display a message to the user.
        /// </summary>
        /// <param name="msg">The message to display.</param>
        protected override void ShowAlert(string msg)
        {
            ShowToastAlert?.Invoke(this, msg);
        }
        #endregion

        #region private methods
        /// <summary>
        /// Callback triggered when a Firebase task (Login/Register) completes.
        /// Handles success and error scenarios.
        /// </summary>
        /// <param name="task">The authentication task.</param>
        private void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                if (IsChecked)
                    SaveToPreferences();
                OnAuthComplete?.Invoke(this, EventArgs.Empty);
            }
            else if (task.Exception != null)
                ShowAlert(GetFirebaseErrorMessage(task.Exception.Message));
            else
                ShowAlert(Strings.UnknownRegistrationFailedError);
        }
        /// <summary>
        /// Saves all current user details to local device storage.
        /// </summary>
        private void SaveToPreferences()
        {
            Preferences.Set(Keys.UserNameKey, UserName);
            Preferences.Set(Keys.PasswordNameKey, Password);
            Preferences.Set(Keys.EmailNameKey, Email);
            Preferences.Set(Keys.AgeNameKey, Age);
        }
        #endregion
    }
}