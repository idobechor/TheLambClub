using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    public class Connectivity : ConnectivityModel
    {
        public Connectivity()
        {
            Microsoft.Maui.Networking.Connectivity.Current.ConnectivityChanged += OnConnectivityChanged;
            IsConnected = Microsoft.Maui.Networking.Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        }

        private void OnConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            IsConnected = e.NetworkAccess == NetworkAccess.Internet;
        }
    }
}
