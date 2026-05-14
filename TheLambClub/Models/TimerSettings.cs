namespace TheLambClub.Models
{
    /// <summary>
    /// Represents the configuration settings for a timer, defining its total duration 
    /// and the frequency of its ticks.
    /// </summary>
    /// <param name="totalTimeInMillSeconds">The overall duration the timer should run for, in milliseconds.</param>
    /// <param name="intervalTimeInMillSeconds">The time elapsed between each timer tick, in milliseconds.</param>
    public class TimerSettings(long totalTimeInMillSeconds, long intervalTimeInMillSeconds)
    {
        #region properties

        /// <summary>
        /// Gets or sets the total duration of the timer in milliseconds.
        /// </summary>
        public long TotalTimeInMillSeconds { get; set; } = totalTimeInMillSeconds;

        /// <summary>
        /// Gets or sets the interval duration between consecutive ticks in milliseconds.
        /// </summary>
        public long IntervalTimeInMillSeconds { get; set; } = intervalTimeInMillSeconds;

        #endregion
    }
}