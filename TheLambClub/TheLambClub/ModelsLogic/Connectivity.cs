using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLambClub.Models;
using static TheLambClub.Models.FBCard;

namespace TheLambClub.ModelsLogic
{
    public class Connectivity : ConnectivityModel
    {
        #region constructors
        public Connectivity()
        {
            Microsoft.Maui.Networking.Connectivity.Current.ConnectivityChanged += OnConnectivityChanged;
            IsConnected = Microsoft.Maui.Networking.Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        }
        #endregion

        #region public methods
        protected override void OnConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            IsConnected = e.NetworkAccess == NetworkAccess.Internet;
        }
        #endregion
    }
}
