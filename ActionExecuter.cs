using Coding.Tracker.Commands;
using Spectre.Console;

namespace Coding.Tracker
{

    public static class ActionExecuter
    {
        private readonly static string startSession = "Start new session";
        private readonly static string addSession = "Add new session";
        private readonly static string viewSessions = "View sessions";
        private readonly static string updateSession = "Update session";
        private readonly static string deleteSession = "Delete session";
        private readonly static string exit = "Exit";

        private readonly static Dictionary<string, UserAction> dictionary = new()
        {
            { startSession, UserAction.StartSession },
            { addSession, UserAction.CreateSession },
            { viewSessions, UserAction.ReadSessions },
            { updateSession, UserAction.UpdateSession },
            { deleteSession, UserAction.DeleteSession },
            { exit, UserAction.Exit },
        };

        public static UserAction GetUserAction()
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("What [blue]action[/] would you like?")
                .AddChoices(
                    startSession,
                    addSession,
                    viewSessions,
                    updateSession,
                    deleteSession,
                    exit)
                );
            return dictionary[choice];
        }

        public static void Execute(UserAction action, CodeSessionService service)
        {
            Command command = default;
            if (action == UserAction.CreateSession)
                command = new CreateSessionCommand(service);

            else if (action == UserAction.StartSession)
                command = new StartSessionCommand(service);

            else if (action == UserAction.ReadSessions)
                command = new ViewDataCommand(service);

            else if (action == UserAction.UpdateSession)
                command = new UpdateSessionCommand(service);

            else if (action == UserAction.DeleteSession)
                command = new DeleteSessionCommand(service);
            else if (action == UserAction.Exit)
                return;
            else throw new ArgumentException("Invalid command");
            command.Execute();
        }
    }
}
