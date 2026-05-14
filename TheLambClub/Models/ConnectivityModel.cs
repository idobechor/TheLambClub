namespace TheLambClub.Models
{
    public abstract class ConnectivityModel
    {
        #region fields
        private bool _isConnected;
        #endregion
        #region properties  
        public bool IsConnected
        {
            get => _isConnected;
            protected set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    ConnectivityChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        #endregion
        #region events
        public EventHandler? ConnectivityChanged { get; set; }
        #endregion
        #region protected methods
        protected abstract void OnConnectivityChanged(object? sender, ConnectivityChangedEventArgs e);
        #endregion
    }
}
