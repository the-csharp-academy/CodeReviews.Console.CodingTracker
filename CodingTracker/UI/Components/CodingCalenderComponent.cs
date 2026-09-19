using CodingTracker.Entities;
using CodingTracker.Services;
using Spectre.Console;

namespace CodingTracker.UI.Components
{
    public static class CodingCalendar
    {
        public static void Show()
        {
            int year = DateTime.Today.Year;

            var days = CalendarServiceBuilder.BuildCalendar(year);
            int Width = days.Max(d => d.Column) + 1;
            List<string> indicators = ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"];
            List<string> months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
            int month = 0;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    if (row == 0)
                    {

                        if(col == 0)
                        {
                            AnsiConsole.Markup($"[bold darkorange]     {months[month]}[/]");
                            month++;
                        }
                        else if(col > 3 && col % 4 == 0)
                        {
                            if (month <= months.Count - 1)
                            {
                                AnsiConsole.Markup($"[bold darkorange]{months[month]}[/]");
                                month++;
                            }
                        }
                        else
                        {
                            AnsiConsole.Markup("  ");
                        }
                    }

                    if (row > 0)
                    {
                        if (col == 0)
                        {
                            AnsiConsole.Markup($"[bold]{indicators[row - 1]}  [/]");
                        }
                        CalendarDay? day = days.FirstOrDefault(d => d.Row == row && d.Column == col);

                        if (day is null)
                        {
                            AnsiConsole.Markup("[gray]█ [/]");
                        }
                        else
                        {
                            AnsiConsole.Markup($"[{day.ActivityColor}]{day.ActivitySymbol}[/]");
                        }
                    }
                }
                AnsiConsole.Markup("\n\n");
            }
        }
    }
}
