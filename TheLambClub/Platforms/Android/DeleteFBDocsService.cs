using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Plugin.CloudFirestore;
using TheLambClub.Models;
using TheLambClub.ModelsLogic;

namespace TheLambClub.Platforms.Android
{
    /// <summary>
    /// An Android Background Service responsible for periodically cleaning up old game documents 
    /// from Firebase Firestore to maintain database health.
    /// </summary>
    [Service]
    public class DeleteFbDocsService : Service
    {
        #region fields

        private bool isRunning = true;
        private readonly FbData fbd = new();

        #endregion

        #region public methods

        /// <summary>
        /// Triggered when the service is started. It initializes a background thread 
        /// to perform the deletion logic without blocking the main UI thread.
        /// </summary>
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent? intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            ThreadStart threadStart = new(DeleteFbDocs);
            Thread thread = new(threadStart);
            thread.Start();
            return base.OnStartCommand(intent, flags, startId);
        }

        /// <summary>
        /// Binding is not supported for this service as it runs independently in the background.
        /// </summary>
        public override IBinder? OnBind(Intent? intent)
        {
            // Not used
            return null;
        }

        /// <summary>
        /// Triggered when the service is destroyed. Sets the running flag to false 
        /// to gracefully exit the background loop.
        /// </summary>
        public override void OnDestroy()
        {
            isRunning = false;
            base.OnDestroy();
        }

        #endregion

        #region private methods

        /// <summary>
        /// The core loop of the service. Queries Firebase for documents older than 24 hours 
        /// and repeats the check every hour.
        /// </summary>
        private void DeleteFbDocs()
        {
            while (isRunning)
            {
                fbd.GetDocumentsWhereLessThan(Keys.GamesCollection, nameof(GameModel.Created), DateTime.Now.AddDays(-1), OnComplete);
                Thread.Sleep(Keys.OneHourInMillisconds);
            }
            StopSelf();
        }

        /// <summary>
        /// Callback triggered when the Firestore query completes. 
        /// Iterates through the resulting snapshots and deletes each document by ID.
        /// </summary>
        /// <param name="qs">The snapshot of documents matching the query criteria.</param>
        private void OnComplete(IQuerySnapshot qs)
        {
            foreach (IDocumentSnapshot doc in qs.Documents)
                fbd.DeleteDocument(Keys.GamesCollection, doc.Id, (task) => { });
        }

        #endregion
    }
}