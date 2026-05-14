using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    /// <summary>
    /// Serves as the abstract base model for user-related operations, 
    /// handling authentication data, registration logic, and identity state.
    /// </summary>
    public abstract class UserModels
    {
        #region fields

        /// <summary>
        /// An instance of <see cref="FbData"/> used for interacting with Firebase services.
        /// </summary>
        public FbData fbd = new();

        #endregion

        #region events

        /// <summary>
        /// Occurs when an authentication process (Login or Registration) is successfully completed.
        /// </summary>
        public EventHandler? OnAuthComplete;

        /// <summary>
        /// Occurs when a toast notification or a short alert message needs to be displayed to the user.
        /// </summary>
        public EventHandler<string>? ShowToastAlert;

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the display name or username for the account.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the account password.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address associated with the user account.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's age as a string value.
        /// </summary>
        public string Age { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the user has checked a specific option (e.g., "Remember Me").
        /// </summary>
        public bool IsChecked { get; set; } = true;

        #endregion

        #region public methods

        /// <summary>
        /// Initiates the registration process for a new user account.
        /// </summary>
        public abstract void Register();

        /// <summary>
        /// Initiates the login process for an existing user account.
        /// </summary>
        public abstract void Login();

        /// <summary>
        /// Validates whether the current property values meet the requirements to attempt a login.
        /// </summary>
        /// <returns><c>true</c> if login can proceed; otherwise, <c>false</c>.</returns>
        public abstract bool CanLogin();

        /// <summary>
        /// Validates whether the current property values meet the requirements to attempt a registration.
        /// </summary>
        /// <returns><c>true</c> if registration can proceed; otherwise, <c>false</c>.</returns>
        public abstract bool CanRegister();

        /// <summary>
        /// Translates raw Firebase error messages into user-friendly localized strings.
        /// </summary>
        /// <param name="msg">The raw error message from the database provider.</param>
        /// <returns>A formatted, readable error description.</returns>
        public abstract string GetFirebaseErrorMessage(string msg);

        #endregion

        #region protected methods

        /// <summary>
        /// Displays an alert dialog to the user with a specific message.
        /// </summary>
        /// <param name="msg">The message to display in the alert.</param>
        protected abstract void ShowAlert(string msg);

        #endregion
    }
}