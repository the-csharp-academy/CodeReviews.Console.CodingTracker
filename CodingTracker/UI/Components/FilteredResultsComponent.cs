using CodingTracker.Entities;
using CodingTracker.Services;
using Spectre.Console;

namespace CodingTracker.UI.Components
{
    public static class FilteredResultsComponent
    {
        public static void RenderFilteredResults(IEnumerable<CodingSession> sessions)
        {
            AnsiConsole.Clear();
            TitleComponent.Show();

            if (sessions.Count() > 0)
            {
                var table = TableBuilderService.BuildSessionsTable(sessions);
                AnsiConsole.Write(table);
            }
            else
            {
                Console.MarkupLine($"[bold darkorange]No Sessions found for the selecte date.[/]");
            }

            AnsiConsole.MarkupLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
