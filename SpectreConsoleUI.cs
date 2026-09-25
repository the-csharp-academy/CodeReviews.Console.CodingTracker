using Spectre.Console;

namespace Coding.Tracker
{
    public static class SpectreConsoleUI
    {
        public static void PrintTable(List<CodeSession> sessions)
        {
            var table = new Table()
                .AddColumn("Id")
                .AddColumn("Start time")
                .AddColumn("End time")
                .AddColumn("Duration");

            foreach(var session in sessions)
            {
                table.AddRow(
                    $"{session.Id}",
                    $"{session.StartTime}",
                    $"{session.EndTime}", 
                    $"{session.Duration}");
            }

            AnsiConsole.Write(table);
        }

        public static void PrintMessage(string message, string color)
        {
            AnsiConsole.MarkupLine($"[{color}] {message}[/]");
        }

        public static void PrintMessage(string message)
        {
            PrintMessage(message, "white");
        }

        public static void Clear()
        {
            AnsiConsole.Clear();
        }

        public static List<int> PrintChoices(List<CodeSession> sessions)
        {
            var multiPrompt = new MultiSelectionPrompt<string>()
                .Title("Choose session")
                .NotRequired()
                .InstructionsText("[grey](Press [blue]<space>[/] to toggle, [green]<enter>[/] to confirm)[/]");

            foreach (var session in sessions)
            {
                multiPrompt.AddChoice(session.ToString());
            }

            var choices = AnsiConsole.Prompt(multiPrompt);
            return sessions
                .Where(s => choices.Contains(s.ToString()))
                .Select(s => s.Id)
                .ToList();
        }
    }
}
