using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using CommunityToolkit.Mvvm.Messaging;
using TheLambClub.Models;

namespace TheLambClub.Platforms.Android
{
    /// <summary>
    /// The main entry point for the Android application. Handles activity lifecycle,
    /// background service initialization, and messaging for timer controls.
    /// </summary>
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        #region fields

        private MyTimer? mTimer;

        #endregion

        #region protected methods

        /// <summary>
        /// Called when the activity is first created. Initializes background services
        /// and registers message listeners for app-wide communication.
        /// </summary>
        /// <param name="savedInstanceState">Bundle containing the activity's previously frozen state, if any.</param>
        override protected void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RegisterTimerMessages();
            StartDeleteFBDocsService();
        }

        #endregion

        #region private methods

        /// <summary>
        /// Starts the background service responsible for deleting old Firestore documents.
        /// </summary>
        private void StartDeleteFBDocsService()
        {
            Intent = new Intent(this, typeof(DeleteFbDocsService));
            StartService(Intent);
        }

        /// <summary>
        /// Registers listeners for timer-related messages using the WeakReferenceMessenger.
        /// This allows the UI or logic layers to trigger or stop the Android-specific timer.
        /// </summary>
        private void RegisterTimerMessages()
        {
            // Listener for starting the timer with specific settings
            WeakReferenceMessenger.Default.Register<AppMessage<TimerSettings>>(this, (r, m) =>
            {
                OnMessageReceived(m.Value);
            });
            // Listener for stopping/canceling the timer
            WeakReferenceMessenger.Default.Register<AppMessage<bool>>(this, (r, m) =>
            {
                OnMessageReceived(m.Value);
            });
        }

        /// <summary>
        /// Handles boolean messages to cancel and clear the current timer instance.
        /// </summary>
        /// <param name="value">If true, the timer will be canceled.</param>
        private void OnMessageReceived(bool value)
        {
            if (value)
            {
                mTimer?.Cancel();
                mTimer = null;
            }
        }

        /// <summary>
        /// Handles TimerSettings messages to initialize and start a new CountDownTimer instance.
        /// </summary>
        /// <param name="value">Object containing total duration and tick interval.</param>
        private void OnMessageReceived(TimerSettings value)
        {
            mTimer = new MyTimer(value.TotalTimeInMillSeconds, value.IntervalTimeInMillSeconds);
            mTimer.Start();
        }

        #endregion
    }
}