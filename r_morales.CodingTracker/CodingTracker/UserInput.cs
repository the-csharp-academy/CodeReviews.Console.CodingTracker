using CodingTracker.Validation;
using Spectre.Console;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodingTracker.Input
{
    public class UserInput
    {
        public MenuChoices GetMenuSelection()
        {
            return AnsiConsole.Prompt(new SelectionPrompt<MenuChoices>()
                .Title("Welcome to the Coding Tracker!\nWhat would you like to do?")
                .AddChoices(Enum.GetValues<MenuChoices>())
                .UseConverter(choice => choice switch
                {
                    MenuChoices.StartNewSession => "Start a new session",
                    MenuChoices.AddNewSession => "Add a new session",
                    MenuChoices.ViewSessions => "View coding sessions",
                    MenuChoices.UpdateSession => "Update a session",
                    MenuChoices.DeleteSession => "Delete a session",
                    MenuChoices.Exit => "Exit",
                    _ => choice.ToString()
                })
            );
        }

        public DateOnly GetDateInput(string prompt)
        {
            while (true)
            {
                string input = AnsiConsole.Ask<string>(prompt);
                if (InputValidation.ValidateDateInput(input, out DateOnly date))
                {
                    return date;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Invalid date format. Please try again.[/]");
                }
            }
        }

        public TimeOnly GetTimeInput(string prompt)
        {
            while (true)
            {
                string input = AnsiConsole.Ask<string>(prompt);
                if (InputValidation.ValidateTimeInput(input, out TimeOnly time))
                {
                    return time;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Invalid time format. Please try again.[/]");
                }
            }
        }

        public int GetIntegerInput(string prompt)
        {
            while (true)
            {
                string input = AnsiConsole.Ask<string>(prompt);
                if (int.TryParse(input, out int number))
                {
                    return number;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Invalid number format. Please try again.[/]");
                }
            }
        }

        public CodingSession GetCodingSessionSelection(string prompt, List<CodingSession> codingSessions)
        {
            return AnsiConsole.Prompt(new SelectionPrompt<CodingSession>()
                .Title(prompt)
                .AddChoices(codingSessions)
                .UseConverter(session => $"ID: {session.SessionID}, Date: {session.Date}, Start: {session.StartTime}, End: {session.EndTime}, Duration: {session.Duration.ToString(@"hh\:mm")}")
            );
        }

        public bool GetUserConfirmation(string prompt)
        {
            return AnsiConsole.Confirm(prompt, false);
        }

        public void PressAnyKeyToContinue()
        {
            AnsiConsole.MarkupLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}

