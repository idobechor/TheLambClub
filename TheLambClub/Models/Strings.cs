namespace TheLambClub.Models
{
    /// <summary>
    /// Provides a centralized static repository for all localized and constant string values used throughout the application.
    /// </summary>
    static class Strings
    {
        #region fields

        /// <summary>
        /// Text for the "Remember me" checkbox on the login screen.
        /// </summary>
        public const string RememberMeCheckBoxText = "Remember me:";

        /// <summary>
        /// Label for the login submission button.
        /// </summary>
        public const string LoginButton = "Login";

        /// <summary>
        /// Label for the registration navigation or submission.
        /// </summary>
        public const string Register = "Register";

        /// <summary>
        /// Header or title text for the login section.
        /// </summary>
        public const string LoginText = "Log in";

        /// <summary>
        /// Placeholder or label for the username input field.
        /// </summary>
        public const string UserName = "User name";

        /// <summary>
        /// Prompt for entering a password.
        /// </summary>
        public const string EnterPassword = "Password:";

        /// <summary>
        /// Prompt for entering a username.
        /// </summary>
        public const string EnterUserName = "UserName:";

        /// <summary>
        /// Prompt for entering an email address.
        /// </summary>
        public const string EnterEmail = "Email:";

        /// <summary>
        /// Prompt for entering an age.
        /// </summary>
        public const string EnterAge = "Age:";

        /// <summary>
        /// General label for Password.
        /// </summary>
        public const string Password = "Password";

        /// <summary>
        /// General label for Email.
        /// </summary>
        public const string Email = "Email";

        /// <summary>
        /// General label for Age.
        /// </summary>
        public const string Age = "Age";

        /// <summary>
        /// Text for a generic confirmation button.
        /// </summary>
        public const string Ok = "Ok";

        /// <summary>
        /// Label for entering a private game room.
        /// </summary>
        public const string PrivateRoomButtonTxt = "private room";

        /// <summary>
        /// Label for the game creation button.
        /// </summary>
        public const string CreateGameButtonTxt = "Create";

        /// <summary>
        /// Formatted welcome message for the main dashboard.
        /// </summary>
        public const string WelcomeTxt = "Hello {0} Welcome to TheLambClub";

        /// <summary>
        /// The name of the application.
        /// </summary>
        public const string TheLambClub = "TheLambClub";

        /// <summary>
        /// Title for the custom room code input dialog.
        /// </summary>
        public const string customRoomCodeTitleTxt = "custom room code";

        /// <summary>
        /// Prompt text asking for a custom room code.
        /// </summary>
        public const string customRoomCodeTxt = "please the code to sign";

        /// <summary>
        /// Title for the instructions section.
        /// </summary>
        public const string InsructionsTxtTitle = "Insructions";

        /// <summary>
        /// Detailed gameplay instructions and rules for Texas Hold'em.
        /// </summary>
        public const string InsructionsTxt = "Texas Hold'em Poker is the most popular form of poker in the world. The goal of the game is to build the strongest possible five-card hand or make the other players fold. Each player receives two private cards that only they can see, and throughout the game, five community cards are " +
               "revealed on the table. The game is divided into four main stages: Pre-Flop, where each player is dealt two cards; Flop, where three community cards are placed on the table; Turn, where a fourth card is added; and River, where the fifth and final card is revealed. After each stage, there is a betting round in which players " +
               "can choose to fold, call, check, or raise. At the end of the game, if two or more players remain, everyone reveals their cards, and the best five-card hand wins the pot. The hands are ranked from strongest to weakest as follows: Royal Flush, Straight Flush, Four of a Kind, Full House, Flush, Straight, Three of a Kind, Two Pair, One Pair, and finally High Card." +
               "Texas Hold'em combines luck, strategy, and psychology, making it one of the most exciting and challenging card games in the world.";

        /// <summary>
        /// Generic message for an unspecified error.
        /// </summary>
        public const string UnknownErrorMessage = "Unknown Error";

        /// <summary>
        /// Key or label for weak password identification.
        /// </summary>
        public const string WeakPassword = "WeakPassword";

        /// <summary>
        /// Error message explaining password requirements.
        /// </summary>
        public const string WeakPasswordErrMessage = "Password needs to be at least 6 charecters long";

        /// <summary>
        /// Error message for an improperly formatted email address.
        /// </summary>
        public const string InvalidEmailErrMessage = "Invalid Email address";

        /// <summary>
        /// Header for the reason part of an error message.
        /// </summary>
        public const string ErrMessageReason = "Reason";

        /// <summary>
        /// Key for existing email conflict during registration.
        /// </summary>
        public const string EmailExists = "EmailExists";

        /// <summary>
        /// Key for an invalid email address state.
        /// </summary>
        public const string InvalidEmailAddress = "InvalidEmailAddress";

        /// <summary>
        /// User-facing message when an email is already registered.
        /// </summary>
        public const string EmailExistsErrMsg = "This email is already in use";

        /// <summary>
        /// Generic failure message for registration.
        /// </summary>
        public const string UnknownRegistrationFailedError = "Unknown Error";

        /// <summary>
        /// Firebase-style error code for invalid login credentials.
        /// </summary>
        public const string UserNotFound = "INVALID_LOGIN_CREDENTIALS";

        /// <summary>
        /// Message displayed when login fails due to wrong email/password.
        /// </summary>
        public const string UserNotFoundmsg = "Email or Password is incorrect";

        /// <summary>
        /// Error message when joining a game fails.
        /// </summary>
        public const string JoinGameErr = "Error joining game";

        /// <summary>
        /// Message indicating the game was deleted.
        /// </summary>
        public const string GameDeleted = "Deleted";

        /// <summary>
        /// Title for the player count picker.
        /// </summary>
        public const string PickerTitle = "number of players";

        /// <summary>
        /// Game status indicating the lobby is waiting for more players.
        /// </summary>
        public const string WaitingStatus = "Waiting for Players";

        /// <summary>
        /// Game status indicating a match is currently in progress.
        /// </summary>
        public const string PlayingStatus = "Playing";

        /// <summary>
        /// Text for a button used to manually transition turns (for testing or hosting).
        /// </summary>
        public const string MoveTurnButttonText = "Next Turn";

        /// <summary>
        /// Label for the 'Stay' or 'Call' action button.
        /// </summary>
        public const string StayBtnTxt = "Stay";

        /// <summary>
        /// Label for the 'Fold' action button.
        /// </summary>
        public const string FoldBtnTxt = "Fold";

        /// <summary>
        /// Instructional text prompting the user to take their turn.
        /// </summary>
        public const string PickMoveTxt = "Play Please";

        /// <summary>
        /// Label for the bet submission button.
        /// </summary>
        public const string SubmitBetBtn = "Submit Bet";

        /// <summary>
        /// Text displayed when the player's turn timer expires.
        /// </summary>
        public const string TimeUp = "Time Up";

        /// <summary>
        /// Shortened waiting status message.
        /// </summary>
        public const string WaitingForPlayers = "Waiting";

        /// <summary>
        /// Prefix for displaying another player's hand rank.
        /// </summary>
        public const string HisRankIs = "His Hand Rank:";

        /// <summary>
        /// Text for the 'Call' betting action.
        /// </summary>
        public const string Call = "call ";

        /// <summary>
        /// Text for the 'Check' betting action.
        /// </summary>
        public const string Check = "Check";

        /// <summary>
        /// Header for announcing the winner of a hand.
        /// </summary>
        public const string IntoruceTheWinner = "The Winner is:";

        /// <summary>
        /// Label for showing the current amount a player has bet.
        /// </summary>
        public const string IntoruceYourBet = "your bet amount is:";

        /// <summary>
        /// Salutation used in messages.
        /// </summary>
        public const string Dear = "Dear ";

        /// <summary>
        /// Message displayed upon winning a game event.
        /// </summary>
        public const string WinningMsg = "you won the game event well done";

        /// <summary>
        /// Message displayed upon losing a game.
        /// </summary>
        public const string LosingMsg = "you Lost the game please try again";

        /// <summary>
        /// Label for the current active player's name.
        /// </summary>
        public const string CurrentTurnTxt = "Current Player:";

        /// <summary>
        /// Victory header text.
        /// </summary>
        public const string YouWonTxt = "You Won";

        /// <summary>
        /// Defeat header text.
        /// </summary>
        public const string YouLostTxt = "You Lost";

        /// <summary>
        /// Label for navigating home.
        /// </summary>
        public const string Home = "Home";

        /// <summary>
        /// Title for the screen displaying round statistics.
        /// </summary>
        public const string RoundResultTxt = "Round Results";

        /// <summary>
        /// Text for a generic 'Close' button.
        /// </summary>
        public const string Close = "Close";

        /// <summary>
        /// Text for the button that requests AI strategy advice.
        /// </summary>
        public const string GetAISuggestion = "Get AI suggestion";

        /// <summary>
        /// Message shown when the AI service is unreachable or errors out.
        /// </summary>
        public const string SuggestionUnavailable = "Suggestion unavailable.";

        /// <summary>
        /// Prefix for the text returned by the AI suggestion service.
        /// </summary>
        public const string AiSuggestsPrefix = "AI suggests: ";

        /// <summary>
        /// System instruction prompt defining the persona and constraints for the OpenAI model.
        /// </summary>
        public const string userPromptExp = $@"You are a strict Poker Logic Engine acting as an API backend.
    Your goal is to output raw data metrics based on GTO principles. 
    DO NOT provide explanations, conversational text, or markdown formatting.";

        /// <summary>
        /// Logic rules and formatting requirements sent to the AI model.
        /// </summary>
        public const string Rulls = "Logic Rules:\n1. CURRENT STRENGTH (1-10):\n   - IF PRE-FLOP: Evaluate hole cards based on raw potential. High scores (8-10) for High Pairs (AA-JJ), AK/AQ. Mid scores (5-7) for Suited Connectors, Ace-X, or Low-Mid Pairs. High cards and same-suit/sequential cards increase this score.\n   - IF POST-FLOP: Evaluate RELATIVE advantage. If the board pairs or connects, do not overrate strength unless you have the best kickers or full house.\n2. ACTION MAPPING: 'Check'/'Call'/'Fold' = 0% Bet. Only 'Raise' if you have a range advantage or the nuts.\n3. BET SIZING: Standard: 10-40% pot. Use >40% only for 'The Nuts' or polarized bluffs. 100% = All-in.\n4. DANGER: High (8-10) if the board is 'wet' (3+ of same suit/sequence) or hand is easily beaten by a single higher card.\n*** CRITICAL OUTPUT INSTRUCTIONS ***\n- You must reply ONLY with the format below.\n- NO N/A values. Use 1-10 for Strength in all stages.\n- Do not add any text before or after the metrics.\nExpected Output Format:\nAction: [Raise, Call, Check, Fold]\nCurrent Strength: [1-10]\nDanger Level: [1-10]\nRecommended Bet: [0-100]%";

        /// <summary>
        /// Text representing a null or empty state.
        /// </summary>
        public const string None = "(none)";

        /// <summary>
        /// Prefix used when describing the cards in a player's hand.
        /// </summary>
        public const string PlayerHandPrefix = "Player hand: ";

        /// <summary>
        /// Prefix used when describing community board cards.
        /// </summary>
        public const string BoardPrefix = "Board: ";

        /// <summary>
        /// Joiner word for card descriptions (e.g., "Ace of Spades").
        /// </summary>
        public const string OfJoiner = " of ";

        /// <summary>
        /// Plural suffix for card suits.
        /// </summary>
        public const string PluralS = "s";

        /// <summary>
        /// Stage name: before any community cards are dealt.
        /// </summary>
        public const string StagePreFlop = "Pre-flop";

        /// <summary>
        /// Stage name: first three community cards.
        /// </summary>
        public const string StageFlop = "Flop";

        /// <summary>
        /// Stage name: fourth community card.
        /// </summary>
        public const string StageTurn = "Turn";

        /// <summary>
        /// Stage name: fifth community card.
        /// </summary>
        public const string StageRiver = "River";

        /// <summary>
        /// Message for an undefined game stage.
        /// </summary>
        public const string StageUnknown = "Unknown";

        /// <summary>
        /// Card value name: Ace.
        /// </summary>
        public const string ValAce = "Ace";

        /// <summary>
        /// Card value name: Two.
        /// </summary>
        public const string ValTwo = "Two";

        /// <summary>
        /// Card value name: Three.
        /// </summary>
        public const string ValThree = "Three";

        /// <summary>
        /// Card value name: Four.
        /// </summary>
        public const string ValFour = "Four";

        /// <summary>
        /// Card value name: Five.
        /// </summary>
        public const string ValFive = "Five";

        /// <summary>
        /// Card value name: Six.
        /// </summary>
        public const string ValSix = "Six";

        /// <summary>
        /// Card value name: Seven.
        /// </summary>
        public const string ValSeven = "Seven";

        /// <summary>
        /// Card value name: Eight.
        /// </summary>
        public const string ValEight = "Eight";

        /// <summary>
        /// Card value name: Nine.
        /// </summary>
        public const string ValNine = "Nine";

        /// <summary>
        /// Card value name: Ten.
        /// </summary>
        public const string ValTen = "Ten";

        /// <summary>
        /// Card value name: Jack.
        /// </summary>
        public const string ValJack = "Jack";

        /// <summary>
        /// Card value name: Queen.
        /// </summary>
        public const string ValQueen = "Queen";

        /// <summary>
        /// Card value name: King.
        /// </summary>
        public const string ValKing = "King";

        /// <summary>
        /// Duplicate entry for AI suggestion prefix.
        /// </summary>
        public const string AiSuggestionTxt = "AI suggests: ";

        /// <summary>
        /// Default message when an AI response is not available.
        /// </summary>
        public const string DefaultUnavailableMessage = "Suggestion unavailable.";

        /// <summary>
        /// Header for instructions section.
        /// </summary>
        public const string Instructions = "Instructions";

        /// <summary>
        /// Header for creating a new game table.
        /// </summary>
        public const string StartNewTable = "START A NEW TABLE";

        /// <summary>
        /// Header for the list of existing game tables.
        /// </summary>
        public const string ActiveTables = "ACTIVE TABLES";

        #endregion
    }
}