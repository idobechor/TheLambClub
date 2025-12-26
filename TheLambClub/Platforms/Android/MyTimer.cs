using Android.OS;
using Android.Runtime;
using CommunityToolkit.Mvvm.Messaging;
using TheLambClub.Models;

namespace TheLambClub.Platforms.Android
{
    public class MyTimer(long millisInFuture, long countDownInterval) : CountDownTimer(millisInFuture, countDownInterval)
    {
        private const long FinishedSignal = -1000;
        public override void OnFinish()
        {
            WeakReferenceMessenger.Default.Send(new AppMessage<long>(FinishedSignal));
        }

        public override void OnTick(long millisUntilFinished)
        {
            WeakReferenceMessenger.Default.Send(new AppMessage<long>(millisUntilFinished));

        }
    }
}
