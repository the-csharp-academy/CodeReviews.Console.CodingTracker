using Spectre.Console;

namespace Coding.Tracker.Commands
{
    public class DeleteSessionCommand : Command
    {
        public DeleteSessionCommand(CodeSessionService service) : base(service)
        {
        }

        public override void Execute()
        {
            var data = service.ReadAllData();
            if (data.Count == 0)
            {
                SpectreConsoleUI.PrintMessage("No sessions to delete", "yellow");
                Console.ReadKey(true);
                return;
            }
            var selected = SpectreConsoleUI.PrintChoices(data);
            if (selected.Count == 0)
            {
                SpectreConsoleUI.PrintMessage("Deletion cancelled", "yellow");
                Console.ReadKey(true);
                return;
            }
            var deletedCount = 0;

            foreach (var id in selected)
            {
                if (service.Delete(id))
                {
                    deletedCount++;
                }
                else
                {
                    SpectreConsoleUI.PrintMessage($"Session with id {id} was not deleted", "yellow");
                }
            }
            SpectreConsoleUI.PrintMessage("Session was succesfully deleted. Press any key to return to Menu", "green");
            Console.ReadKey(true);
        }
    }
}
