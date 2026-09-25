using Spectre.Console;

namespace Coding.Tracker.Commands
{
    public class UpdateSessionCommand : Command
    {
        public UpdateSessionCommand(CodeSessionService service) : base(service)
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
            var reader = new ViewDataCommand(service);
            reader.ExecuteWithoutExit();
            var id = ValidateNumeric(AnsiConsole.Ask<string>("Enter numeric [blue]id[/] of session for update:"));

            var session = service.ReadAllData().FirstOrDefault(s => s.Id == id);
            if (session == null)
            {
                SpectreConsoleUI.PrintMessage($"Session with {id} not found", "yellow");
                Console.ReadKey();
            }
            else
            {
                var startTime = ValidateDateTime(AnsiConsole.Ask<string>("Enter start time of session in dd-MM-yy HH:mm:ss format:"));
                var endTime = ValidateDateTime(AnsiConsole.Ask<string>("Enter end time of session in dd-MM-yy HH:mm:ss format:"));

                if (endTime <= startTime)
                {
                    throw new ArgumentException($"StartTime {startTime} cannot be greater then EndTime {endTime}");
                }

                session.StartTime = startTime.ToString("dd-MM-yy HH:mm:ss");
                session.EndTime = endTime.ToString("dd-MM-yy HH:mm:ss");
                session.Duration = TimeOnly.FromTimeSpan(endTime - startTime).ToString("HH:mm:ss");

                if (!service.Update(session))
                {
                    throw new ArgumentException("Error occures when updating of database");
                }

                SpectreConsoleUI.PrintMessage("Session was succesfully updated. Press any key to return to Menu", "green");
                Console.ReadKey();
            }
        }
    }
}
