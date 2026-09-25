using Spectre.Console;

namespace Coding.Tracker.Commands
{
    public class ViewDataCommand : Command
    {
        public ViewDataCommand(CodeSessionService service) : base(service)
        {
        }

        public override void Execute()
        {
            var list = service.ReadAllData();
            if (list.Count == 0)
            {
                SpectreConsoleUI.PrintMessage("No sessions to delete", "yellow");
                Console.ReadKey(true);
                return;
            }
            SpectreConsoleUI.PrintTable(list);


            while (true)
            {
                var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("What [blue]action[/] would you like?")
                .AddChoices(
                    "Exit",
                    "Order duration descending",
                    "Order duration ascending"));

                if (choice == "Exit")
                    break;
                else if (choice == "Order duration descending")
                    OrderByDesc();
                else if (choice == "Order duration ascending")
                    OrderByAsc();
            }
        }

        public void ExecuteWithoutExit()
        {
            var list = service.ReadAllData();
            SpectreConsoleUI.PrintTable(list);
        }

        public void OrderByDesc()
        {
            SpectreConsoleUI.Clear();
            var ordered = service.ReadAllData().OrderByDescending(c => c.Duration).ToList();
            SpectreConsoleUI.PrintTable(ordered);
        }

        public void OrderByAsc()
        {
            SpectreConsoleUI.Clear();
            var ordered = service.ReadAllData().OrderBy(c => c.Duration).ToList();
            SpectreConsoleUI.PrintTable(ordered);
        }
    }
}
