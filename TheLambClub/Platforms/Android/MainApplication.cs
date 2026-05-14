using Android.App;
using Android.Runtime;

namespace TheLambClub.Platforms.Android
{
    /// <summary>
    /// Represents the main Android application class. 
    /// This class is responsible for initializing the MAUI application lifecycle on the Android platform.
    /// </summary>
    [Application]
    public class MainApplication(nint handle, JniHandleOwnership ownership) : MauiApplication(handle, ownership)
    {
        #region protected methods

        /// <summary>
        /// Bootstraps the MAUI application by calling the shared MauiProgram configuration.
        /// </summary>
        /// <returns>The initialized MauiApp instance.</returns>
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        #endregion
    }
}