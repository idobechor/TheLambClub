using TheLambClub.ModelsLogic;
namespace TheLambClub.Models
{
    public abstract class UserModels
    {
        #region fields

        public FbData fbd = new();

        #endregion

        #region events

        public EventHandler? OnAuthComplete;
        public EventHandler<string>? ShowToastAlert;

        #endregion

        #region properties

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Age { get; set; } = string.Empty;
        public bool IsChecked { get; set; } = true;
        public bool IsRegistered => (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password)) || !IsChecked;

        #endregion

        #region public methods

        public abstract void Register();
        public abstract void Login();
        public abstract bool CanLogin();
        public abstract bool CanRegister();
        public abstract string GetFirebaseErrorMessage(string msg);

        #endregion

        #region protected methods

        protected abstract void ShowAlert(string msg);

        #endregion
    }
}
