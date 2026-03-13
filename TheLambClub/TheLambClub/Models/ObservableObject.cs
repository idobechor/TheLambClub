using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace TheLambClub.Models;
public class ObservableObject : INotifyPropertyChanged
{
    #region events

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    #region protected methods

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
