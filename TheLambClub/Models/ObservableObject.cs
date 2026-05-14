using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TheLambClub.Models;

/// <summary>
/// Provides a base implementation for objects that require property change notifications 
/// to support data binding, implementing the <see cref="INotifyPropertyChanged"/> interface.
/// </summary>
public class ObservableObject : INotifyPropertyChanged
{
    #region events

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Displays an alert containing game instructions to the user.
    /// </summary>
    public static void ShowInstructionsPrompt()
    {
        Application.Current!.MainPage!.DisplayAlert(Strings.InsructionsTxtTitle, Strings.InsructionsTxt, Strings.Ok);
    }

    #endregion

    #region protected methods

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event for a specific property.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed. Automatically populated via <see cref="CallerMemberNameAttribute"/>.</param>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}