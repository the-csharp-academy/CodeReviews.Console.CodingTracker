using System;
using System.Globalization;
using Spectre.Console;

namespace CodingTracker;

public class UserInput
{
    public static void GetUserInput()
    {
        bool closeApp = false;
        while (closeApp == false)
        {
            AnsiConsole.MarkupLine("\n[bold]Main Menu[/]");
            AnsiConsole.MarkupLine("[grey]Type 0 to Close Application[/]");
            AnsiConsole.MarkupLine("[grey]Type 1 to View All Records[/]");
            AnsiConsole.MarkupLine("[grey]Type 2 to Add New Record[/]");
            AnsiConsole.MarkupLine("[grey]Type 3 to Delete Record[/]");
            AnsiConsole.MarkupLine("[grey]Type 4 to Update Record[/]");

            string commandInput = AnsiConsole.Prompt(
                new TextPrompt<string>("What would you like to do?")
                    .PromptStyle("cyan"));

            switch (commandInput)
            {
                case "0":
                    AnsiConsole.MarkupLine("[green]Goodbye![/]");
                    closeApp = true;
                    break;
                case "1":
                    DataBase.GetAllRecords();
                    break;
                case "2":
                    DataBase.AddNewRecord();
                    break;
                case "3":
                    DataBase.DeleteRecord();
                    break;
                case "4":
                    DataBase.UpdateRecord();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid input. Please try again.[/]");
                    break;
            }
        }
    }

    public static string? GetTimeInput(string timeType)
    {
        string prompt = $"Please enter the [red]{timeType}[/] time in 24-hour format HH:mm (or type 0 to return to the main menu):";
        while (true)
        {
            string timeInput = AnsiConsole.Ask<string>(prompt);
            if (timeInput == "0") return null;

            if (DateTime.TryParseExact(timeInput, "HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                return timeInput;
            }

            AnsiConsole.MarkupLine($"[red]Invalid {timeType} time. Use HH:mm, for example 14:30.[/]");
        }
    }
}
