using Android.App;
using Android.Runtime;

namespace TheLambClub.Platforms.Android
{
    [Application]
    public class MainApplication(nint handle, JniHandleOwnership ownership) : MauiApplication(handle, ownership)
    {
        #region protected methods
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
        #endregion
    }
}
