

namespace CodingTracker.Entities
{
    public class CodingSession
    {
        public int Id { get; set; }
        public string Project { get; set; } = "";
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Duration { get; set; }
    }


}
