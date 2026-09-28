using Spectre.Console;

namespace CodingTracker
{
    internal static class TableVisualizationEngine
    {
        internal static Table CreateTable(List<CodingSession> sessions)
        {
            Table table = new Table().AddColumns("Id", "Project", "Language", "Start time", "End time", "Duration");
            foreach (CodingSession session in sessions)
            {
                var duration = session.Duration;
                table.AddRow(session.Id.ToString(), session.Project, session.Language, session.StartTime.ToString(), session.EndTime?.ToString() ?? "In progress", $"{(int)duration.TotalHours}h {duration.Minutes}m {duration.Seconds}s");
            }
            return table;
        }
    }
}
