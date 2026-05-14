using System;
using System.Collections.Generic;
using System.Linq;
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    /// <summary>
    /// Manages the data and logic for the winning popup display, including formatting 
    /// the winner's names and their corresponding hand rankings.
    /// </summary>
    public class WinningPopup(Player[] players, Dictionary<Player, HandRank> ranks, int numUpWinners) : WinningPopupModel(players, ranks)
    {
        #region properties

        /// <summary>
        /// Generates an array of strings representing the players' names and their performance.
        /// Distinguishes between winners and other participants based on the provided hand ranks.
        /// </summary>
        public override string[] PlayersNames => [.. Players.Select(player =>
                 {
                     string WinnerText = string.Empty;
                     
                     // If no rankings are available, simply introduce the player as a winner
                     if (Ranks == null)
                         WinnerText = Models.Strings.IntoruceTheWinner + player.Name;
                     else
                     {
                         if (player != null)
                         {
                             // Check if the current player is within the top 'numUpWinners'
                             if (Array.IndexOf(Players, player) < numUpWinners)
                                 WinnerText = Models.Strings.IntoruceTheWinner + player.Name + " " + Ranks[player].ToString();
                             else
                                 // Otherwise, display their numerical standing and hand rank
                                 WinnerText = (Array.IndexOf(Players, player) + 1) + " " + player.Name + " " + Ranks[player].ToString();
                         }
                     }
                     return WinnerText;
                 })];

        #endregion
    }
}