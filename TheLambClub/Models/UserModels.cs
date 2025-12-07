using CommunityToolkit.Maui.Behaviors;
using Firebase.Auth;
using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    public abstract class UserModels
    {
        public EventHandler? OnAuthComplete;
        public FbData fbd = new();
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Age { get; set; } = string.Empty;
        public bool IsChecked { get;set;}=true;       
        public abstract void Register();
        public abstract void Login();
        public abstract bool CanLogin();
        public abstract bool CanRegister();
        public abstract string GetFirebaseErrorMessage(string msg);
        public bool IsRegistered => (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))||!IsChecked;
    }
}
