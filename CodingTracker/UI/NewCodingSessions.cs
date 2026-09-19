using CodingTracker.Controllers;
using CodingTracker.UI.Components;
using Spectre.Console;

namespace CodingTracker.UI
{
    public static class NewCodingSessions
    {
        public static void Show()
        {
            while (true)
            {
                var choice = AnsiConsole.Prompt(new SelectionPrompt<NewSessionOptions>()
                    .Title("Select Start to start tracking your new Coding Session or Manually create a coding session.")
                    .UseConverter(x => x switch
                    {
                        NewSessionOptions.Start => "Start a timed session.",
                        NewSessionOptions.Manual => "Manually create a Coding Session",
                        NewSessionOptions.Cancel => "Back to main menu."
                    })
                    .AddChoices(Enum.GetValues<NewSessionOptions>()));

                switch (choice)
                {
                    case NewSessionOptions.Start:
                        TimerEntry();
                        break;
                    case NewSessionOptions.Manual:
                        ManualEntry();
                        break;
                    case NewSessionOptions.Cancel:
                        return;
                }
                return;
            }
        }

        public static void TimerEntry()
        {
            var project = AnsiConsole.Prompt(new TextPrompt<string>("What project are you working on?"));

            SessionTimerComponent.Show(project, DateTime.Now);
        }

        public static void ManualEntry()
        {
            var project = AnsiConsole.Prompt(new TextPrompt<string>
                ("What are you working on?:")
                .AllowEmpty());

            var startTime = AnsiConsole.Prompt(new TextPrompt<DateTime>("Enter a Start time." +
                "\nExpected format is: [bold yellow]'MM/DD/YYYY hh:mm:ss:'[/]")
                .ValidationErrorMessage("Invalid time value or foramt provided\nThe expected format is:" +
                "[bold yellow]'MM/DD/YYYY hh:mm:ss:'[/]"));

            var endTime = AnsiConsole.Prompt(new TextPrompt<DateTime>("Enter a End time." +
                "\nExpected format is: [bold yellow]'MM/DD/YYYY hh:mm:ss:'[/]")
                .Validate(x =>
                {
                    if (x < startTime)
                    {
                        return ValidationResult.Error("[red]End time can not be before start time![/]");
                    }
                    return ValidationResult.Success();
                })
                .ValidationErrorMessage("Invalid time value or foramt provided\nThe expected format is:" +
                "[bold yellow]'MM/DD/YYYY hh:mm:ss:'[/]"));

            CodingController.HandleNewSession(project, startTime, endTime);
        }
    }

    enum NewSessionOptions
    {
        Start,
        Manual,
        Cancel
    }
}
