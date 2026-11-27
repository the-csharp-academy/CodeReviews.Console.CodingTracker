using System;

public class CodingSession
{
    public int SessionID { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public DateOnly Date { get; set; }

    public TimeSpan Duration => EndTime - StartTime;
}
