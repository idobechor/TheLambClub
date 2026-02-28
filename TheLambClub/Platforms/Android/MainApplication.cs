using Android.App;
using Android.Runtime;

namespace TheLambClub
{
    [Application]
    public class MainApplication : MauiApplication
    {
        #region constructors

        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        #endregion

        #region protected methods

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        #endregion
    }
}
