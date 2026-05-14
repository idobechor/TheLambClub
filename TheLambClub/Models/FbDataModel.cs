using Firebase.Auth;
using Firebase.Auth.Providers;
using Plugin.CloudFirestore;

namespace TheLambClub.Models
{
    /// <summary>
    /// Serves as an abstract base class for Firebase data operations, providing core authentication and Firestore instances.
    /// </summary>
    public abstract class FbDataModel
    {
        #region fields

        /// <summary>
        /// The Firebase authentication client used for managing user sessions.
        /// </summary>
        protected FirebaseAuthClient facl;

        /// <summary>
        /// The Cloud Firestore instance used for database operations.
        /// </summary>
        protected IFirestore fs;

        #endregion

        #region properties

        /// <summary>
        /// Gets the display name associated with the user.
        /// </summary>
        public abstract string DisplayName { get; }

        /// <summary>
        /// Gets the unique identifier for the user.
        /// </summary>
        public abstract string UserId { get; }

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FbDataModel"/> class and configures Firebase services.
        /// </summary>
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

        /// <summary>
        /// Creates a new user account with the specified email, password, and name.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="name">The user's full name.</param>
        /// <param name="OnComplete">Callback action to execute when the task completes.</param>
        public abstract void CreateUserWithEmailAndPasswordAsync(string email, string password, string name, Action<System.Threading.Tasks.Task> OnComplete);

        /// <summary>
        /// Signs in an existing user using their email and password.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="OnComplete">Callback action to execute when the task completes.</param>
        public abstract void SignInWithEmailAndPasswordAsync(string email, string password, Action<System.Threading.Tasks.Task> OnComplete);

        /// <summary>
        /// Sets or overwrites a document in a specific Firestore collection.
        /// </summary>
        /// <param name="obj">The object to store in the document.</param>
        /// <param name="collectonName">The name of the Firestore collection.</param>
        /// <param name="id">The unique ID for the document.</param>
        /// <param name="OnComplete">Callback action to execute when the task completes.</param>
        /// <returns>The ID of the document.</returns>
        public abstract string SetDocument(object obj, string collectonName, string id, Action<System.Threading.Tasks.Task> OnComplete);

        /// <summary>
        /// Subscribes to real-time updates for an entire Firestore collection.
        /// </summary>
        /// <param name="collectonName">The name of the collection to listen to.</param>
        /// <param name="OnChange">Handler to process collection snapshot updates.</param>
        /// <returns>A registration object to manage the listener's lifecycle.</returns>
        public abstract IListenerRegistration AddSnapshotListener(string collectonName, Plugin.CloudFirestore.QuerySnapshotHandler OnChange);

        /// <summary>
        /// Subscribes to real-time updates for a specific Firestore document.
        /// </summary>
        /// <param name="collectonName">The name of the collection.</param>
        /// <param name="id">The unique ID of the document.</param>
        /// <param name="OnChange">Handler to process document snapshot updates.</param>
        /// <returns>A registration object to manage the listener's lifecycle.</returns>
        public abstract IListenerRegistration AddSnapshotListener(string collectonName, string id, Plugin.CloudFirestore.DocumentSnapshotHandler OnChange);

        /// <summary>
        /// Updates specific fields within a Firestore document without overwriting the entire document.
        /// </summary>
        /// <param name="collectonName">The name of the collection.</param>
        /// <param name="id">The unique ID of the document.</param>
        /// <param name="dict">A dictionary containing the field names and their new values.</param>
        /// <param name="OnComplete">Callback action to execute when the task completes.</param>
        public abstract void UpdateFields(string collectonName, string id, Dictionary<string, object> dict, Action<Task> OnComplete);

        /// <summary>
        /// Retrieves documents from a collection where a specific field's value is less than the provided value.
        /// </summary>
        /// <param name="collectonName">The name of the collection.</param>
        /// <param name="fName">The field name to filter by.</param>
        /// <param name="fValue">The value for comparison.</param>
        /// <param name="OnComplete">Callback action to execute with the resulting query snapshot.</param>
        public abstract void GetDocumentsWhereLessThan(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete);

        /// <summary>
        /// Deletes a specific document from a Firestore collection.
        /// </summary>
        /// <param name="collectonName">The name of the collection.</param>
        /// <param name="id">The unique ID of the document to delete.</param>
        /// <param name="OnComplete">Callback action to execute when the task completes.</param>
        public abstract void DeleteDocument(string collectonName, string id, Action<Task> OnComplete);

        #endregion
    }
}