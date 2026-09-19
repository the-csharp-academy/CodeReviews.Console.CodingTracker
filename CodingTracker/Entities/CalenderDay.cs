using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.Entities
{
    public class CalendarDay
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public DateTime Date { get; set; }
        public int CodingMinutes { get; set; }
        public string ActivitySymbol { get; set; } = "░ ";
        public string ActivityColor { get; set; } = "gray";
    }
}
