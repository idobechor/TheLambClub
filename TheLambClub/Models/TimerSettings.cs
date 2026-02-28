namespace TheLambClub.Models
{
    public class TimerSettings(long totalTimeInMillSeconds, long intervalTimeInMillSeconds)
    {
        #region properties

        public long TotalTimeInMillSeconds { get; set; } = totalTimeInMillSeconds;
        public long IntervalTimeInMillSeconds { get; set; } = intervalTimeInMillSeconds;

        #endregion
    }
}
