namespace TheLambClub.Models
{
    /// <summary>
    /// Provides a centralized static storage for application-wide constant keys, 
    /// configuration values, and resource identifiers.
    /// </summary>
    public static class Keys
    {
        #region fields

        /// <summary>
        /// The API key for accessing OpenAI services.
        /// </summary>
        public const string OpenAIApiKey = "";

        /// <summary>
        /// Key used for storing or retrieving the user's name.
        /// </summary>
        public const string UserNameKey = "Username";

        /// <summary>
        /// Key used for storing or retrieving the user's password.
        /// </summary>
        public const string PasswordNameKey = "Password";

        /// <summary>
        /// Key used for storing or retrieving the user's email address.
        /// </summary>
        public const string EmailNameKey = "Email";

        /// <summary>
        /// Key used for storing or retrieving the user's age.
        /// </summary>
        public const string AgeNameKey = "Age";

        /// <summary>
        /// The API key used for Firebase project authentication.
        /// </summary>
        public const string FbApiKey = "AIzaSyA1H5fWVJzTVoqsayJe9QUoWVpzv2btBh0";

        /// <summary>
        /// The domain URL for the Firebase application.
        /// </summary>
        public const string FBappDomainKey = "thelambclub-2bf8c.firebaseapp.com";

        /// <summary>
        /// The name of the Firestore collection where game data is stored.
        /// </summary>
        public const string GamesCollection = "game";

        /// <summary>
        /// The filename or resource path for the card back image.
        /// </summary>
        public const string BackOfCard = "backofcard.jpg";

        /// <summary>
        /// The default AI model version used for processing.
        /// </summary>
        public const string DefaultModel = "gpt-4o-mini";

        /// <summary>
        /// Error message displayed when the OpenAI API key is missing.
        /// </summary>
        public const string ApiKeyDosentSet = "API key not set. Add your OpenAI key in app settings.";

        /// <summary>
        /// The total duration for a game timer in milliseconds.
        /// </summary>
        public const int TimerTotalTime = 31000;

        /// <summary>
        /// The interval rate for timer ticks in milliseconds.
        /// </summary>
        public const int TimerInterval = 1000;

        /// <summary>
        /// Represents one hour converted into milliseconds.
        /// </summary>
        public const int OneHourInMilliseconds = 3600000;

        /// <summary>
        /// The starting balance for a new player.
        /// </summary>
        public const int InitialMoney = 10000;

        /// <summary>
        /// The starting amount of money in the pot at the beginning of a game.
        /// </summary>
        public const int InitialPotsMoney = 0;

        /// <summary>
        /// Signal value indicating that a process or timer has finished.
        /// </summary>
        public const long FinishedSignal = -1000;

        /// <summary>
        /// Represents one hour in milliseconds (duplicate entry for compatibility).
        /// </summary>
        public const int OneHourInMillisconds = 3600000;

        #endregion
    }
}