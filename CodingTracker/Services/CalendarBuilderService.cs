using CodingTracker.Controllers;
using CodingTracker.Entities;
using CodingTracker.Repositories;

namespace CodingTracker.Services
{
    public static class CalendarServiceBuilder
    {
        public static List<CalendarDay> BuildCalendar(int year)
        {
            DateTime start = new DateTime(year, 1, 1);
            int days = GetDaysInYear(year);

            List<DateTime> dates = Enumerable.Range(0, days).Select(offset => start.AddDays(offset)).ToList();
            List<CalendarDay> dayList = new();
            var sessions = CodingController.GetSessionsByYear(start, "Ascending");

            int column = 0;

            //Advance the int value of monday to be the start of the week.
            int row = ((int)(start.DayOfWeek + 6) % 7) + 1;

            foreach (DateTime d in dates)
            {
                dayList.Add(
                    new CalendarDay
                    {
                        Column = column,
                        Row = row,
                        Date = d
                    });

                row++;
                if (row > 7)
                {
                    column++;
                    row = 1;
                }
            }

            foreach (var d in dayList)
            {
                foreach (var s in sessions)
                {
                    DateTime dayStart = d.Date.Date;
                    DateTime dayEnd = dayStart.AddDays(1);

                    DateTime overlapStart = s.StartTime > dayStart ? s.StartTime : dayStart;
                    DateTime overlapEnd = s.EndTime < dayEnd ? s.EndTime : dayEnd;

                    if (overlapStart < overlapEnd)
                    {
                        int minutes = (int)(overlapEnd - overlapStart).TotalMinutes;
                        d.CodingMinutes += minutes;

                    }
                }

                AssignSymbolAndColor(d);
            }
                
            return dayList;
        }

        public static int GetDaysInYear(int year)
        {
            return DateTime.IsLeapYear(year) ? 366 : 365;
        }

        public static int GetWeeksInYear(int year)
        {
            return GetDaysInYear(year) / 7;
        }

        public static void AssignSymbolAndColor(CalendarDay day)
        {
            

            switch (day.CodingMinutes)
            {
                case 0:
                    {
                        day.ActivitySymbol = "░ ";
                    }
                    break;
                case <= 30:
                    {
                        day.ActivitySymbol = "░ ";
                        day.ActivityColor = "darkgreen";
                    }
                    break;
                case <= 60:
                    {
                        day.ActivitySymbol = "▒ ";
                        day.ActivityColor = "darkgreen";
                    }
                    break;
                case <= 90:
                    {
                        day.ActivitySymbol = "▓ ";
                        day.ActivityColor = "darkgreen";
                    }
                    break;
                case <= 120:
                    {
                        day.ActivitySymbol = "█ ";
                        day.ActivityColor = "darkgreen";
                    }
                    break;
                case <= 150:
                    {
                        day.ActivitySymbol = "▒ ";
                        day.ActivityColor = "green";
                    }
                    break;
                case > 150:
                    {
                        day.ActivitySymbol = "█ ";
                        day.ActivityColor = "green";
                    }
                    break;
            }
        }
    }
}
