namespace CodingTracker;

class CodingSession
{
    public int Id {get; set;}
    public DateTime StartTime {get; set;}
    public DateTime EndTime {get; set;}
    public int Duration {get; set;}

    public string FormattedDuration
    {
        get
        {
            TimeSpan time = TimeSpan.FromMinutes(Duration);
            return $"{(int)time.TotalHours:D2}:{time.Minutes:D2}";
        }
    }
}