using static CodingTracker.Models.Enums.MenuAction;
using Spectre.Console;
using CodingTracker.Controllers;

namespace CodingTracker.UI
{
    internal class UserInterface 
    {
        private readonly ICodingSessionsController _controller;

        public UserInterface(ICodingSessionsController codingSessionsController)
        {
            _controller = codingSessionsController;
        }

        internal void MainMenu()
        {
            AnsiConsole.Clear();
            bool closeApp = false;
            while (!closeApp)
            {
                AnsiConsole.MarkupLine("Coding tracker\n");

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<MainMenuAction>()
                    .Title("Select an action:")
                    .AddChoices(Enum.GetValues<MainMenuAction>()));

                switch (choice)
                {
                    case MainMenuAction.Exit:
                        AnsiConsole.Clear();
                        AnsiConsole.MarkupLine("Thank you!");
                        closeApp = true;
                        break;
                    case MainMenuAction.ViewSessions:
                        AnsiConsole.Clear();
                        _controller.ViewSessions();
                        break;
                    case MainMenuAction.AddSession:
                        AnsiConsole.Clear();
                        _controller.AddSession();
                        break;
                    case MainMenuAction.DeleteSession:
                        AnsiConsole.Clear();
                        _controller.DeleteSession();
                        break;
                    case MainMenuAction.UpdateSession:
                        AnsiConsole.Clear();
                        _controller.UpdateSession();
                        break;
                }
            }
        }
    }
}
