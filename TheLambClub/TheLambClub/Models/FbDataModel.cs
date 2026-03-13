using Firebase.Auth;
using Firebase.Auth.Providers;
using Plugin.CloudFirestore;

namespace TheLambClub.Models
{
    public abstract class FbDataModel
    {
        #region fields

        protected FirebaseAuthClient facl;
        protected IFirestore fs;

        #endregion

        #region properties

        public abstract string DisplayName { get; }
        public abstract string UserId { get; }

        #endregion

        #region constructors

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

        #endregion

        #region public methods

        public abstract void CreateUserWithEmailAndPasswordAsync(string email, string password, string name, Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void SignInWithEmailAndPasswordAsync(string email, string password, Action<System.Threading.Tasks.Task> OnComplete);
        public abstract string SetDocument(object obj, string collectonName, string id, Action<System.Threading.Tasks.Task> OnComplete);
        public abstract IListenerRegistration AddSnapshotListener(string collectonName, Plugin.CloudFirestore.QuerySnapshotHandler OnChange);
        public abstract IListenerRegistration AddSnapshotListener(string collectonName, string id, Plugin.CloudFirestore.DocumentSnapshotHandler OnChange);
        public abstract void UpdateFields(string collectonName, string id, Dictionary<string, object> dict, Action<Task> OnComplete);
        public abstract void GetDocumentsWhereLessThan(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete);
        public abstract void DeleteDocument(string collectonName, string id, Action<Task> OnComplete);

        #endregion
    }
}
