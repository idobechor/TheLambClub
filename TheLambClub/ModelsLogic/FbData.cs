using Plugin.CloudFirestore;
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    /// <summary>
    /// Implements the Firebase data provider, handling authentication, document management, 
    /// and real-time listeners using the Cloud Firestore plugin.
    /// </summary>
    public class FbData : FbDataModel
    {
        #region properties

        /// <summary>
        /// Gets the display name of the currently authenticated user. 
        /// Returns an empty string if no user is logged in.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                string dn = string.Empty;
                if (facl.User != null)
                    dn = facl.User.Info.DisplayName;
                return dn;
            }
        }

        /// <summary>
        /// Gets the unique identifier (UID) of the currently authenticated user.
        /// </summary>
        public override string UserId => facl.User.Uid;

        #endregion

        #region public methods

        /// <summary>
        /// Asynchronously creates a new user account with the specified email, password, and display name.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's chosen password.</param>
        /// <param name="name">The display name to be associated with the account.</param>
        /// <param name="OnComplete">A callback action to execute when the task completes.</param>
        public override async void CreateUserWithEmailAndPasswordAsync(string email, string password, string name, Action<System.Threading.Tasks.Task> OnComplete)
        {
            await facl.CreateUserWithEmailAndPasswordAsync(email, password, name).ContinueWith(OnComplete);
        }

        /// <summary>
        /// Asynchronously signs into an existing account using an email and password.
        /// </summary>
        /// <param name="email">The user's registered email address.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="OnComplete">A callback action to execute when the sign-in task completes.</param>
        public override async void SignInWithEmailAndPasswordAsync(string email, string password, Action<System.Threading.Tasks.Task> OnComplete)
        {
            await facl.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(OnComplete);
        }

        /// <summary>
        /// Sets or creates a document in a specified collection with a given object and ID.
        /// </summary>
        /// <param name="obj">The data object to store.</param>
        /// <param name="collectonName">The name of the target Firestore collection.</param>
        /// <param name="id">The document ID. If null or empty, a new ID is generated automatically.</param>
        /// <param name="OnComplete">A callback action to execute when the operation completes.</param>
        /// <returns>The ID of the document that was set.</returns>
        public override string SetDocument(object obj, string collectonName, string id, Action<System.Threading.Tasks.Task> OnComplete)
        {
            IDocumentReference dr = string.IsNullOrEmpty(id) ? fs.Collection(collectonName).Document() : fs.Collection(collectonName).Document(id);
            dr.SetAsync(obj).ContinueWith(OnComplete);
            return dr.Id;
        }

        /// <summary>
        /// Registers a real-time listener for a specific Firestore collection.
        /// </summary>
        /// <param name="collectonName">The name of the collection to monitor.</param>
        /// <param name="OnChange">A handler invoked whenever the collection data changes.</param>
        /// <returns>A registration object used to stop the listener.</returns>
        public override IListenerRegistration AddSnapshotListener(string collectonName, Plugin.CloudFirestore.QuerySnapshotHandler OnChange)
        {
            ICollectionReference cr = fs.Collection(collectonName);
            return cr.AddSnapshotListener(OnChange);
        }

        /// <summary>
        /// Retrieves documents from a collection where a specific field is less than the provided value.
        /// </summary>
        /// <param name="collectonName">The target collection.</param>
        /// <param name="fName">The field name to filter by.</param>
        /// <param name="fValue">The value for the comparison.</param>
        /// <param name="OnComplete">A callback action to execute with the resulting query snapshot.</param>
        public override async void GetDocumentsWhereLessThan(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete)
        {
            ICollectionReference cr = fs.Collection(collectonName);
            IQuerySnapshot qs = await cr.WhereLessThan(fName, fValue).GetAsync();
            OnComplete(qs);
        }

        /// <summary>
        /// Registers a real-time listener for changes to a specific Firestore document.
        /// </summary>
        /// <param name="collectonName">The name of the collection.</param>
        /// <param name="id">The unique ID of the document.</param>
        /// <param name="OnChange">A handler invoked whenever the document data changes.</param>
        /// <returns>A registration object used to stop the listener.</returns>
        public override IListenerRegistration AddSnapshotListener(string collectonName, string id, Plugin.CloudFirestore.DocumentSnapshotHandler OnChange)
        {
            IDocumentReference cr = fs.Collection(collectonName).Document(id);
            return cr.AddSnapshotListener(OnChange);
        }

        /// <summary>
        /// Retrieves documents from a collection where a specific field equals the provided value.
        /// </summary>
        /// <param name="collectonName">The target collection.</param>
        /// <param name="fName">The field name to filter by.</param>
        /// <param name="fValue">The value for the equality check.</param>
        /// <param name="OnComplete">A callback action to execute with the resulting query snapshot.</param>
        public async void GetDocumentsWhereEqualTo(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete)
        {
            ICollectionReference cr = fs.Collection(collectonName);
            IQuerySnapshot qs = await cr.WhereEqualsTo(fName, fValue).GetAsync();
            OnComplete(qs);
        }

        /// <summary>
        /// Updates specific fields within an existing Firestore document.
        /// </summary>
        /// <param name="collectonName">The name of the collection.</param>
        /// <param name="id">The document ID.</param>
        /// <param name="dict">A dictionary containing field names and their new values.</param>
        /// <param name="OnComplete">A callback action to execute when the update completes.</param>
        public override async void UpdateFields(string collectonName, string id, Dictionary<string, object> dict, Action<Task> OnComplete)
        {
            IDocumentReference dr = fs.Collection(collectonName).Document(id);
            await dr.UpdateAsync(dict).ContinueWith(OnComplete);
        }

        /// <summary>
        /// Deletes a specific document from a Firestore collection.
        /// </summary>
        /// <param name="collectonName">The name of the collection.</param>
        /// <param name="id">The document ID to remove.</param>
        /// <param name="OnComplete">A callback action to execute when the deletion completes.</param>
        public override async void DeleteDocument(string collectonName, string id, Action<Task> OnComplete)
        {
            IDocumentReference dr = fs.Collection(collectonName).Document(id);
            await dr.DeleteAsync().ContinueWith(OnComplete);
        }

        #endregion
    }
}