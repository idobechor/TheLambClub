using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TheLambClub.Models
{
    public class AppMessage<T>(T msg):ValueChangedMessage<T>(msg)
    {
    }
}
