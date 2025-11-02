using Firebase.Auth;
using Firebase.Auth.Providers;
using Plugin.CloudFirestore;

namespace TheLambClub.Models
{
    abstract class FbDataModel
    {
        protected FirebaseAuthClient facl;
        protected IFirestore fs;
        public abstract string DisplayName { get; }
        public abstract string UserId { get; }
        public abstract void CreateUserWithEmailAndPasswordAsync(string email, string password, string name, Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void SignInWithEmailAndPasswordAsync(string email, string password, Action<System.Threading.Tasks.Task> OnComplete);
        public abstract string SetDocument(object obj, string collectonName, string id, Action<System.Threading.Tasks.Task> OnComplete);
        public FbDataModel() 
        {
            FirebaseAuthConfig fac = new()
            {
                ApiKey = Keys.FbApiKey,
                AuthDomain = Keys.FBappDomainKey,
                Providers = [new EmailProvider()]
            };
            facl = new FirebaseAuthClient(fac);
            fs = CrossCloudFirestore.Current.Instance;
        }
    }
}
