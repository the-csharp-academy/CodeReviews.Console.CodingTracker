using CodingTracker.UI.Components;
using Spectre.Console;


namespace CodingTracker.UI
{
    public static class MainMenu
    {
        public static void Show()
        {
            while (true)
            {
                AnsiConsole.Clear();
                TitleComponent.Show();
                CodingCalendar.Show();

                var choice = AnsiConsole.Prompt(new SelectionPrompt<MainMenuOptions>()
                    .Title("What would you like to do?")
                    .UseConverter(m => m switch
                    {
                        MainMenuOptions.StartCodingSession => "Start a new Coding Session",
                        MainMenuOptions.ViewCodingSessions => "View an Manage Coding Sessions",
                        MainMenuOptions.Exit => "Exit"
                    })
                    .AddChoices(Enum.GetValues<MainMenuOptions>()));

                switch (choice)
                {
                    case MainMenuOptions.StartCodingSession:
                        NewCodingSessions.Show();
                        break;
                    case MainMenuOptions.ViewCodingSessions:
                        CodingSessionsUI.Show();
                        break;
                    case MainMenuOptions.Exit:
                        return;
                }
            }
        }
    }

    enum MainMenuOptions
    {
        StartCodingSession,
        ViewCodingSessions,
        Exit
    }
}
