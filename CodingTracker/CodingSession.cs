namespace CodingTracker
{
    public class CodingSession
    {
        public int Id { get; set; }
        public string Project { get; set; } = "";
        public string Language { get; set; } = "";
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan Duration => EndTime == null ? DateTime.Now - StartTime : EndTime.Value - StartTime;

        public CodingSession()
        {

        }

        public CodingSession(string project, string language, DateTime startTime, DateTime? endTime)
        {
            Project = project;
            Language = language;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
