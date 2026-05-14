using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TheLambClub.Models
{
    /// <summary>
    /// Represents a generic application message used for communication between different parts of the system.
    /// </summary>
    public class AppMessage<T>(T msg) : ValueChangedMessage<T>(msg)
    {
    }
}