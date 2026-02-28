namespace TheLambClub.Models
{
    static class Strings
    {
        #region fields

        public const string RememberMeCheckBoxText = "Remember me:";
        public const string LoginButton = "Login";
        public const string Register = "Register";
        public const string LoginText = "Log in";
        public const string UserName = "User name";
        public const string EnterPassword = "Password:";
        public const string EnterUserName = "UserName:";
        public const string EnterEmail = "Email:";
        public const string EnterAge = "Age:";
        public const string Password = "Password";
        public const string Email = "Email";
        public const string Age = "Age";
        public const string Ok = "Ok";
        public const string PrivateRoomButtonTxt = "private room";
        public const string CreateGameButtonTxt = "Create game";
        public const string WelcomeTxt = "Hello {0} Welcome to TheLambClub";
        public const string TheLambClub = "TheLambClub";
        public const string customRoomCodeTitleTxt = "custom room code";
        public const string customRoomCodeTxt = "please the code to sign";
        public const string InsructionsTxtTitle = "Insructions";
        public const string InsructionsTxt = "Texas Hold'em Poker is the most popular form of poker in the world. The goal of the game is to build the strongest possible five-card hand or make the other players fold. Each player receives two private cards that only they can see, and throughout the game, five community cards are " +
               "revealed on the table. The game is divided into four main stages: Pre-Flop, where each player is dealt two cards; Flop, where three community cards are placed on the table; Turn, where a fourth card is added; and River, where the fifth and final card is revealed. After each stage, there is a betting round in which players " +
               "can choose to fold, call, check, or raise. At the end of the game, if two or more players remain, everyone reveals their cards, and the best five-card hand wins the pot. The hands are ranked from strongest to weakest as follows: Royal Flush, Straight Flush, Four of a Kind, Full House, Flush, Straight, Three of a Kind, Two Pair, One Pair, and finally High Card." +
               "Texas Hold'em combines luck, strategy, and psychology, making it one of the most exciting and challenging card games in the world.";
        public const string UnknownErrorMessage = "Unknown Error";
        public const string WeakPassword = "WeakPassword";
        public const string WeakPasswordErrMessage = "Password needs to be at least 6 charecters long";
        public const string InvalidEmailErrMessage = "Invalid Email address";
        public const string ErrMessageReason = "Reason";
        public const string EmailExists = "EmailExists";
        public const string InvalidEmailAddress = "InvalidEmailAddress";
        public const string EmailExistsErrMsg = "This email is already in use";
        public const string UnknownRegistrationFailedError = "Unknown Error";
        public const string UserNotFound = "INVALID_LOGIN_CREDENTIALS";
        public const string UserNotFoundmsg = "User not found";
        public const string JoinGameErr = "Error joining game";
        public const string GameDeleted = "Deleted";
        public const string PickerTitle = "number of players";
        public const string WaitingStatus = "Waiting for Players";
        public const string PlayingStatus = "Playing";
        public const string MoveTurnButttonText = "Next Turn";
        public const string StayBtnTxt = "Stay";
        public const string FoldBtnTxt = "Fold";
        public const string PickMoveTxt = "Play Please";
        public const string SubmitBetBtn = "Submit Bet";
        public const string TimeUp = "Time Up";
        public const string WaitingForPlayers = "Waiting";
        public const string HisRankIs = "HisHandRank-";
        public const string Call = "call ";
        public const string Check = "Check";
        public const string IntoruceTheWinner = "The Winner is:";
        public const string IntoruceYourBet = "your bet amount is:";
        public const string Dear = "Dear ";
        public const string WinningMsg = "you won the game event well done";
        public const string LosingMsg = "you Lost the game please try again";
        public const string CurrentTurnTxt = "Current Player:";
        public const string YouWonTxt = "You Won";
        public const string YouLostTxt = "You Lost";
        public const string Home = "Home";
        public const string RoundResultTxt = "Round Results";
        public const string Close = "Close";
        public const string GetAISuggestion = "Get AI suggestion";
        public const string SuggestionUnavailable = "Suggestion unavailable.";
        public const string AiSuggestsPrefix = "AI suggests: ";
        public const string userPromptExp = $@"You are a strict Poker Logic Engine acting as an API backend.
    Your goal is to output raw data metrics based on GTO principles. 
    DO NOT provide explanations, conversational text, or markdown formatting.";
        public const string Rulls = "Logic Rules:\n1. CURRENT STRENGTH (1-10):\n   - IF PRE-FLOP: Evaluate hole cards based on raw potential. High scores (8-10) for High Pairs (AA-JJ), AK/AQ. Mid scores (5-7) for Suited Connectors, Ace-X, or Low-Mid Pairs. High cards and same-suit/sequential cards increase this score.\n   - IF POST-FLOP: Evaluate RELATIVE advantage. If the board pairs or connects, do not overrate strength unless you have the best kickers or full house.\n2. ACTION MAPPING: 'Check'/'Call'/'Fold' = 0% Bet. Only 'Raise' if you have a range advantage or the nuts.\n3. BET SIZING: Standard: 10-40% pot. Use >40% only for 'The Nuts' or polarized bluffs. 100% = All-in.\n4. DANGER: High (8-10) if the board is 'wet' (3+ of same suit/sequence) or hand is easily beaten by a single higher card.\n*** CRITICAL OUTPUT INSTRUCTIONS ***\n- You must reply ONLY with the format below.\n- NO N/A values. Use 1-10 for Strength in all stages.\n- Do not add any text before or after the metrics.\nExpected Output Format:\nAction: [Raise, Call, Check, Fold]\nCurrent Strength: [1-10]\nDanger Level: [1-10]\nRecommended Bet: [0-100]%";
        public const string None = "(none)";
        public const string PlayerHandPrefix = "Player hand: ";
        public const string BoardPrefix = "Board: ";
        public const string OfJoiner = " of ";
        public const string PluralS = "s";
        public const string StagePreFlop = "Pre-flop";
        public const string StageFlop = "Flop";
        public const string StageTurn = "Turn";
        public const string StageRiver = "River";
        public const string StageUnknown = "Unknown";
        public const string ValAce = "Ace";
        public const string ValTwo = "Two";
        public const string ValThree = "Three";
        public const string ValFour = "Four";
        public const string ValFive = "Five";
        public const string ValSix = "Six";
        public const string ValSeven = "Seven";
        public const string ValEight = "Eight";
        public const string ValNine = "Nine";
        public const string ValTen = "Ten";
        public const string ValJack = "Jack";
        public const string ValQueen = "Queen";
        public const string ValKing = "King";

        #endregion
    }
}
