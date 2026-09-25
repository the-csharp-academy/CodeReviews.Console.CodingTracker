using Spectre.Console;

namespace Coding.Tracker.Commands
{
    public class CreateSessionCommand : Command
    {
        public CreateSessionCommand(CodeSessionService service) : base(service)
        {
        }

        public override void Execute()
        {
            var startTime = ValidateDateTime(AnsiConsole.Ask<string>("Enter starttime of session in dd-MM-yy HH:mm:ss format"));
            var endTime = ValidateDateTime(AnsiConsole.Ask<string>("Enter endTime of session in dd-MM-yy HH:mm:ss format"));

            if (endTime <= startTime)
            {
                throw new ArgumentException($"StartTime {startTime} cannot be greater then EndTime {endTime}");
            }

            var session = new CodeSession
            {
                StartTime = startTime.ToString("dd-MM-yy HH:mm:ss"),
                EndTime = endTime.ToString("dd-MM-yy HH:mm:ss"),
                Duration = TimeOnly.FromTimeSpan(endTime - startTime).ToString("HH:mm:ss")
            };

            if (!service.Create(session))
            {
                throw new ArgumentException("Error occures when adding to database");
            }
            SpectreConsoleUI.PrintMessage("Session was succesfully created. Press any key to return to Menu", "green");
            Console.ReadKey();
        }
    }
}
