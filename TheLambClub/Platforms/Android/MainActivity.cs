using Android.App;
using Android.Content.PM;
using Android.OS;
using CommunityToolkit.Mvvm.Messaging;
using TheLambClub.Models;
using TheLambClub.Platforms.Android;

namespace TheLambClub
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            WeakReferenceMessenger.Default.Register<AppMessage<TimerSettings>>(this, (r, m) =>
            {
                OnMessageRecieived(m.Value);
            });
        }

        private static void OnMessageRecieived(TimerSettings value)
        {
            _=new MyTimer (value.TotalTimeInMillSeconds,value.IntervalTimeInMillSeconds).Start ();
            
        }
    }
}
