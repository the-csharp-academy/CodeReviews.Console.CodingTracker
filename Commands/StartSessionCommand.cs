using System.Diagnostics;

namespace Coding.Tracker.Commands
{
    public class StartSessionCommand : Command
    {
        public StartSessionCommand(CodeSessionService service) : base(service)
        {
        }

        public override void Execute()
        {
            SpectreConsoleUI.PrintMessage("To start session press any key...");
            Console.ReadKey();

            var startTime = DateTime.Now;
            SpectreConsoleUI.Clear();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var lastSecond = 0;
            while (!Console.KeyAvailable)
            {
                var currentSecond = (int)stopwatch.Elapsed.TotalSeconds;
                if (currentSecond != lastSecond)
                {
                    SpectreConsoleUI.Clear();
                    lastSecond = currentSecond;
                    var elapsed = stopwatch.Elapsed;
                    SpectreConsoleUI.PrintMessage($"Elapsed: {(int)elapsed.TotalHours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}. Press any key to stop");
                }
            }
            Console.ReadKey(true);
            SpectreConsoleUI.PrintMessage(string.Empty);
            stopwatch.Stop();

            var endTime = DateTime.Now;
            var codeSession = new CodeSession
            {
                StartTime = startTime.ToString("dd-MM-yy HH:mm:ss"),
                EndTime = endTime.ToString("dd-MM-yy HH:mm:ss"),
                Duration = TimeOnly.FromTimeSpan(endTime - startTime).ToString("HH:mm:ss")
            };

            if (!service.Create(codeSession))
            {
                throw new ArgumentException("Error occures when adding to database");
            }
            SpectreConsoleUI.PrintMessage("Session was succesfully created. Press any key to return to Menu", "green");
            Console.ReadKey();
        }
    }
}
