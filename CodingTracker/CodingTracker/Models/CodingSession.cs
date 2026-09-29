using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.Models
{
    public class CodingSession
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration
        {
            get
            {
                if(EndTime < StartTime) return TimeSpan.Zero;
                return EndTime - StartTime;
            }
        }
    }
}
