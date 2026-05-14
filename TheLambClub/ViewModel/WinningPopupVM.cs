using System.Windows.Input;
using TheLambClub.ModelsLogic;

namespace TheLambClub.ViewModel
{
    /// <summary>
    /// ViewModel for the 'Winning' popup. Manages the display of player names 
    /// and provides the command to close the popup window.
    /// </summary>
    public class WinningPopupVM
    {
        #region fields

        private readonly WinningPopup? winningPopup;

        #endregion

        #region events

        /// <summary>
        /// Event triggered when a request to close the popup is made.
        /// </summary>
        public event Action? RequestClose;

        #endregion

        #region commands

        /// <summary>
        /// Command to close the winning popup.
        /// </summary>
        public ICommand? ClosePopupCommand { get; }

        #endregion

        #region properties

        /// <summary>
        /// Gets the list of names of the winning players.
        /// </summary>
        public string[] PlayersNames => winningPopup!.PlayersNames;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes the ViewModel with player data, hand ranks, and the number of winners.
        /// Sets up the command to close the popup.
        /// </summary>
        /// <param name="players">Array of players involved in the showdown.</param>
        /// <param name="ranks">Dictionary mapping players to their hand rank.</param>
        /// <param name="numUpWinners">Number of winners to be displayed.</param>
        public WinningPopupVM(Player[] players, Dictionary<Player, HandRank> ranks, int numUpWinners)
        {
            winningPopup = new WinningPopup(players, ranks, numUpWinners);
            ClosePopupCommand = new Command(ClosePopup);
        }

        /// <summary>
        /// Default constructor for the WinningPopupVM.
        /// </summary>
        public WinningPopupVM()
        {
        }

        #endregion

        #region private methods

        /// <summary>
        /// Invokes the RequestClose event to dismiss the popup.
        /// </summary>
        /// <param name="obj">The command parameter (unused).</param>
        private void ClosePopup(object obj)
        {
            RequestClose?.Invoke();
        }

        #endregion
    }
}