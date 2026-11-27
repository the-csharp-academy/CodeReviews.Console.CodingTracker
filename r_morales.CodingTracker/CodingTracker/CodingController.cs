using CodingTracker.Input;
using CodingTracker.Validation;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System.Diagnostics;

namespace CodingTracker.Controllers
{
    public class CodingController
    {
        public IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        public string? connectionString;
        UserInput userInput = new UserInput();


        bool seedDatabase;
        int numberOfSeedEntries;
        public CodingController()
        {
            connectionString = config.GetConnectionString("DataBasePath");
            seedDatabase = bool.Parse(config["Seeding:Enabled"] ?? "false");
            numberOfSeedEntries = int.Parse(config["Seeding:NumberOfEntries"] ?? "0");
        }

        public void SeedDatabase()
        {
            if (!seedDatabase)
            {
                return;
            }

            for (int i = 0; i < numberOfSeedEntries; i++)
            {
                var date = InputValidation.ValidateDateInput(DateOnly.FromDateTime(DateTime.Now.AddDays(-i)).ToString("MM-dd-yyyy"), out var parsedDate);
                var startTime = InputValidation.ValidateTimeInput(TimeOnly.FromDateTime(DateTime.Now.AddHours(-i * 2)).ToString("HH:mm"), out var parsedStartTime);
                var rnd = new Random();
                var endTime = InputValidation.ValidateTimeInput(TimeOnly.FromDateTime(DateTime.Now.AddHours(-i * 2 + rnd.Next(1, 6))).ToString("HH:mm"), out var parsedEndTime);
                var codingSession = new CodingSession() { StartTime = parsedStartTime, EndTime = parsedEndTime, Date = parsedDate };
                DataBaseController.InsertCodingSession(connectionString, codingSession);
            }
        }

        public void BootMenu()
        {
            AnsiConsole.Clear();
            MenuChoices menuChoice = userInput.GetMenuSelection();

            switch (menuChoice)
            {
                case MenuChoices.StartNewSession:
                    StartNewSession();
                    break;
                case MenuChoices.AddNewSession:
                    AddNewSession();
                    break;
                case MenuChoices.ViewSessions:
                    ViewSessions();
                    break;
                case MenuChoices.UpdateSession:
                    UpdateSession();
                    break;
                case MenuChoices.DeleteSession:
                    DeleteSession();
                    break;
                case MenuChoices.Exit:
                    ExitApplication();
                    break;
                default:
                    AnsiConsole.MarkupLine("Invalid choice. Please try again.");
                    BootMenu();
                    break;
            }
        }

        void StartNewSession()
        {
            var startedAt = DateTime.Now;
            AnsiConsole.MarkupLine("Starting a new coding session. Press any key to stop.");

            Stopwatch stopwatch = Stopwatch.StartNew();

            AnsiConsole.Live(new Markup("[bold green]00:00:00[/]"))
                .Start(ctx =>
                {
                    while (!Console.KeyAvailable)
                    {
                        ctx.UpdateTarget(new Markup($"[bold green]{stopwatch.Elapsed:hh\\:mm\\:ss}[/]"));
                    }

                    Console.ReadKey(intercept: true);
                });

            stopwatch.Stop();
            var endedAt = DateTime.Now;

            var date = InputValidation.ValidateDateInput(DateOnly.FromDateTime(startedAt).ToString("MM-dd-yyyy"), out var parsedDate);
            var startTime = InputValidation.ValidateTimeInput(TimeOnly.FromDateTime(startedAt).ToString("HH:mm"), out var parsedStartTime);
            var endTime = InputValidation.ValidateTimeInput(TimeOnly.FromDateTime(endedAt).ToString("HH:mm"), out var parsedEndTime);

            var codingSession = new CodingSession { Date = parsedDate, StartTime = parsedStartTime, EndTime = parsedEndTime };

            DataBaseController.InsertCodingSession(connectionString, codingSession);
            AnsiConsole.MarkupLine($"[green]Session saved successfully! Duration: {stopwatch.Elapsed:hh\\:mm\\:ss}[/]");

            userInput.PressAnyKeyToContinue();
            BootMenu();
        }

        void AddNewSession()
        {
            AnsiConsole.MarkupLine("Adding a new coding session...");

            var date = userInput.GetDateInput("Enter the date (MM-dd-yyyy):");
            var startTime = userInput.GetTimeInput("Enter the start time (HH:mm):");
            var endTime = userInput.GetTimeInput("Enter the end time (HH:mm):");

            while (endTime <= startTime)
            {
                AnsiConsole.MarkupLine("[red]End time must be after start time. Please try again.[/]");
                endTime = userInput.GetTimeInput("Enter the end time (HH:mm):");
            }

            var codingSession = new CodingSession() { StartTime = startTime, EndTime = endTime, Date = date };
            DataBaseController.InsertCodingSession(connectionString, codingSession);
            AnsiConsole.MarkupLine($"[green]Session added successfully! Duration: {codingSession.Duration.ToString(@"hh\:mm")}[/]");
            userInput.PressAnyKeyToContinue();
            BootMenu();
        }

        void ViewSessions()
        {
            AnsiConsole.MarkupLine("Viewing coding sessions...");

            var codingSessions = DataBaseController.GetAllCodingSessions(connectionString);

            if (codingSessions.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No coding sessions found.[/]");
            }
            else
            {
                var table = new Table();
                table.AddColumn("Session ID");
                table.AddColumn("Date");
                table.AddColumn("Start Time");
                table.AddColumn("End Time");
                table.AddColumn("Duration");
                foreach (var session in codingSessions)
                {
                    table.AddRow(session.SessionID.ToString(), session.Date.ToString(), session.StartTime.ToString(), session.EndTime.ToString(), session.Duration.ToString(@"hh\:mm"));
                }
                AnsiConsole.Write(table);
            }

            userInput.PressAnyKeyToContinue();
            BootMenu();
        }

        void UpdateSession()
        {
            AnsiConsole.MarkupLine("Updating a coding session...");

            var codingSessions = DataBaseController.GetAllCodingSessions(connectionString);

            if (codingSessions.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No coding sessions found to update.[/]");
            }
            else
            {
                var session = userInput.GetCodingSessionSelection("Select a session to update:", codingSessions);

                bool updating = true;
                while (updating)
                {
                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine($"[yellow]Updating Session[/]\nDate:{session.Date} - Start Time:{session.StartTime} - End Time {session.EndTime}");
                    var fieldToUpdate = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Select the field to update:")
                    .AddChoices(["Date", "Start Time", "End Time", "Done"]));

                    switch (fieldToUpdate)
                    {
                        case "Date":
                            session.Date = userInput.GetDateInput("Enter the new date (MM-dd-yyyy):");
                            break;
                        case "Start Time":
                            session.StartTime = userInput.GetTimeInput("Enter the new start time (HH:mm):");
                            break;
                        case "End Time":
                            session.EndTime = userInput.GetTimeInput("Enter the new end time (HH:mm):");
                            while (session.EndTime <= session.StartTime)
                            {
                                AnsiConsole.MarkupLine("[red]End time must be after start time. Please try again.[/]");
                                session.EndTime = userInput.GetTimeInput("Enter the new end time (HH:mm):");
                            }
                            break;
                        case "Done":
                            updating = false;
                            break;
                    }
                }

                DataBaseController.UpdateCodingSession(connectionString, session);
            }

            BootMenu();
        }

        void DeleteSession()
        {
            AnsiConsole.MarkupLine("Deleting a coding session...");

            var codingSessions = DataBaseController.GetAllCodingSessions(connectionString);

            if (codingSessions.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No coding sessions found to update.[/]");
            }
            else
            {
                var session = userInput.GetCodingSessionSelection("Select an entry to delete:", codingSessions);

                AnsiConsole.MarkupLine($"[red]Deleting Session\nDate:{session.Date} - Start Time:{session.StartTime} - End Time {session.EndTime}[/]");

                if (userInput.GetUserConfirmation("Are you sure you want to delete the record?"))
                {
                    DataBaseController.DeleteCodingSessionByID(connectionString, session.SessionID);
                    AnsiConsole.MarkupLine("[green]Coding Session succesfully deleted.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Aborting operation.[/]");
                }
            }

            userInput.PressAnyKeyToContinue();
            BootMenu();
        }

        void ExitApplication()
        {
            AnsiConsole.MarkupLine("Exiting the application. Goodbye!");
        }
    }
}