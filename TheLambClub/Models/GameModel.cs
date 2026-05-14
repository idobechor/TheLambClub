using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System.Collections.ObjectModel;
using TheLambClub.ModelsLogic;

namespace TheLambClub.Models
{
    /// <summary>
    /// Represents the abstract base model for a game, defining the core structure, 
    /// properties, and methods for managing poker game logic and Firebase synchronization.
    /// </summary>
    public abstract class GameModel
    {
        #region fields

        /// <summary>
        /// Registration for the Firestore snapshot listener.
        /// </summary>
        [Ignored]
        protected IListenerRegistration? ilr;

        /// <summary>
        /// Instance of the Firebase data handler.
        /// </summary>
        [Ignored]
        protected FbData fbd = new();

        /// <summary>
        /// The index of the player whose turn it currently is.
        /// </summary>
        [Ignored]
        protected int _currentPlayerIndex;

        /// <summary>
        /// Indicates whether the game timer has been initialized.
        /// </summary>
        [Ignored]
        protected bool TimerCreated = false;

        /// <summary>
        /// Constant representing the state when a hand is complete.
        /// </summary>
        [Ignored]
        protected const int HandComplete = 4;

        /// <summary>
        /// Configuration settings for the game timer.
        /// </summary>
        [Ignored]
        public TimerSettings timerSettings = new(Keys.TimerTotalTime, Keys.TimerInterval);

        #endregion

        #region events

        /// <summary>
        /// Occurs when a winner is determined and the popup should be displayed.
        /// </summary>
        [Ignored]
        public EventHandler<WinningPopupEvent>? OnwinnerSelected;

        /// <summary>
        /// Occurs when the remaining time value changes.
        /// </summary>
        [Ignored]
        public EventHandler? TimeLeftChanged;

        /// <summary>
        /// Occurs when the turn timer reaches zero.
        /// </summary>
        [Ignored]
        public EventHandler? TimeLeftFinished;

        /// <summary>
        /// Occurs when the turn moves to a different player.
        /// </summary>
        [Ignored]
        public EventHandler? OnTurnChanged;

        /// <summary>
        /// Occurs when the game document is deleted from the database.
        /// </summary>
        [Ignored]
        public EventHandler? OnGameDeleted;

        /// <summary>
        /// Occurs when a player loses and exits the game.
        /// </summary>
        [Ignored]
        public EventHandler? OnPlayerLost;

        /// <summary>
        /// Occurs when a winner is selected for the current round/hand.
        /// </summary>
        [Ignored]
        public EventHandler? OnWinnerSelected;

        /// <summary>
        /// Occurs when any general game state change is detected.
        /// </summary>
        [Ignored]
        public EventHandler? OnGameChanged;

        /// <summary>
        /// Occurs when the UI status for 'Check' or 'Call' needs updating.
        /// </summary>
        [Ignored]
        public EventHandler? OnCheckOrCallChanged;

        /// <summary>
        /// Occurs when the local player's balance is updated.
        /// </summary>
        [Ignored]
        public EventHandler<string>? OnMyMoneyChanged;

        #endregion

        #region properties
        /// <summary>
        /// Gets the player object whose turn it currently is.
        /// </summary>
        [Ignored]
        public abstract Player? CurrentPlayer { get; }

        /// <summary>
        /// Gets a string representation of the current game status.
        /// </summary>
        [Ignored]
        public abstract string CurrentStatus { get; }

        /// <summary>
        /// Gets the set of cards (deck) used in the game.
        /// </summary>
        [Ignored]
        protected SetOfCards SetOfCards { get; } = new SetOfCards();

        /// <summary>
        /// Gets or sets the string format of the time remaining for the current turn.
        /// </summary>
        [Ignored]
        public string TimeLeft { get; protected set; } = string.Empty;

        /// <summary>
        /// Gets the maximum possible bet allowed based on the remaining money of active players.
        /// </summary>
        [Ignored]
        public int MaxBet => (int)Players!.Min(p => p.IsFolded == false ? p.CurrentMoney : 10000);

        /// <summary>
        /// Gets or sets the array of cards currently displayed on the board.
        /// </summary>
        public FBCard[] BoardCards { get; set; } = new FBCard[5];

        /// <summary>
        /// Gets or sets the current round number of the hand.
        /// </summary>
        public int RoundNumber { get; set; }
        /// <summary>
        /// Gets or sets the current total amount of money in the pot.
        /// </summary>
        public int Pot { get; set; } = 0;
        /// <summary>
        /// Gets or sets the index of the current active player.
        /// </summary>
        public abstract int CurrentPlayerIndex { get; set; }

        /// <summary>
        /// Gets or sets the creation timestamp of the game session.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of players allowed in the game.
        /// </summary>
        public int MaxNumOfPlayers { get; set; }

        /// <summary>
        /// Gets or sets the current number of players joined in the game.
        /// </summary>
        public int CurrentNumOfPlayers { get; set; } = 1;

        /// <summary>
        /// Gets or sets the minimum required bet for the current turn.
        /// </summary>
        [Ignored]
        public int MinBet { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the game lobby is full.
        /// </summary>
        public abstract bool IsFull { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the game document.
        /// </summary>
        [Ignored]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets the name of the local user.
        /// </summary>
        [Ignored]
        public string MyName { get; } = new User().UserName;

        /// <summary>
        /// Gets or sets the unique identifier of the game host.
        /// </summary>
        public string? HostId { get; set; } = string.Empty;

        /// <summary>
        /// Gets a value indicating whether the local user is the host.
        /// </summary>
        [Ignored]
        public abstract bool IsHost { get; }

        /// <summary>
        /// Gets a value indicating whether it is currently the local user's turn.
        /// </summary>
        [Ignored]
        public abstract bool IsMyTurn { get; }

        /// <summary>
        /// Gets or sets the text display for the 'Check' or 'Call' button.
        /// </summary>
        [Ignored]
        public string CheckOrCall { get; set; } = Strings.Check;

        /// <summary>
        /// Gets or sets the array of players participating in the game.
        /// </summary>
        public Player[]? Players { get; set; }

        /// <summary>
        /// Gets the collection of card views for the board.
        /// </summary>
        [Ignored]
        public abstract ObservableCollection<ViewCard>? BoardViewCards { get; }

        /// <summary>
        /// Gets the first private card view for the player.
        /// </summary>
        [Ignored]
        public abstract ViewCard? ViewCard1 { get; }

        /// <summary>
        /// Gets the second private card view for the player.
        /// </summary>
        [Ignored]
        public abstract ViewCard? ViewCard2 { get; }

        /// <summary>
        /// Gets the string representation of the maximum player count.
        /// </summary>
        [Ignored]
        public string NumOfPlayersName => $"{MaxNumOfPlayers}";

        #endregion

        #region public methods

        /// <summary>
        /// Creates or updates the game document in the database.
        /// </summary>
        /// <param name="OnComplete">Callback action executed upon completion.</param>
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);

        /// <summary>
        /// Attaches a real-time listener to the game document.
        /// </summary>
        public abstract void AddSnapShotListener();

        /// <summary>
        /// Detaches the real-time listener from the game document.
        /// </summary>
        public abstract void RemoveSnapShotListener();

        /// <summary>
        /// Deletes the game document from the database.
        /// </summary>
        /// <param name="OnComplete">Callback action executed upon completion.</param>
        public abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete);

        /// <summary>
        /// Handles the logic when a player chooses to fold.
        /// </summary>
        public abstract void PickedFold();

        /// <summary>
        /// Updates guest user data within the game state.
        /// </summary>
        /// <param name="OnComplete">Callback action executed upon completion.</param>
        public abstract void UpdateGuestUser(Action<Task> OnComplete);

        /// <summary>
        /// Executes the 'Call' or 'Check' action logic.
        /// </summary>
        public abstract void CallFunction();

        /// <summary>
        /// Executes the 'Bet' action logic.
        /// </summary>
        /// <param name="obj">The bet amount or relevant data.</param>
        public abstract void BetFunction(object obj);

        /// <summary>
        /// Calculates and returns the index of the previous player.
        /// </summary>
        /// <returns>The index of the previous player.</returns>
        public abstract int PreviousPlayerIndex();

        #endregion

        #region protected methods

        /// <summary>
        /// Generic internal callback for task completion.
        /// </summary>
        /// <param name="task">The completed task.</param>
        protected abstract void OnComplete(Task task);

        /// <summary>
        /// Updates the database when a new player joins the game.
        /// </summary>
        /// <param name="OnComplete">Callback action executed upon completion.</param>
        protected abstract void UpdateFireBaseJoinGame(Action<Task> OnComplete);

        /// <summary>
        /// Populates the board cards based on the current round.
        /// </summary>
        /// <param name="round">The current round number.</param>
        protected abstract void FillBoard(int round);

        /// <summary>
        /// Handles data changes received from the Firestore snapshot.
        /// </summary>
        /// <param name="snapshot">The updated document snapshot.</param>
        /// <param name="error">Any potential error encountered.</param>
        protected abstract void OnChange(IDocumentSnapshot? snapshot, Exception? error);

        /// <summary>
        /// Initializes the card array and optionally updates the remote state.
        /// </summary>
        /// <param name="upDateFB">True to push updates to Firebase.</param>
        /// <param name="OnComplete">Callback action executed upon completion.</param>
        protected abstract void FillArrayAndAddCards(bool upDateFB, Action<Task> OnComplete);

        /// <summary>
        /// Synchronizes the players array with the remote database.
        /// </summary>
        /// <param name="OnComplete">Callback action executed upon completion.</param>
        protected abstract void UpdatePlayersArray(Action<Task> OnComplete);

        /// <summary>
        /// Checks if only one player remains active in the hand.
        /// </summary>
        /// <returns>True if only one player is left.</returns>
        protected abstract bool IsOneStaying();

        /// <summary>
        /// Resets the 'IsFolded' status for all players.
        /// </summary>
        protected abstract void ChangeIsFoldedToFalse();

        /// <summary>
        /// Executes final logic for the current hand.
        /// </summary>
        protected abstract void EndHand();

        /// <summary>
        /// Processes timer-related messages and updates the countdown UI.
        /// </summary>
        /// <param name="timeLeft">The remaining time in milliseconds.</param>
        protected abstract void OnMessageReceived(long timeLeft);

        /// <summary>
        /// Initializes and starts the game timer.
        /// </summary>
        protected abstract void RegisterTimer();

        /// <summary>
        /// Conditionally updates Firebase based on round or turn transitions.
        /// </summary>
        /// <param name="endedRound">Whether the current betting round ended.</param>
        /// <param name="skippedTurn">Whether a turn was skipped.</param>
        /// <param name="round">The current round number.</param>
        /// <param name="isEndOfHand">Whether the entire hand has ended.</param>
        protected abstract void UpdateFirebaseIfNeeded(bool endedRound, bool skippedTurn, int round, bool isEndOfHand);

        /// <summary>
        /// Updates the 'Check'/'Call' text on the user interface.
        /// </summary>
        protected abstract void UpdateCheckOrCallUI();

        /// <summary>
        /// Synchronizes the local game state with the provided game object.
        /// </summary>
        /// <param name="updatedGame">The updated game data.</param>
        protected abstract void SyncGameState(Game updatedGame);

        /// <summary>
        /// Determines if the game has just transitioned from a lobby to a started state.
        /// </summary>
        /// <param name="updatedGame">The updated game data.</param>
        /// <returns>True if the game just started.</returns>
        protected abstract bool HasGameJustStarted(Game updatedGame);

        /// <summary>
        /// Checks if the current betting round should conclude.
        /// </summary>
        /// <param name="isEndOfRound">Current round end status.</param>
        /// <param name="isHandEnded">Current hand end status.</param>
        /// <returns>True if the round should end.</returns>
        protected abstract bool ShouldEndRound(bool isEndOfRound, bool isHandEnded);

        /// <summary>
        /// Manages the scenario where only one player remains after others fold.
        /// </summary>
        /// <returns>The updated array of players.</returns>
        protected abstract Player[] HandleLastPlayerWins();

        /// <summary>
        /// Checks if all active players have contributed equal bets to the pot.
        /// </summary>
        /// <returns>True if all active bets are equal.</returns>
        protected abstract bool EveryOneAreEqual();

        /// <summary>
        /// Executes logic specific to the end of a betting round.
        /// </summary>
        /// <param name="round">The round that is ending.</param>
        protected abstract void EndOfRound(int round);

        /// <summary>
        /// Calculates the minimum bet required for the next action.
        /// </summary>
        /// <returns>The minimum bet amount.</returns>
        protected abstract int CalculateMinBet();

        /// <summary>
        /// Checks if all active players have a current bet of zero.
        /// </summary>
        /// <returns>True if all bets are zero.</returns>
        protected abstract bool AllBetsZero();

        /// <summary>
        /// Orchestrates updates for round and turn transitions.
        /// </summary>
        /// <param name="isEndOfRound">Whether the round ended.</param>
        /// <param name="isHandEnded">Whether the hand ended.</param>
        /// <param name="changedToFull">Whether the game became full.</param>
        /// <param name="nextRound">The value of the next round.</param>
        protected abstract void ProcessRoundAndTurnUpdates(bool isEndOfRound, bool isHandEnded, bool changedToFull, int nextRound);

        /// <summary>
        /// Splits and distributes the pot among the winners based on hand strength.
        /// </summary>
        /// <param name="sortedPlayers">Array of players sorted by hand rank.</param>
        /// <param name="Dict">Dictionary mapping players to their hand ranks.</param>
        protected abstract void DistributePotToWinners(Player[] sortedPlayers, Dictionary<Player, HandRank> Dict);

        /// <summary>
        /// Determines if the game has reached its maximum player capacity.
        /// </summary>
        /// <param name="updatedGame">The updated game data.</param>
        /// <returns>True if the game is now full.</returns>
        protected abstract bool HasGameBecomeFull(Game updatedGame);

        /// <summary>
        /// Evaluates the hand strength for all active players.
        /// </summary>
        /// <returns>A dictionary of players and their evaluated hand ranks.</returns>
        protected abstract Dictionary<Player, HandRank> EvaluatePlayerHands();

        /// <summary>
        /// Checks if any player in the game has gone All-In.
        /// </summary>
        /// <returns>True if at least one player is All-In.</returns>
        protected abstract bool AnyOneIsAllIn();

        /// <summary>
        /// Sorts players based on the strength of their poker hands.
        /// </summary>
        /// <param name="ranks">Dictionary containing player hand ranks.</param>
        /// <returns>A sorted array of players.</returns>
        protected abstract Player[] SortPlayersByHandRank(Dictionary<Player, HandRank> ranks);

        /// <summary>
        /// Determines if the local player is a winner in the current hand.
        /// </summary>
        /// <returns>True if the local player won.</returns>
        protected abstract bool AmIWinner();

        /// <summary>
        /// Checks if the current round should end based on the latest game update.
        /// </summary>
        /// <param name="updatedGame">The updated game data.</param>
        /// <returns>True if the round is ending.</returns>
        protected abstract bool IsRoundEnding(Game updatedGame);

        /// <summary>
        /// Checks if the current hand has concluded.
        /// </summary>
        /// <returns>True if the hand is over.</returns>
        protected abstract bool IsHandOver();

        /// <summary>
        /// Determines if the overall game session is over (e.g., all players but one bankrupt).
        /// </summary>
        /// <returns>True if the game is over.</returns>
        protected abstract bool CheckForGameOver();

        /// <summary>
        /// Handles the showdown process where cards are revealed and hands compared.
        /// </summary>
        /// <returns>The array of winning players.</returns>
        protected abstract Player[] HandleShowdown();

        /// <summary>
        /// Ensures the turn timer is properly registered and active.
        /// </summary>
        protected abstract void EnsureTimerRegistered();

        /// <summary>
        /// Performs cleanup and state updates at the conclusion of a hand.
        /// </summary>
        /// <returns>The final player states after the hand.</returns>
        protected abstract Player[] HandleHandEnd();

        /// <summary>
        /// Executes hand finalization tasks if the local user is the host.
        /// </summary>
        /// <returns>True if successful.</returns>
        protected abstract bool FinalizeHandIfHost();

        /// <summary>
        /// Manages specific game flow adjustments when All-In scenarios occur.
        /// </summary>
        /// <returns>An integer code or value related to the All-In status.</returns>
        protected abstract int HandleAllInScenarios();

        /// <summary>
        /// Updates UI labels to display the names of opponents.
        /// </summary>
        /// <param name="lstOponnentsLabels">List of labels for opponent names.</param>
        public abstract void DisplayOponnentsNames(List<Label> lstOponnentsLabels);

        /// <summary>
        /// Updates the financial status and winner display on the UI.
        /// </summary>
        /// <param name="lstOponnentsLabels">List of labels for names.</param>
        /// <param name="lstOponnentsMoneyLabels">List of labels for money values.</param>
        /// <param name="winnerName">The name of the winner to highlight.</param>
        public abstract void UpdateMoney(List<Label> lstOponnentsLabels, List<Label> lstOponnentsMoneyLabels, string winnerName);

        /// <summary>
        /// Determines if the current player's turn should be automatically skipped (e.g., if they folded).
        /// </summary>
        /// <returns>True if the turn should be skipped.</returns>
        protected bool ShouldSkipCurrentPlayerTurn() => CurrentPlayer != null && IsMyTurn && (CurrentPlayer.IsFolded);
        #endregion
    }
}