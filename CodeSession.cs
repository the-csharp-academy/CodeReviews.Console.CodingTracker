namespace Coding.Tracker
{
    public class CodeSession
    {
        public int Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Duration { get; set; }

        public override string ToString()
        {
            return Id.ToString()+ ":" + StartTime + " " + EndTime + " " + Duration;
        }
    }
}
