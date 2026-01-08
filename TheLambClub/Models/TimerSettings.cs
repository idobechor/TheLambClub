
namespace TheLambClub.Models
{
    public class TimerSettings(long totalTimeInMillSeconds, long intervalTimeInMillSeconds )
    {
        public long TotalTimeInMillSeconds { get; set; } = totalTimeInMillSeconds;
        public long IntervalTimeInMillSeconds { get;set; }= intervalTimeInMillSeconds;
    }
}
