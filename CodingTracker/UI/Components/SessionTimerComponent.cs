using CodingTracker.Controllers;
using Spectre.Console;


namespace CodingTracker.UI.Components
{
    public static class SessionTimerComponent
    {
        public static void Show(string project, DateTime startTime)
        {
            var choice = AnsiConsole.Prompt(new SelectionPrompt<TimerChoices>()
                .Title("Select start to start the timer or press cancel to return to the menu:")
                .AddChoices(Enum.GetValues<TimerChoices>()));

            switch (choice)
            {
                case TimerChoices.Start:
                    Timer(project, startTime);
                    break;
                case TimerChoices.Cancel:
                    return;
            }
        }

        public static void Timer(string project, DateTime startTime)
        {
            bool running = true;

            AnsiConsole.MarkupLine("Press 'Q' to stop the timer.");
            while (running)
            {
                var endTime = AnsiConsole.Progress()
                     .Columns(
                     new TaskDescriptionColumn(),
                     new SpinnerColumn
                     {
                         Spinner = Spinner.Known.Aesthetic,
                         Style = new Style(Color.Green)
                     },
                     new ElapsedTimeColumn()
                     )
                     .Start(ctx =>
                     {
                         var task = ctx.AddTask($"Tracking: {project}.");

                         while (running)
                         {
                             if (Console.KeyAvailable)
                             {
                                 var key = Console.ReadKey(true);

                                 if (key.Key == ConsoleKey.Q)
                                 {
                                     running = false;
                                 }
                             }
                             Thread.Sleep(100);
                         }

                         return DateTime.Now;
                     });

                CodingController.HandleNewSession(project, startTime, endTime);
            }
        }
    }

    enum TimerChoices
    {
        Start,
        Cancel
    }
}
